﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Virtual_Assistant.Models;

[Table("channels")]
public class ChatChannel
{
    [Key] public long Id { get; set; }

    public List<RoleplayCharacter> Characters { get; set; } = new();

    public List<ChatMessage> Messages { get; set; } = new();

    public string? CharacterAiHistoryId { get; set; } = default;
}