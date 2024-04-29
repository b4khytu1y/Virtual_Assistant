using CharacterAI.Client;
using LLama.Common;
using Virtual_Assistant.Data;
using Virtual_Assistant.Models;
using Data_Settings = Virtual_Assistant.Data.Settings;
using Settings = Virtual_Assistant.Data.Settings;

namespace Virtual_Assistant.ChatHandlers;

public class CharacterAiChatHandler : IChatHandler
{
    private readonly Data_Settings _settings;
    private readonly CharacterAiApi _characterAiApi;
    private readonly RoleplayCharacter _roleplayCharacter;
    private readonly Messages _messages;
    public event EventHandler<ChatMessage>? CompleteMessageGenerated;

    public event EventHandler<string>? PartialMessageGenerated;

    public ChatChannel ChatChannel { get; set; }


    public CharacterAiChatHandler(Data_Settings settings, CharacterAiApi characterAiApi,
        Messages messages, ChatChannel chatChannel, RoleplayCharacter roleplayCharacter)
    {
        _settings = settings;
        _characterAiApi = characterAiApi;
        _messages = messages;
        _roleplayCharacter = roleplayCharacter;
        ChatChannel = chatChannel;
    }

    private bool isInitialized = false;
    private SemaphoreSlim initSemaphore = new(1);

    public async Task InitializeAsync()
    {
        await initSemaphore.WaitAsync();

        if (isInitialized)
            return;

        ChatChannel = await _messages.GetOrCreateChannelWithCharacter(_roleplayCharacter);

        isInitialized = true;

        initSemaphore.Release();
    }

    public async Task<string?> SendMessageAndGetResultAsync(ChatMessage message)
    {
        if (!isInitialized)
            await InitializeAsync();

        var savedMessage = await _messages.AddMessageAsync(message);

        var characterResponse = await _characterAiApi.SendMessageAsync(_roleplayCharacter, ChatChannel, savedMessage);

        var savedResponseMessage = await _messages.AddMessageAsync(new ChatMessage()
        {
            Sender = _roleplayCharacter.Id,
            ChatChannel = ChatChannel,
            Message = characterResponse.Response?.Text ?? string.Empty
        });

        CompleteMessageGenerated?.Invoke(this, savedResponseMessage);

        return savedResponseMessage.Message;
    }
}