﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Virtual_Assistant.Models;

[Table("characters")]
public class RoleplayCharacter
{
    [Key] public long Id { get; set; }

    public required string CharacterName { get; set; }

    public string SampleMessages { get; set; } = string.Empty;

    // optional
    public string? ElevenlabsSelectedVoice { get; set; } = "Charlotte";

    public string Description { get; set; } = string.Empty;

    public string ProfilePictureHash { get; set; } = "default.jpg";

    public bool IsCharacterAi { get; set; }

    public string? CharacterAiId { get; set; } = null;

    public string? CharacterAiTargetPersona { get; set; } = null;

    public string ProfilePictureFilePath => Path.Combine(Constants.ProfilePicturesFolder, ProfilePictureHash);

    public List<ChatChannel> CharacterChannels { get; set; } = new();
}