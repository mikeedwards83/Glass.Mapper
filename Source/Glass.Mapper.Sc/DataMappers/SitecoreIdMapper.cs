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
        private SitecoreIdConfiguration _config;

        public SitecoreIdMapper()
        {
            this.ReadOnly = true;
        }

        public override void Setup(Mapper.Configuration.AbstractPropertyConfiguration configuration)
        {
            _config = configuration as SitecoreIdConfiguration;
            base.Setup(configuration);
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            SitecoreDataMappingContext context = mappingContext as SitecoreDataMappingContext;
            var item = context.Item;

            if (_config.PropertyInfo.PropertyType == typeof(Guid))
                return item.ID.Guid;
            else if (_config.PropertyInfo.PropertyType == typeof (ID))
                return item.ID;
            else
            {
                throw new NotSupportedException("The type {0} on {0}.{1} is not supported by SitecoreIdMapper".Formatted
                                                    (_config.PropertyInfo.ReflectedType.FullName,
                                                        _config.PropertyInfo.Name));
            }

        }

        

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration)
        {
            return configuration is SitecoreIdConfiguration;
        }
    }
}
