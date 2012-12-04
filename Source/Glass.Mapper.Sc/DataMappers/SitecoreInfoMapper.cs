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

        private SitecoreInfoConfiguration _config;

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var context = mappingContext as SitecoreDataMappingContext;
            var item = context.Item;

            //TODO: move this to the config?
            var urlOptions = Utilities.CreateUrlOptions(_config.UrlOptions);

            switch (_config.Type)
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
                    if (_config.PropertyInfo != null && _config.PropertyInfo.PropertyType == typeof (Sitecore.Data.ID))
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
                    throw new MapperException("SitecoreInfoType {0} not supported".Formatted(_config.Type));
            }
        }

        public override void Setup(Mapper.Configuration.AbstractPropertyConfiguration configuration)
        {
            _config = configuration as SitecoreInfoConfiguration;
            base.Setup(configuration);
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration)
        {
            return configuration is SitecoreInfoConfiguration;
        }
    }
}
