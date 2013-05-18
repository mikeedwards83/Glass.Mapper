using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// This mapped does nothing, used to ignore a property
    /// </summary>
     public class SitecoreIgnoreMapper : AbstractDataMapper
    {
         public override void MapCmsToProperty(AbstractDataMappingContext mappingContext)
         {
             //does nothing!!!
         }
         public override void MapPropertyToCms(AbstractDataMappingContext mappingContext)
         {
             //does nothing!!!
         }
         
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return Configuration is IgnoreAttribute;
        }
    }
}
