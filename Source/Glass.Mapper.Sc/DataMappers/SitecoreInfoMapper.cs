using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public override void Setup(Mapper.Configuration.AbstractPropertyConfiguration configuration)
        {
            var scConfig = configuration as SitecoreInfoConfiguration;
            this.ReadOnly = scConfig.Type != SitecoreInfoType.DisplayName && scConfig.Type != SitecoreInfoType.Name;
            base.Setup(configuration);
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreInfoConfiguration;
        }
    }
}
