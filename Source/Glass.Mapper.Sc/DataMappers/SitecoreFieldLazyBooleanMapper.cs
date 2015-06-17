using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldLazyBooleanMapper : SitecoreFieldLazyMapper
    {
        public SitecoreFieldLazyBooleanMapper()
            : base(new[] { typeof(Lazy<bool>) })
        {
            
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return GetLazy<bool>(fieldValue, config, context);
        }
    }
}
