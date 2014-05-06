using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreItemMapper : AbstractDataMapper
    {
        public SitecoreItemMapper()
        {
            ReadOnly = true;
        }
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scContext = mappingContext as SitecoreDataMappingContext;
            if (scContext != null)
            {
                return scContext.Item;
            }
            return null;
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreItemConfiguration && configuration.PropertyInfo.PropertyType == typeof (Item);
        }
    }
}
