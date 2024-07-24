﻿using System.IO;
using Playnite.SDK.Data;

namespace OriginLibrary
{
    public class EasyAntiCheatLauncherSettings
    {
        public string title { get; set; }
        public string executable { get; set; }
        public string parameters { get; set; }
        public string use_cmdline_parameters { get; set; }
        public string working_directory { get; set; }
    }

    public class EasyAntiCheat
    {
        public static EasyAntiCheatLauncherSettings GetLauncherSettings(string gameDirectory)
        {
            var settingsPath = Path.Combine(gameDirectory, "EasyAntiCheat", "Launcher", "Settings.json");
            if (!File.Exists(settingsPath))
            {
                throw new FileNotFoundException($"EAC launcher settings not found: {settingsPath}");
            }

            return Serialization.FromJson<EasyAntiCheatLauncherSettings>(File.ReadAllText(settingsPath));
        }
    }
}
