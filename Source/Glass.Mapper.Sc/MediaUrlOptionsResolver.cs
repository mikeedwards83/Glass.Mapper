using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Func<SitecoreInfoMediaUrlOptions, bool> flagCheck =
                option => (mediaUrlOptions & option) == option;

            defaultMediaUrl.AbsolutePath = !flagCheck(SitecoreInfoMediaUrlOptions.DisableAbsolutePath) && defaultMediaUrl.AbsolutePath;
            defaultMediaUrl.AllowStretch = flagCheck(SitecoreInfoMediaUrlOptions.AllowStretch) || defaultMediaUrl.AllowStretch;
            defaultMediaUrl.AlwaysIncludeServerUrl = flagCheck(SitecoreInfoMediaUrlOptions.AlwaysIncludeServerUrl) || defaultMediaUrl.AlwaysIncludeServerUrl;
            defaultMediaUrl.DisableBrowserCache = flagCheck(SitecoreInfoMediaUrlOptions.DisableBrowserCache) || defaultMediaUrl.DisableBrowserCache;
            defaultMediaUrl.DisableMediaCache = flagCheck(SitecoreInfoMediaUrlOptions.DisableMediaCache) || defaultMediaUrl.DisableMediaCache;
            defaultMediaUrl.IgnoreAspectRatio = flagCheck(SitecoreInfoMediaUrlOptions.IgnoreAspectRatio) || defaultMediaUrl.IgnoreAspectRatio;
            defaultMediaUrl.IncludeExtension = !flagCheck(SitecoreInfoMediaUrlOptions.RemoveExtension) && defaultMediaUrl.IncludeExtension;
            defaultMediaUrl.LowercaseUrls = flagCheck(SitecoreInfoMediaUrlOptions.LowercaseUrls) || defaultMediaUrl.LowercaseUrls;
            defaultMediaUrl.Thumbnail = flagCheck(SitecoreInfoMediaUrlOptions.Thumbnail) || defaultMediaUrl.Thumbnail;
            defaultMediaUrl.UseDefaultIcon = flagCheck(SitecoreInfoMediaUrlOptions.UseDefaultIcon) || defaultMediaUrl.UseDefaultIcon;
            defaultMediaUrl.UseItemPath = flagCheck(SitecoreInfoMediaUrlOptions.UseItemPath) || defaultMediaUrl.UseItemPath;

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
