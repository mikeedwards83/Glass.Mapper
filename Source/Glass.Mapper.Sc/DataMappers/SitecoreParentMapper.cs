using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreParentMapper :AbstractDataMapper
    {

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
            var scConfig = Configuration as SitecoreParentConfiguration;

            return scContext.Service.CreateClass(
                scConfig.PropertyInfo.PropertyType,
                scContext.Item.Parent,
                scConfig.IsLazy,
                scConfig.InferType);
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration)
        {
            return configuration is SitecoreParentConfiguration;
        }
    }
}
