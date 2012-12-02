using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;

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
            return context.Item.Name;
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
