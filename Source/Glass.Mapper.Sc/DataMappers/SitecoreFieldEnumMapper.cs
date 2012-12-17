using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldEnumMapper : AbstractSitecoreFieldMapper
    {
        public override object GetFieldValue(Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Type enumType = config.PropertyInfo.PropertyType;

            int intValue;
            if (int.TryParse(field.Value, out intValue))
            {
                return Enum.ToObject(enumType, intValue);
            }
            else
            {
                if (Enum.IsDefined(enumType, field.Value))
                    return Enum.Parse(enumType, field.Value, true);
                else if (!field.Value.IsNullOrEmpty())
                    throw new MapperException("Can not convert value {0} to enum type {1}".Formatted(field.Value, enumType.FullName));
                else
                    throw new MapperException("Can not convert value {0} to enum type {1}".Formatted(field.Value, enumType.FullName));

            }
        }

        public override void SetFieldValue(Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            field.Value= Enum.GetName(config.PropertyInfo.PropertyType, value);
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration.PropertyInfo.PropertyType.IsEnum;
        }
    }
}
