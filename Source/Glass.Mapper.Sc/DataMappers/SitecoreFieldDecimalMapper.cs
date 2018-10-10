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
    /// Class SitecoreFieldDecimalMapper
    /// </summary>
    public class SitecoreFieldDecimalMapper : AbstractSitecoreFieldMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldDecimalMapper"/> class.
        /// </summary>
        public SitecoreFieldDecimalMapper() : base(typeof (decimal))
        {

        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="Glass.Mapper.MapperException">Could not convert value to decimal</exception>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config,
                                             SitecoreDataMappingContext context)
        {
            if (fieldValue.IsNullOrEmpty()) return 0M;

            decimal dValue = 0;
            if (decimal.TryParse(fieldValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dValue))
                return dValue;
            else throw new MapperException("Could not convert value to decimal");
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotSupportedException">The value is not of type System.Decimal</exception>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            decimal? decimalValue = value as decimal?;
            if (decimalValue.HasValue)
            {
                return decimalValue.Value.ToString(CultureInfo.InvariantCulture);
            }

            throw new NotSupportedException("The value is not of type System.Decimal");
        }
    }
}




