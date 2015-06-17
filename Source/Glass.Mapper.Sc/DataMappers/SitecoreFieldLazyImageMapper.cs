using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldLazyImageMapper : SitecoreFieldLazyMapper
    {
        public SitecoreFieldLazyImageMapper()
            : base(new[] { typeof(Lazy<Image>) })
        {
            
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return GetLazy<Image>(fieldValue, config, context);
        }
    }
}
