using System.Reflection;
using Microsoft.Extensions.Logging;
using Virtual_Assistant.ChatHandlers;
using Virtual_Assistant.Models;

namespace Virtual_Assistant.Data;

public class ChatServiceManager
{
    private readonly ILogger<ChatServiceManager> _logger;
    private readonly Settings _settings;

    public ChatServiceManager(ILogger<ChatServiceManager> logger, Settings settings)
    {
        _logger = logger;
        _settings = settings;
    }


    public Type? GetEnabledChatServiceForCharacter(RoleplayCharacter roleplayCharacter)
    {
        if (roleplayCharacter.IsCharacterAi)
            return typeof(CharacterAiChatHandler);

        return typeof(LocalLlama);
    }
}