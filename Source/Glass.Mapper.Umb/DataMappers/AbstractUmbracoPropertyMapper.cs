using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Umb.Configuration;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb.DataMappers
{
    public abstract class AbstractUmbracoPropertyMapper : AbstractDataMapper
    {
        public IEnumerable<Type> TypesHandled { get; private set; }

        public AbstractUmbracoPropertyMapper(params Type[] typesHandled)
        {
            TypesHandled = typesHandled;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            var config = Configuration as UmbracoPropertyConfiguration;
            var context =  mappingContext  as UmbracoDataMappingContext;

            if (context.Content.Properties.Contains(config.PropertyAlias))
            {
                var property = context.Content.Properties[config.PropertyAlias];
                object value = Configuration.PropertyInfo.GetValue(mappingContext.Object, null);

                SetProperty(property, value, config, context);
            }
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var config = Configuration as UmbracoPropertyConfiguration;
            var context = mappingContext as UmbracoDataMappingContext;

            if (context.Content.Properties.Contains(config.PropertyAlias))
            {
                var property = context.Content.Properties[config.PropertyAlias];
                return GetProperty(property, config, context);
            }

            return null;
        }

        public virtual object GetProperty(Property property, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            var propertyValue = property.Value;

            return GetPropertyValue(propertyValue, config, context);
        }

        public virtual void SetProperty(Property property, object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            property.Value = SetPropertyValue(value, config, context);
        }

        public abstract object SetPropertyValue(object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context);
        public abstract object GetPropertyValue(object propertyValue, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context);

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is UmbracoPropertyConfiguration &&
                   TypesHandled.Any(x => x == configuration.PropertyInfo.PropertyType);
        }
    }
}
