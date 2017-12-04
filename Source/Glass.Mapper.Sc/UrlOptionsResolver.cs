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
            //We can test this because it throws an error due to config being missing
            UrlOptions defaultUrl = UrlOptions.DefaultOptions;

            return CreateUrlOptions(urlOptions, defaultUrl);
        }

        public static bool FlagCheck(SitecoreInfoUrlOptions urlOptions, SitecoreInfoUrlOptions option)
        {
            return (urlOptions & option) == option;
        }
        public virtual UrlOptions CreateUrlOptions(SitecoreInfoUrlOptions urlOptions, UrlOptions defaultOptions)
        {
            if (urlOptions == 0) return defaultOptions;

            //check for any default overrides
            defaultOptions.AddAspxExtension = FlagCheck(urlOptions, SitecoreInfoUrlOptions.AddAspxExtension) || defaultOptions.AddAspxExtension;
            defaultOptions.AlwaysIncludeServerUrl =  FlagCheck(urlOptions, SitecoreInfoUrlOptions.AlwaysIncludeServerUrl) || defaultOptions.AlwaysIncludeServerUrl;
            defaultOptions.EncodeNames =  FlagCheck(urlOptions, SitecoreInfoUrlOptions.EncodeNames) || defaultOptions.EncodeNames;
            defaultOptions.ShortenUrls =  FlagCheck(urlOptions, SitecoreInfoUrlOptions.ShortenUrls) || defaultOptions.ShortenUrls;
            defaultOptions.SiteResolving =  FlagCheck(urlOptions, SitecoreInfoUrlOptions.SiteResolving) || defaultOptions.SiteResolving;
            defaultOptions.UseDisplayName =  FlagCheck(urlOptions, SitecoreInfoUrlOptions.UseUseDisplayName) || defaultOptions.UseDisplayName;


            if ( FlagCheck(urlOptions, SitecoreInfoUrlOptions.LanguageEmbeddingAlways))
                defaultOptions.LanguageEmbedding = LanguageEmbedding.Always;
            else if ( FlagCheck(urlOptions, SitecoreInfoUrlOptions.LanguageEmbeddingAsNeeded))
                defaultOptions.LanguageEmbedding = LanguageEmbedding.AsNeeded;
            else if ( FlagCheck(urlOptions, SitecoreInfoUrlOptions.LanguageEmbeddingNever))
                defaultOptions.LanguageEmbedding = LanguageEmbedding.Never;

            if ( FlagCheck(urlOptions, SitecoreInfoUrlOptions.LanguageLocationFilePath))
                defaultOptions.LanguageLocation = LanguageLocation.FilePath;
            else if ( FlagCheck(urlOptions, SitecoreInfoUrlOptions.LanguageLocationQueryString))
                defaultOptions.LanguageLocation = LanguageLocation.QueryString;

            return defaultOptions;

        }
    }
}
