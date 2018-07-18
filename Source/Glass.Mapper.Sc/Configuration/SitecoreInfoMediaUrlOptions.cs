using System;

namespace Glass.Mapper.Sc.Configuration
{
    [Flags]
    public enum SitecoreInfoMediaUrlOptions
    {
        /// <summary>
        /// The default
        /// </summary>
        Default = 0,
        DisableAbsolutePath =2 ,
        AllowStretch =4,
        AlwaysIncludeServerUrl =8,
        DisableBrowserCache=16,
        DisableMediaCache=32,
        IgnoreAspectRatio=64,
        RemoveExtension=128,
        LowercaseUrls=256,
        Thumbnail=512,
        UseDefaultIcon=1024,
        UseItemPath=2048
    }

    [Flags]
    public enum SitecoreMediaUrlOptions
    {
        /// <summary>
        /// The default
        /// </summary>
        Default = 0,
        DisableAbsolutePath = 2,
        AllowStretch = 4,
        AlwaysIncludeServerUrl = 8,
        DisableBrowserCache = 16,
        DisableMediaCache = 32,
        IgnoreAspectRatio = 64,
        RemoveExtension = 128,
        LowercaseUrls = 256,
        Thumbnail = 512,
        UseDefaultIcon = 1024,
        UseItemPath = 2048
    }
}
