/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

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
        UseUseDisplayName = 1024


    }
}




