using System;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{

    /// <summary>
    /// The delegate mapper
    /// </summary>
    public class SitecoreDelegateMapper : AbstractDataMapper
    {
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            var config = Configuration as SitecoreDelegateConfiguration;
            var context = mappingContext as SitecoreDataMappingContext;
            if (config == null)
            {
                throw new ArgumentException("A delegate property configuration was expected");
            }

            if (context == null)
            {
                throw new ArgumentException("A sitecore data mapping context was expected");
            }

            if (config.MapToCmsAction == null)
            {
                return;
            }

            config.MapToCmsAction(context);
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var config = Configuration as SitecoreDelegateConfiguration;
            var context = mappingContext as SitecoreDataMappingContext;
            if (config == null)
            {
                throw new ArgumentException("A delegate property configuration was expected");
            }

            if (context == null)
            {
                throw new ArgumentException("A sitecore data mapping context was expected");
            }

            return config.MapToPropertyAction == null
                ? null
                : config.MapToPropertyAction(context);
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreDelegateConfiguration;
        }
    }
}
