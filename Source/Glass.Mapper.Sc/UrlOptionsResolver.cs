using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.IoC;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc
{
    public class UrlOptionsResolver : IUrlOptionsResolver
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

        /// <summary>
        /// Creates the URL options.
        /// </summary>
        /// <param name="urlOptions">The URL options.</param>
        /// <returns>UrlOptions.</returns>
        public virtual UrlOptions CreateUrlOptions(SitecoreInfoUrlOptions urlOptions)
        {
            UrlOptions defaultUrl = UrlOptions.DefaultOptions;

            return CreateUrlOptions(urlOptions, defaultUrl);
        }

        public virtual UrlOptions CreateUrlOptions(SitecoreInfoUrlOptions urlOptions, UrlOptions defaultOptions)
        {
            if (urlOptions == 0) return defaultOptions;

            Func<SitecoreInfoUrlOptions, bool> flagCheck =
                option => (urlOptions & option) == option;


            //check for any default overrides
            defaultOptions.AddAspxExtension = flagCheck(SitecoreInfoUrlOptions.AddAspxExtension) || defaultOptions.AddAspxExtension;
            defaultOptions.AlwaysIncludeServerUrl = flagCheck(SitecoreInfoUrlOptions.AlwaysIncludeServerUrl) || defaultOptions.AlwaysIncludeServerUrl;
            defaultOptions.EncodeNames = flagCheck(SitecoreInfoUrlOptions.EncodeNames) || defaultOptions.EncodeNames;
            defaultOptions.ShortenUrls = flagCheck(SitecoreInfoUrlOptions.ShortenUrls) || defaultOptions.ShortenUrls;
            defaultOptions.SiteResolving = flagCheck(SitecoreInfoUrlOptions.SiteResolving) || defaultOptions.SiteResolving;
            defaultOptions.UseDisplayName = flagCheck(SitecoreInfoUrlOptions.UseUseDisplayName) || defaultOptions.UseDisplayName;


            if (flagCheck(SitecoreInfoUrlOptions.LanguageEmbeddingAlways))
                defaultOptions.LanguageEmbedding = LanguageEmbedding.Always;
            else if (flagCheck(SitecoreInfoUrlOptions.LanguageEmbeddingAsNeeded))
                defaultOptions.LanguageEmbedding = LanguageEmbedding.AsNeeded;
            else if (flagCheck(SitecoreInfoUrlOptions.LanguageEmbeddingNever))
                defaultOptions.LanguageEmbedding = LanguageEmbedding.Never;

            if (flagCheck(SitecoreInfoUrlOptions.LanguageLocationFilePath))
                defaultOptions.LanguageLocation = LanguageLocation.FilePath;
            else if (flagCheck(SitecoreInfoUrlOptions.LanguageLocationQueryString))
                defaultOptions.LanguageLocation = LanguageLocation.QueryString;

            return defaultOptions;

        }
    }
}
