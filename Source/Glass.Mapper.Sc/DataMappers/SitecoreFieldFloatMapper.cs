using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldFloatMapper
    /// </summary>
    public class SitecoreFieldFloatMapper : AbstractSitecoreFieldMapper
    {
        protected override object DefaultValue { get { return (float)0; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldFloatMapper"/> class.
        /// </summary>
        public SitecoreFieldFloatMapper()
            : base(typeof(float))
        {

        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="Glass.Mapper.MapperException">Could not convert value to double</exception>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config,
                                             SitecoreDataMappingContext context)
        {
            if (fieldValue.IsNullOrEmpty()) return 0f;
            float dValue = 0f;
            if (float.TryParse(fieldValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dValue)) return dValue;
            else throw new MapperException("Could not convert value to float");
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotSupportedException">The value is not of type System.Float</exception>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            float? floatValue = value as float?;
            if (floatValue.HasValue)
            {
                return floatValue.Value.ToString(CultureInfo.InvariantCulture);
            }

            throw new NotSupportedException("The value is not of type System.Float");
        }
    }
}




