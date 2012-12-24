using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public abstract class AbstractSitecoreFieldMapper : AbstractDataMapper
    {
        public IEnumerable<Type> TypesHandled { get; private set; }

        public AbstractSitecoreFieldMapper(params Type [] typesHandled)
        {
            TypesHandled = typesHandled;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreFieldConfiguration;
            var scContext = mappingContext as SitecoreDataMappingContext;

            var field = Utilities.GetField(scContext.Item, scConfig.FieldId, scConfig.FieldName);
            object value = Configuration.PropertyInfo.GetValue(mappingContext.Object, null);

            SetField(field, value, scConfig, scContext);
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreFieldConfiguration;
            var scContext = mappingContext as SitecoreDataMappingContext;

            var field = Utilities.GetField(scContext.Item, scConfig.FieldId, scConfig.FieldName);
            return GetField(field, scConfig, scContext);
        }

        public virtual object GetField(Field field, SitecoreFieldConfiguration config,
                                       SitecoreDataMappingContext context)
        {
            var fieldValue = field.Value;

            return GetFieldValue(fieldValue, config, context);
        }
        public virtual void SetField(Field field, object value, SitecoreFieldConfiguration config,
                                      SitecoreDataMappingContext context)
        {
            field.Value = SetFieldValue(value, config, context);
        }

        public abstract string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context);
        public abstract object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context);

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreFieldConfiguration &&
                   TypesHandled.Any(x => x == configuration.PropertyInfo.PropertyType);
        }
    }
}
