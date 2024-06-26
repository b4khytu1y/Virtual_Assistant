﻿using System.IO;

namespace Virtual_Assistant.Utilities;

public static class FileUtilities
{
    public static async Task CopyFileAsync(string sourceFile, string destinationFile)
    {
        await using (var sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                         FileOptions.Asynchronous | FileOptions.SequentialScan))
        await using (var destinationStream = new FileStream(destinationFile, FileMode.CreateNew, FileAccess.Write,
                         FileShare.None, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan))
            await sourceStream.CopyToAsync(destinationStream);
    }
}