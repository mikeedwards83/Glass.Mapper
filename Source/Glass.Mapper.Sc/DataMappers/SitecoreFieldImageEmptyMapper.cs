using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldImageEmptyMapper : SitecoreFieldImageMapper
    {
        public override object GetField(Field field, SitecoreFieldConfiguration config,
            SitecoreDataMappingContext context)
        {
            var result = base.GetField(field, config, context);
            return result ?? new Image();
        }
    }
}