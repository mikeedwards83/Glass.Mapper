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
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Links;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreInfoMapper : AbstractDataMapper
    {
        public SitecoreInfoMapper()
        {
            ReadOnly = true;
        }


        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            var context = mappingContext as SitecoreDataMappingContext;
            var item = context.Item;
            var value = context.PropertyValue;
            var scConfig = Configuration as SitecoreInfoConfiguration;

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
                        if ((value ?? string.Empty).ToString().IsNullOrEmpty()) 
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

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var context = mappingContext as SitecoreDataMappingContext;
            var item = context.Item;
            var scConfig = Configuration as SitecoreInfoConfiguration;

            //TODO: move this to the config?
            var urlOptions = Utilities.CreateUrlOptions(scConfig.UrlOptions);

            switch (scConfig.Type)
            {
               
                  case SitecoreInfoType.ContentPath:
                    return item.Paths.ContentPath;
                case SitecoreInfoType.DisplayName:
                    return item.DisplayName;
                case SitecoreInfoType.FullPath:
                    return item.Paths.FullPath;
                case SitecoreInfoType.Name:
                    return item.Name;
                case SitecoreInfoType.Key:
                    return item.Key;
                case SitecoreInfoType.MediaUrl:
                    var media = new global::Sitecore.Data.Items.MediaItem(item);
                    return global::Sitecore.Resources.Media.MediaManager.GetMediaUrl(media);
                    break;
                case SitecoreInfoType.Path:
                    return item.Paths.Path;
                case SitecoreInfoType.TemplateId:
                    if (scConfig.PropertyInfo != null && scConfig.PropertyInfo.PropertyType == typeof(Sitecore.Data.ID))
                        return item.TemplateID;
                    else
                        return item.TemplateID.Guid;
                case SitecoreInfoType.TemplateName:
                    return item.TemplateName;
                case SitecoreInfoType.Url:
                    return LinkManager.GetItemUrl(item, urlOptions);
                case SitecoreInfoType.Version:
                    return item.Version.Number;
                case SitecoreInfoType.Language:
                    return item.Language;  
                default:
                    throw new MapperException("SitecoreInfoType {0} not supported".Formatted(scConfig.Type));
            }
        }

        public override void Setup(DataMapperResolverArgs args)
        {
            var scConfig = args.PropertyConfiguration as SitecoreInfoConfiguration;
            this.ReadOnly = scConfig.Type != SitecoreInfoType.DisplayName && scConfig.Type != SitecoreInfoType.Name;
            base.Setup(args);
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreInfoConfiguration;
        }
    }
}



