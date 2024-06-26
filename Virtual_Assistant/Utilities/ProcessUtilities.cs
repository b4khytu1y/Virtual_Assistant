﻿using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using SharpHook.Native;

namespace Virtual_Assistant.Utilities;

public class ProcessUtilities
{
    public async Task<IEnumerable<Process>> GetProcessesRunningInFolderAsync(string folderPath)
    {
        var allProcesses = Process.GetProcesses();

        var processesInFolder = await Task.Run(() =>
        {
            return allProcesses.Where(p =>
            {
                try
                {
                    string processPath = p.MainModule.FileName;

                    return Path.GetDirectoryName(processPath)
                        .StartsWith(Path.GetFullPath(folderPath), StringComparison.OrdinalIgnoreCase);
                }
                catch (Exception)     
                {
                    return false;
                }
            });
        });

        return processesInFolder;
    }
}