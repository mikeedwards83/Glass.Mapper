using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Links;

namespace Glass.Mapper.Sc
{
    public static class Utilities
    {
        public static UrlOptions CreateUrlOptions(SitecoreInfoUrlOptions urlOptions)
        {
            UrlOptions defaultUrl = UrlOptions.DefaultOptions;

            if (urlOptions == 0) return defaultUrl;

            var t = (urlOptions & SitecoreInfoUrlOptions.AddAspxExtension);

            Func<SitecoreInfoUrlOptions, bool> flagCheck =
                (SitecoreInfoUrlOptions option) => (urlOptions & option) == option;


            //check for any default overrides
            defaultUrl.AddAspxExtension = flagCheck(SitecoreInfoUrlOptions.AddAspxExtension) ? true : defaultUrl.AddAspxExtension;
            defaultUrl.AlwaysIncludeServerUrl = flagCheck(SitecoreInfoUrlOptions.AlwaysIncludeServerUrl) ? true : defaultUrl.AlwaysIncludeServerUrl;
            defaultUrl.EncodeNames = flagCheck(SitecoreInfoUrlOptions.EncodeNames) ? true : defaultUrl.EncodeNames;
            defaultUrl.ShortenUrls = flagCheck(SitecoreInfoUrlOptions.ShortenUrls) ? true : defaultUrl.ShortenUrls;
            defaultUrl.SiteResolving = flagCheck(SitecoreInfoUrlOptions.SiteResolving) ? true : defaultUrl.SiteResolving;
            defaultUrl.UseDisplayName =flagCheck(SitecoreInfoUrlOptions.UseUseDisplayName) ? true : defaultUrl.UseDisplayName;


            if (flagCheck(SitecoreInfoUrlOptions.LanguageEmbeddingAlways))
                defaultUrl.LanguageEmbedding = LanguageEmbedding.Always;
            else if (flagCheck(SitecoreInfoUrlOptions.LanguageEmbeddingAsNeeded))
                defaultUrl.LanguageEmbedding = LanguageEmbedding.AsNeeded;
            else if (flagCheck(SitecoreInfoUrlOptions.LanguageEmbeddingNever))
                defaultUrl.LanguageEmbedding = LanguageEmbedding.Never;

            if (flagCheck(SitecoreInfoUrlOptions.LanguageLocationFilePath))
                defaultUrl.LanguageLocation = LanguageLocation.FilePath;
            else if (flagCheck(SitecoreInfoUrlOptions.LanguageLocationQueryString))
                defaultUrl.LanguageLocation = LanguageLocation.QueryString;

            return defaultUrl;

        }

    }
}
