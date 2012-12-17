using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreIdMapper : AbstractDataMapper
    {

        public SitecoreIdMapper()
        {
            this.ReadOnly = true;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            SitecoreDataMappingContext context = mappingContext as SitecoreDataMappingContext;
            var item = context.Item;

            var scConfig = Configuration as SitecoreIdConfiguration;

            if (scConfig.PropertyInfo.PropertyType == typeof(Guid))
                return item.ID.Guid;
            else if (scConfig.PropertyInfo.PropertyType == typeof(ID))
                return item.ID;
            else
            {
                throw new NotSupportedException("The type {0} on {0}.{1} is not supported by SitecoreIdMapper".Formatted
                                                    (scConfig.PropertyInfo.ReflectedType.FullName,
                                                        scConfig.PropertyInfo.Name));
            }

        }



        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreIdConfiguration;
        }
    }
}
