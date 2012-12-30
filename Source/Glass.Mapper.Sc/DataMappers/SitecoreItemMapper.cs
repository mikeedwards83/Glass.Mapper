using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreItemMapper : AbstractDataMapper
    {
        public SitecoreItemMapper()
        {
            ReadOnly = true;
        }


        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreNodeConfiguration;
            var scContext = mappingContext as SitecoreDataMappingContext;
            var item = scContext.Item;

            Item targetItem = null;

            if (scConfig.Id.IsNotNullOrEmpty())
            {
                var guid = Guid.Empty;

                if (Guid.TryParse(scConfig.Id, out guid) && guid != Guid.Empty)
                {
                    targetItem = item.Database.GetItem(new ID(guid), item.Language);
                }
            }
            else if (!scConfig.Path.IsNullOrEmpty())
            {
                targetItem = item.Database.GetItem(scConfig.Path, item.Language);
            }

            if (targetItem == null || targetItem.Versions.Count == 0)
            {
                return null;
            }
            else
            {
                return scContext.Service.CreateClass(scConfig.PropertyInfo.PropertyType, targetItem, scConfig.IsLazy,
                                                     scConfig.InferType);
            }

        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreNodeConfiguration && context.TypeConfigurations.ContainsKey(configuration.PropertyInfo.PropertyType);
        }
    }
}
