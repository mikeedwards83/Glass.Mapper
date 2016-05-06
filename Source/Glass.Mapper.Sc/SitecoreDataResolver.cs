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
    public interface IUrlOptionsResolver
    {
        /// <summary>
        /// Creates the URL options.
        /// </summary>
        /// <param name="urlOptions">The URL options.</param>
        /// <returns>UrlOptions.</returns>
        UrlOptions CreateUrlOptions(SitecoreInfoUrlOptions urlOptions);

        UrlOptions CreateUrlOptions(SitecoreInfoUrlOptions urlOptions, UrlOptions defaultOptions);

        MediaUrlOptions GetMediaUrlOptions(SitecoreInfoMediaUrlOptions mediaUrlOptions);
    }

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

    public interface ISitecoreFieldResolver
    {
        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>Field.</returns>
        Field GetField(Item item, string fieldName);

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldId">Id of the field.</param>
        /// <returns>Field.</returns>
        Field GetField(Item item, ID fieldId);

        Field GetSitecoreField(Item item, SitecoreFieldConfiguration scConfig);
    }

    public class SitecoreFieldResolver : ISitecoreFieldResolver
    {
        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>Field.</returns>
        public virtual Field GetField(Item item, string fieldName)
        {
            ValidateItem(item);
            return item.Fields[fieldName];
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldId">Id of the field.</param>
        /// <returns>Field.</returns>
        public virtual Field GetField(Item item, ID fieldId)
        {
            ValidateItem(item);

            if (ID.IsNullOrEmpty(fieldId))
            {
                return null;
            }

            return item.Fields[fieldId];
        }

        protected virtual void ValidateItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item is null");
            }
        }

        public virtual Field GetSitecoreField(Item item, SitecoreFieldConfiguration scConfig)
        {
            Field field = GetField(item, scConfig.FieldId);

            if (field != null)
            {
                return field;
            }

            field = GetField(item, scConfig.FieldName);
            if (field != null)
            {
                return field;
            }

            // NM: Seperated to reduce the level of overrides that would be required from extension
            return GetAlternativeSitecoreField(item, scConfig);
        }

        protected virtual Field GetAlternativeSitecoreField(Item item, SitecoreFieldConfiguration scConfig)
        {
            string spaceFieldName = GetFieldName(scConfig.FieldName);
            Field field = GetField(item, spaceFieldName);
            if (field != null)
            {
                scConfig.FieldName = spaceFieldName;
            }

            return field;
        }

        protected virtual string GetFieldName(string fieldName)
        {
            return new string(InsertSpacesBeforeCaps(fieldName).ToArray()).Trim();
        }

        private IEnumerable<char> InsertSpacesBeforeCaps(IEnumerable<char> input)
        {
            foreach (char c in input)
            {
                if (char.IsUpper(c))
                {
                    yield return ' ';
                }

                yield return c;
            }
        }
    }
}
