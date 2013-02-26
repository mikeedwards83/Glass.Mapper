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
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Umb.Configuration;

namespace Glass.Mapper.Umb.DataMappers
{
    public class UmbracoInfoMapper : AbstractDataMapper
    {
        public UmbracoInfoMapper()
        {
            ReadOnly = true;
        }


        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            //var context = mappingContext as UmbracoDataMappingContext;
            //var item = context.Item;
            //var value = context.PropertyValue;
            //var scConfig = Configuration as UmbracoInfoConfiguration;

            //switch (scConfig.Type)
            //{
            //    case UmbracoInfoType.DisplayName:
            //        if (value is string || value == null)
            //            item[Global.Fields.DisplayName] = (value ?? string.Empty).ToString();
            //        else
            //            throw new NotSupportedException("Can't set DisplayName. Value is not of type System.String");
            //        break;
            //    case UmbracoInfoType.Name:
            //        if (value is string || value == null)
            //        {
            //            //if the name is null or empty nothing should happen
            //            if ((value ?? string.Empty).ToString().IsNullOrEmpty()) 
            //                throw new MapperException("You can not set an empty or null Item name");

            //            if (item.Name != value.ToString())
            //            {
            //                item.Name = value.ToString();
            //            }

            //        }
            //        else
            //            throw new NotSupportedException("Can't set Name. Value is not of type System.String");
            //        break;
            //    default:
            //        throw new NotSupportedException("You can not save UmbracoInfo {0}".Formatted(scConfig.Type));
            //}
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            //var context = mappingContext as UmbracoDataMappingContext;
            //var item = context.Item;
            //var scConfig = Configuration as UmbracoInfoConfiguration;

            ////TODO: move this to the config?
            //var urlOptions = Mapper.Utilities.CreateUrlOptions(scConfig.UrlOptions);

            //switch (scConfig.Type)
            //{
               
            //      case UmbracoInfoType.ContentPath:
            //        return item.Paths.ContentPath;
            //    case UmbracoInfoType.DisplayName:
            //        return item.DisplayName;
            //    case UmbracoInfoType.FullPath:
            //        return item.Paths.FullPath;
            //    case UmbracoInfoType.Name:
            //        return item.Name;
            //    case UmbracoInfoType.Key:
            //        return item.Key;
            //    case UmbracoInfoType.MediaUrl:
            //        var media = new global::Umbraco.Data.Items.MediaItem(item);
            //        return global::Umbraco.Resources.Media.MediaManager.GetMediaUrl(media);
            //        break;
            //    case UmbracoInfoType.Path:
            //        return item.Paths.Path;
            //    case UmbracoInfoType.TemplateId:
            //        if (scConfig.PropertyInfo != null && scConfig.PropertyInfo.PropertyType == typeof(Umbraco.Data.ID))
            //            return item.TemplateID;
            //        else
            //            return item.TemplateID.Guid;
            //    case UmbracoInfoType.TemplateName:
            //        return item.TemplateName;
            //    case UmbracoInfoType.Url:
            //        return LinkManager.GetItemUrl(item, urlOptions);
            //    case UmbracoInfoType.Version:
            //        return item.Version.Number;
            //    case UmbracoInfoType.Language:
            //        return item.Language;  
            //    default:
            //        throw new MapperException("UmbracoInfoType {0} not supported".Formatted(scConfig.Type));
            //}
            return null;
        }

        public override void Setup(DataMapperResolverArgs args)
        {
            var scConfig = args.PropertyConfiguration as UmbracoInfoConfiguration;
            this.ReadOnly = scConfig.Type != UmbracoInfoType.DisplayName && scConfig.Type != UmbracoInfoType.Name;
            base.Setup(args);
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is UmbracoInfoConfiguration;
        }
    }
}



