using System;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Enum SitecoreInfoUrlOptions
    /// </summary>
    [Flags]
    public enum SitecoreInfoUrlOptions
    {
        /// <summary>
        /// The default
        /// </summary>
        Default = 0,
        /// <summary>
        /// The add aspx extension
        /// </summary>
        AddAspxExtension = 1,
        /// <summary>
        /// The always include server URL
        /// </summary>
        AlwaysIncludeServerUrl = 2,
        /// <summary>
        /// The encode names
        /// </summary>
        EncodeNames = 4,

        /// <summary>
        /// Do not use with LanguageEmbeddingAsNeeded, LanguageEmbeddingNever
        /// </summary>
        LanguageEmbeddingAlways = 8,

        /// <summary>
        /// Do not use with LanguageEmbeddingAlways, LanguageEmbeddingNever
        /// </summary>
        LanguageEmbeddingAsNeeded = 16,

        /// <summary>
        /// Do not use with LanguageEmbeddingAlways, LanguageEmbeddingAsNeeded
        /// </summary>
        LanguageEmbeddingNever = 32,

        /// <summary>
        /// Do not use with LanguageLocationQueryString
        /// </summary>
        LanguageLocationFilePath = 64,

        /// <summary>
        /// Do not use with LanguageLocationFilePath
        /// </summary>
        LanguageLocationQueryString = 128,

        /// <summary>
        /// The shorten urls
        /// </summary>
        ShortenUrls = 256,
        /// <summary>
        /// The site resolving
        /// </summary>
        SiteResolving = 512,
        /// <summary>
        /// The use use display name
        /// </summary>
        UseUseDisplayName = 1024,
        /// <summary>
        /// Use the item language to generate the URL and not the context language
        /// </summary>
        UseItemLanguage = 2048

    }
}




