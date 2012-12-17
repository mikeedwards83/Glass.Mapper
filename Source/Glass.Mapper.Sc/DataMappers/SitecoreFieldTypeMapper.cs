using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldTypeMapper : AbstractSitecoreFieldMapper
    {

        public override object GetFieldValue(Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {

            var fieldValue = field.Value;
            var item = context.Item;

            if (fieldValue.IsNullOrEmpty()) return null;

            Guid id = Guid.Empty;
            Item target;

            if (Guid.TryParse(fieldValue, out id)) {

                target = item.Database.GetItem(new ID(id), item.Language);
            }
            else
            {
                target = item.Database.GetItem(fieldValue, item.Language);
            }

            if (target == null) return null;
            return context.Service.CreateClass(config.PropertyInfo.PropertyType, target, IsLazy, InferType);
        }

        public override void SetFieldValue(Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {

            if (value == null)
                field.Value = string.Empty;
            else
            {
                var typeConfig = context.Service.GlassContext[value.GetType()] as SitecoreTypeConfiguration;

                var item = typeConfig.ResolveItem(value, context.Service.Database);
                if(item == null)
                    throw new NullReferenceException("Could not find item to save value {0}".Formatted(Configuration));

                field.Value = item.ID.ToString();
            }
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration,  Context context)
        {
            return context[configuration.PropertyInfo.PropertyType] != null;
        }

        public override void Setup(Mapper.Configuration.AbstractPropertyConfiguration configuration)
        {
            var scConfig = configuration as SitecoreFieldConfiguration;

            IsLazy = (scConfig.Setting & SitecoreFieldSettings.DontLoadLazily) != SitecoreFieldSettings.DontLoadLazily;
            InferType = (scConfig.Setting & SitecoreFieldSettings.InferType) == SitecoreFieldSettings.InferType;
            base.Setup(configuration);
        }

        protected bool InferType { get; set; }

        protected bool IsLazy { get; set; }
    }
}
