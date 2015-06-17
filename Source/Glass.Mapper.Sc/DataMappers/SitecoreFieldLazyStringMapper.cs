using System;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldLazyStringMapper : SitecoreFieldLazyMapper
    {
        public SitecoreFieldLazyStringMapper() : base(new[] {typeof(Lazy<string>)})
        {
            
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return GetLazy<string>(fieldValue, config, context);
        }
    }
}
