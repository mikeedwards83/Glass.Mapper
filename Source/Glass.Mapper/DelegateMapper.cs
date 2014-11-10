using System;
using Glass.Mapper.Configuration;

namespace Glass.Mapper
{
    /// <summary>
    /// The delegate mapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateMapper<T> : AbstractDataMapper where T : AbstractDataMappingContext
    {
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            var config = Configuration as DelegatePropertyConfiguration<T>;
            var context = mappingContext as T;
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
            var config = Configuration as DelegatePropertyConfiguration<T>;
            var context = mappingContext as T;
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
            return configuration is DelegatePropertyConfiguration<T>;
        }
    }
}