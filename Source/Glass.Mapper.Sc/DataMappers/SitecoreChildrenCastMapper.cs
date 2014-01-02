using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Custom;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreChildrenCastMapper : AbstractDataMapper
    {
        public SitecoreChildrenCastMapper()
        {
            ReadOnly = true;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scContext = mappingContext as SitecoreDataMappingContext;
              var scConfig = Configuration as SitecoreChildrenConfiguration;
            if (scContext != null && scConfig != null)
            {
                return new ChildrenCast(scContext.Service, scContext.Item, scConfig.IsLazy, scConfig.InferType);
            }
            return null;
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreChildrenConfiguration &&
                   configuration.PropertyInfo.PropertyType == typeof (ChildrenCast);
        }
    }
}
