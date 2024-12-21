﻿using System.Collections.Generic;

namespace EpicLibrary.Models
{
    public class LauncherInstalled
    {
        public class InstalledApp
        {
            public string InstallLocation;
            public string AppName;
            public long AppID;
            public string AppVersion;
        }

        public List<InstalledApp> InstallationList;
    }

    public class InstalledManifiest
    {
        public string LaunchCommand;
        public string LaunchExecutable;
        public string ManifestLocation;
        public bool bIsApplication;
        public bool bIsExecutable;
        public bool bRequiresAuth;
        public bool bCanRunOffline;
        public string AppName;
        public string CatalogNamespace;
        public List<string> AppCategories;
        public List<string> CompatibleApps;
        public string DisplayName;
        public string FullAppName;
        public string InstallationGuid;
        public string InstallLocation;
        public string InstallSessionId;
        public string StagingLocation;
        public string TechnicalType;
        public string VaultThumbnailUrl;
        public string VaultTitleText;
        public string InstallSize;
        public string MainWindowProcessName;
        public List<string> ProcessNames;
        public string MainGameAppName;
        public string MandatoryAppFolderName;
    }
}
