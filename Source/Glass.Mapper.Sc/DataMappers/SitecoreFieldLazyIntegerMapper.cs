using System;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldLazyIntegerMapper : SitecoreFieldLazyMapper
    {
        public SitecoreFieldLazyIntegerMapper()
            : base(new[] { typeof(Lazy<int>) })
        {
            
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return GetLazy<int>(fieldValue, config, context);
        }
    }
}
