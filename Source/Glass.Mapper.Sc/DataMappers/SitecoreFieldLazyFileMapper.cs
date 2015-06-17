using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldLazyFileMapper : SitecoreFieldLazyMapper
    {
        public SitecoreFieldLazyFileMapper()
            : base(new[] { typeof(Lazy<File>) })
        {
            
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return GetLazy<File>(fieldValue, config, context);
        }
    }
}
