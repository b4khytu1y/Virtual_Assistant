﻿using System.IO;

namespace Virtual_Assistant;

public static class Constants
{
    public const string LocalResourcesFolder = "Resources";

    public const string ProfilePicturesFolder = $"{LocalResourcesFolder}\\Images";

    public const string ModelsFolder = $"{LocalResourcesFolder}\\Models";

    public const string DatabasePath = $"{LocalResourcesFolder}\\database.db";

    public const string DefaultProfilePicture = $".\\{ProfilePicturesFolder}\\default.jpg";
    public const string DefaultLowIcon = $".\\{ProfilePicturesFolder}\\128_Icon.png";

    public static readonly string DataFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Alizer", "Virtual_Assistant");

    public const string UpdateJson = "./update.json";

}