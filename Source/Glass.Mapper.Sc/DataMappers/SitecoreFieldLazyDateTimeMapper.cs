using System;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldLazyDateTimeMapper : SitecoreFieldLazyMapper
    {
        public SitecoreFieldLazyDateTimeMapper(): base(new[] { typeof(Lazy<DateTime>) })
        {
            
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return GetLazy<DateTime>(fieldValue, config, context);
        }
    }
}
