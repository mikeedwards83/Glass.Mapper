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
            var umbConfig = Configuration as UmbracoPropertyConfiguration;
            var umbContext = mappingContext as UmbracoDataMappingContext;

            var property = umbContext.Content.Properties[umbConfig.PropertyAlias];
            object value = Configuration.PropertyInfo.GetValue(mappingContext.Object, null);

            SetPropertyValue(property, value, umbConfig, umbContext);
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var umbConfig = Configuration as UmbracoPropertyConfiguration;
            var umbContext = mappingContext as UmbracoDataMappingContext;

            var property = umbContext.Content.Properties[umbConfig.PropertyAlias];

            return GetPropertyValue(property, umbConfig, umbContext);
        }

        public abstract object GetPropertyValue(Property property, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context);
        public abstract void SetPropertyValue(Property property, object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context);

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is UmbracoPropertyConfiguration &&
                   TypesHandled.Any(x => x == configuration.PropertyInfo.PropertyType);
        }
    }
}
