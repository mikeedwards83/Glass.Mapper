using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreSelfMapper : AbstractDataMapper
    {
        public SitecoreSelfMapper()
        {
            ReadOnly = true;
        }
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {

        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scContext = mappingContext as SitecoreDataMappingContext;
            var scConfig = Configuration as SitecoreSelfConfiguration;

            if (scContext != null && scConfig != null)
            {
                return scContext.Service.CreateType(scConfig.PropertyInfo.PropertyType, scContext.Item, scConfig.IsLazy, scConfig.InferType, null);
            }
            return null;
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreSelfConfiguration;
        }
    }
}