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
