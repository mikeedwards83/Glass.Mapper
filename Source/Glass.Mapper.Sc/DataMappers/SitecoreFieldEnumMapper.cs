
using System;
using System.Linq;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldEnumMapper
    /// </summary>
    public class SitecoreFieldEnumMapper : AbstractSitecoreFieldMapper
    {
        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="Glass.Mapper.MapperException">
        /// Can not convert value {0} to enum type {1}.Formatted(fieldValue, enumType.FullName)
        /// or
        /// Can not convert value {0} to enum type {1}.Formatted(fieldValue, enumType.FullName)
        /// </exception>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Type enumType = config.PropertyInfo.PropertyType;

            int intValue;
            if (int.TryParse(fieldValue, out intValue))
            {
                return Enum.ToObject(enumType, intValue);
            }
            else
            {
                if (Enum.GetNames(enumType).Any(x => x.Equals(fieldValue, StringComparison.InvariantCultureIgnoreCase)))
                    return Enum.Parse(enumType, fieldValue, true);
                else if (!fieldValue.IsNullOrEmpty())
                    throw new MapperException("Can not convert value {0} to enum type {1}".Formatted(fieldValue,
                                                                                                     enumType.FullName));
                else
                   return Enum.GetValues(enumType).GetValue(0);

            }
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        public override string SetFieldValue( object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return Enum.GetName(config.PropertyInfo.PropertyType, value);
        }

        /// <summary>
        /// Determines whether this instance can handle the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration.PropertyInfo.PropertyType.IsEnum;
        }
    }
}




