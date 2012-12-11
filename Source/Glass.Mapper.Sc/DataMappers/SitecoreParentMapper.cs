using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreParentMapper :AbstractDataMapper
    {
        public SitecoreParentConfiguration Config { get; private set; }

        public SitecoreParentMapper()
        {
            ReadOnly = true;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotSupportedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scContext = mappingContext as SitecoreDataMappingContext;
            return scContext.Service.CreateClass(
                Config.PropertyInfo.PropertyType,
                scContext.Item.Parent,
                Config.IsLazy,
                Config.InferType);
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration)
        {
            return configuration is SitecoreParentConfiguration;
        }
        public override void Setup(Mapper.Configuration.AbstractPropertyConfiguration configuration)
        {
            Config = configuration as SitecoreParentConfiguration;
            base.Setup(configuration);
        }
    }
}
