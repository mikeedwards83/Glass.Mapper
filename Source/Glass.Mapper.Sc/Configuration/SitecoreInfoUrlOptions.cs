using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Configuration
{
    [Flags]
    public enum SitecoreInfoUrlOptions
    {
        Default = 0x00,
        AddAspxExtension = 0x01,
        AlwaysIncludeServerUrl = 0x02,
        EncodeNames = 0x04,

        /// <summary>
        /// Do not use with LanguageEmbeddingAsNeeded, LanguageEmbeddingNever
        /// </summary>
        LanguageEmbeddingAlways = 0x08,

        /// <summary>
        /// Do not use with LanguageEmbeddingAlways, LanguageEmbeddingNever
        /// </summary>
        LanguageEmbeddingAsNeeded = 0x16,

        /// <summary>
        /// Do not use with LanguageEmbeddingAlways, LanguageEmbeddingAsNeeded
        /// </summary>
        LanguageEmbeddingNever = 0x32,

        /// <summary>
        /// Do not use with LanguageLocationQueryString
        /// </summary>
        LanguageLocationFilePath = 0x64,

        /// <summary>
        /// Do not use with LanguageLocationFilePath
        /// </summary>
        LanguageLocationQueryString = 0x128,

        ShortenUrls = 0x256,
        SiteResolving = 0x512,
        UseUseDisplayName = 0x1024


    }
}
