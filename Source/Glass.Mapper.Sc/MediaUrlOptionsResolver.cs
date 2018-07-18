using Glass.Mapper.Sc.Configuration;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc
{
    public class MediaUrlOptionsResolver : IMediaUrlOptionsResolver
    {

        public virtual MediaUrlOptions GetMediaUrlOptions(SitecoreInfoMediaUrlOptions mediaUrlOptions)
        {
            var defaultMediaUrl = MediaUrlOptions.Empty;

            if (mediaUrlOptions == 0) return defaultMediaUrl;

            bool FlagCheck(SitecoreInfoMediaUrlOptions option) => (mediaUrlOptions & option) == option;

            defaultMediaUrl.AbsolutePath = !FlagCheck(SitecoreInfoMediaUrlOptions.DisableAbsolutePath) && defaultMediaUrl.AbsolutePath;
            defaultMediaUrl.AllowStretch = FlagCheck(SitecoreInfoMediaUrlOptions.AllowStretch) || defaultMediaUrl.AllowStretch;
            defaultMediaUrl.AlwaysIncludeServerUrl = FlagCheck(SitecoreInfoMediaUrlOptions.AlwaysIncludeServerUrl) || defaultMediaUrl.AlwaysIncludeServerUrl;
            defaultMediaUrl.DisableBrowserCache = FlagCheck(SitecoreInfoMediaUrlOptions.DisableBrowserCache) || defaultMediaUrl.DisableBrowserCache;
            defaultMediaUrl.DisableMediaCache = FlagCheck(SitecoreInfoMediaUrlOptions.DisableMediaCache) || defaultMediaUrl.DisableMediaCache;
            defaultMediaUrl.IgnoreAspectRatio = FlagCheck(SitecoreInfoMediaUrlOptions.IgnoreAspectRatio) || defaultMediaUrl.IgnoreAspectRatio;
            defaultMediaUrl.IncludeExtension = !FlagCheck(SitecoreInfoMediaUrlOptions.RemoveExtension) && defaultMediaUrl.IncludeExtension;
            defaultMediaUrl.LowercaseUrls = FlagCheck(SitecoreInfoMediaUrlOptions.LowercaseUrls) || defaultMediaUrl.LowercaseUrls;
            defaultMediaUrl.Thumbnail = FlagCheck(SitecoreInfoMediaUrlOptions.Thumbnail) || defaultMediaUrl.Thumbnail;
            defaultMediaUrl.UseDefaultIcon = FlagCheck(SitecoreInfoMediaUrlOptions.UseDefaultIcon) || defaultMediaUrl.UseDefaultIcon;
            defaultMediaUrl.UseItemPath = FlagCheck(SitecoreInfoMediaUrlOptions.UseItemPath) || defaultMediaUrl.UseItemPath;

            // defaultMediaUrl.BackgroundColor 
            // defaultMediaUrl.Database 
            // defaultMediaUrl.Height 
            // defaultMediaUrl.DefaultIcon
            // defaultMediaUrl.ItemRevision 
            //defaultMediaUrl.Language;
            //defaultMediaUrl.MaxHeight;
            //defaultMediaUrl.MaxWidth;
            //defaultMediaUrl.MediaLinkServerUrl;
            //defaultMediaUrl.RequestExtension;
            //defaultMediaUrl.Scale;
            //defaultMediaUrl.Version;
            //defaultMediaUrl.VirtualFolder;
            //defaultMediaUrl.Width;

            return defaultMediaUrl;
        }
        public virtual MediaUrlOptions GetMediaUrlOptions(SitecoreMediaUrlOptions mediaUrlOptions)
        {
            var defaultMediaUrl = MediaUrlOptions.Empty;

            if (mediaUrlOptions == 0) return defaultMediaUrl;

            bool FlagCheck(SitecoreMediaUrlOptions option) => (mediaUrlOptions & option) == option;

            defaultMediaUrl.AbsolutePath = !FlagCheck(SitecoreMediaUrlOptions.DisableAbsolutePath) && defaultMediaUrl.AbsolutePath;
            defaultMediaUrl.AllowStretch = FlagCheck(SitecoreMediaUrlOptions.AllowStretch) || defaultMediaUrl.AllowStretch;
            defaultMediaUrl.AlwaysIncludeServerUrl = FlagCheck(SitecoreMediaUrlOptions.AlwaysIncludeServerUrl) || defaultMediaUrl.AlwaysIncludeServerUrl;
            defaultMediaUrl.DisableBrowserCache = FlagCheck(SitecoreMediaUrlOptions.DisableBrowserCache) || defaultMediaUrl.DisableBrowserCache;
            defaultMediaUrl.DisableMediaCache = FlagCheck(SitecoreMediaUrlOptions.DisableMediaCache) || defaultMediaUrl.DisableMediaCache;
            defaultMediaUrl.IgnoreAspectRatio = FlagCheck(SitecoreMediaUrlOptions.IgnoreAspectRatio) || defaultMediaUrl.IgnoreAspectRatio;
            defaultMediaUrl.IncludeExtension = !FlagCheck(SitecoreMediaUrlOptions.RemoveExtension) && defaultMediaUrl.IncludeExtension;
            defaultMediaUrl.LowercaseUrls = FlagCheck(SitecoreMediaUrlOptions.LowercaseUrls) || defaultMediaUrl.LowercaseUrls;
            defaultMediaUrl.Thumbnail = FlagCheck(SitecoreMediaUrlOptions.Thumbnail) || defaultMediaUrl.Thumbnail;
            defaultMediaUrl.UseDefaultIcon = FlagCheck(SitecoreMediaUrlOptions.UseDefaultIcon) || defaultMediaUrl.UseDefaultIcon;
            defaultMediaUrl.UseItemPath = FlagCheck(SitecoreMediaUrlOptions.UseItemPath) || defaultMediaUrl.UseItemPath;

            // defaultMediaUrl.BackgroundColor 
            // defaultMediaUrl.Database 
            // defaultMediaUrl.Height 
            // defaultMediaUrl.DefaultIcon
            // defaultMediaUrl.ItemRevision 
            //defaultMediaUrl.Language;
            //defaultMediaUrl.MaxHeight;
            //defaultMediaUrl.MaxWidth;
            //defaultMediaUrl.MediaLinkServerUrl;
            //defaultMediaUrl.RequestExtension;
            //defaultMediaUrl.Scale;
            //defaultMediaUrl.Version;
            //defaultMediaUrl.VirtualFolder;
            //defaultMediaUrl.Width;

            return defaultMediaUrl;
        }
    }
}
