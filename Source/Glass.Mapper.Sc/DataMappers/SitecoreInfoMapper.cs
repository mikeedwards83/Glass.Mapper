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
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreInfoMapper
    /// </summary>
    public class SitecoreInfoMapper : AbstractDataMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInfoMapper"/> class.
        /// </summary>
        public SitecoreInfoMapper()
        {
            ReadOnly = true;
        }


        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>The value to write</returns>
        /// <exception cref="System.NotSupportedException">
        /// Can't set DisplayName. Value is not of type System.String
        /// or
        /// Can't set Name. Value is not of type System.String
        /// or
        /// You can not save SitecoreInfo {0}.Formatted(scConfig.Type)
        /// </exception>
        /// <exception cref="Glass.Mapper.MapperException">You can not set an empty or null Item name</exception>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            var context = mappingContext as SitecoreDataMappingContext;

            if (context == null)
            {
                throw new NullReferenceException("Mapping context has not been set.");
            }

            var item = context.Item;
            var value = context.PropertyValue;
            var scConfig = Configuration as SitecoreInfoConfiguration;

            if (scConfig == null)
            {
                throw  new NullReferenceException("Configuration is not set");
            }

            switch (scConfig.Type)
            {
                case SitecoreInfoType.DisplayName:
                    if (value is string || value == null)
                        item[Global.Fields.DisplayName] = (value ?? string.Empty).ToString();
                    else
                        throw new NotSupportedException("Can't set DisplayName. Value is not of type System.String");
                    break;
                case SitecoreInfoType.Name:
                    if (value is string || value == null)
                    {
                        //if the name is null or empty nothing should happen
                        if (value == null  || value.ToString().IsNullOrEmpty()) 
                            throw new MapperException("You can not set an empty or null Item name");

                        if (item.Name != value.ToString())
                        {
                            item.Name = value.ToString();
                        }

                    }
                    else
                        throw new NotSupportedException("Can't set Name. Value is not of type System.String");
                    break;
                default:
                    throw new NotSupportedException("You can not save SitecoreInfo {0}".Formatted(scConfig.Type));
            }
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="Glass.Mapper.MapperException">SitecoreInfoType {0} not supported.Formatted(scConfig.Type)</exception>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var context = mappingContext as SitecoreDataMappingContext;

            if (context == null)
            {
                throw new NullReferenceException("Mapping Context has not been set.");
            }

            var item = context.Item;
            var scConfig = Configuration as SitecoreInfoConfiguration;

            if (scConfig == null)
            {
                throw new NullReferenceException("Configuration has not been set.");
            }
            //TODO: move this to the config?
            switch (scConfig.Type)
            {



                case SitecoreInfoType.ContentPath:
                    return item.Paths.ContentPath;
                case SitecoreInfoType.DisplayName:
                    return item[Global.Fields.DisplayName];
                case SitecoreInfoType.FullPath:
                    return item.Paths.FullPath;
                case SitecoreInfoType.Name:
                    return item.Name;
                case SitecoreInfoType.Key:
                    return item.Key;
                case SitecoreInfoType.MediaUrl:
                    var mediaUrlOptions = Utilities.GetMediaUrlOptions(scConfig.MediaUrlOptions);
                    var media = new MediaItem(item);
                    return MediaManager.GetMediaUrl(media, mediaUrlOptions);
                case SitecoreInfoType.Path:
                    return item.Paths.Path;
                case SitecoreInfoType.TemplateId:
                    if (scConfig.PropertyInfo != null && scConfig.PropertyInfo.PropertyType == typeof (ID))
                        return item.TemplateID;
                    return item.TemplateID.Guid;
                case SitecoreInfoType.TemplateName:
                    return item.TemplateName;
                case SitecoreInfoType.Url:
                    var urlOptions = Utilities.CreateUrlOptions(scConfig.UrlOptions);
                    urlOptions.Language = null;
                    return LinkManager.GetItemUrl(item, urlOptions);
                case SitecoreInfoType.Version:
                    if (scConfig.PropertyInfo != null && scConfig.PropertyInfo.PropertyType == typeof (string))
                    {
                        return item.Version.Number.ToString();
                    }
                    return item.Version.Number;
                case SitecoreInfoType.Language:
                    if (scConfig.PropertyInfo != null && scConfig.PropertyInfo.PropertyType == typeof (string))
                    {
                        return item.Language.Name;
                    }
                    return item.Language;
                case SitecoreInfoType.BaseTemplateIds:
                    Template template = TemplateManager.GetTemplate(item.TemplateID, item.Database);
                    if (scConfig.PropertyInfo != null &&
                        scConfig.PropertyInfo.PropertyType == typeof (IEnumerable<ID>))
                        return template.GetBaseTemplates().Select(x => x.ID);
                    return template.GetBaseTemplates().Select(x => x.ID.Guid);
                case SitecoreInfoType.ItemUri:
                    return new ItemUri(item.ID, item.Language, item.Version, item.Database);
#if SC81
                case SitecoreInfoType.OriginalLanguage:
                    return item.OriginalLanguage;
                case SitecoreInfoType.OriginatorId:
                    return item.OriginatorId;
#endif
                default:
                    throw new MapperException("SitecoreInfoType {0} not supported".Formatted(scConfig.Type));
            }
        }

        /// <summary>
        /// Sets up the data mapper for a particular property
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Setup(DataMapperResolverArgs args)
        {
            var scConfig = args.PropertyConfiguration as SitecoreInfoConfiguration;

            if (scConfig == null)
            {
                throw new NullReferenceException("Configuration has not been set.");
            }

            ReadOnly = scConfig.Type != SitecoreInfoType.DisplayName && scConfig.Type != SitecoreInfoType.Name;
            base.Setup(args);
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreInfoConfiguration;
        }
    }
}




