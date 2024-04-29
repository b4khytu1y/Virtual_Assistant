using System.IO;
using System.Text;
using LLama;
using LLama.Common;
using Virtual_Assistant.Models;

namespace Virtual_Assistant.ChatHandlers;

public class LocalLlama : IChatHandler
{
    private readonly Data.Settings _settings;
    private readonly PersonaSingle _personaSingle;
    private readonly RoleplayCharacter _roleplayCharacter;

    public event EventHandler<ChatMessage>? CompleteMessageGenerated;

    public event EventHandler<string>? PartialMessageGenerated;
    public ChatChannel ChatChannel { get; set; }

    public ModelParams Parameters { get; private set; }

    public static LLamaWeights? Weights { get; private set; } = null;

    public static LLamaContext? LLamaContext { get; private set; } = null;

    public ChatSession ChatSession { get; private set; }

    public InteractiveExecutor InteractiveExecutor { get; private set; }

    public event EventHandler ModelLoaded;

    public LocalLlama(ChatChannel chatChannel, Data.Settings settings, PersonaSingle personaSingle,
        RoleplayCharacter roleplayCharacter)
    {
        _settings = settings;
        _personaSingle = personaSingle;
        _roleplayCharacter = roleplayCharacter;
        ChatChannel = chatChannel;
    }

    private SemaphoreSlim _semaphoreSlim = new(1);

    private bool isInitialized = false;

    public async Task InitializeAsync()
    {
        await _semaphoreSlim.WaitAsync();

        if (isInitialized)
            return;

        if (Weights is null) // only load weights once~
            await Task.Run(() => { Weights = LLamaWeights.LoadFromFile(Parameters); });

        var settings = await _settings.GetOrCreateSettings();

        Parameters = new(Path.GetFullPath(settings.LocalModel))
        {
            ContextSize = 1024,
            Seed = (uint)Random.Shared.Next(), GpuLayerCount = settings.GpuLayerCount
        };

        LLamaContext ??= Weights.CreateContext(Parameters);

        InteractiveExecutor = new InteractiveExecutor(LLamaContext);

        ChatSession = new ChatSession(InteractiveExecutor);

        // add the character
        ChatSession.AddMessage(new ChatHistory.Message(AuthorRole.System,
            $"your name is {_roleplayCharacter.CharacterName}, {_roleplayCharacter.Description}"));

        _semaphoreSlim.Release();

        isInitialized = true;
    }

    public Task<string?> SendMessageAndGetResultAsync(ChatMessage message)
    {
        throw new NotImplementedException();
    }
   
}