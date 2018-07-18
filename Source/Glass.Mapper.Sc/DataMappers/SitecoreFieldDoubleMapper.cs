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
    /// Class SitecoreFieldDoubleMapper
    /// </summary>
    public class SitecoreFieldDoubleMapper : AbstractSitecoreFieldMapper
    {

        protected override object DefaultValue { get { return 0d; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldDoubleMapper"/> class.
        /// </summary>
        public SitecoreFieldDoubleMapper() : base(typeof (double))
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
            if (fieldValue.IsNullOrEmpty()) return 0d;
            double dValue = 0;
            if (double.TryParse(fieldValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dValue)) return dValue;
            else throw new MapperException("Could not convert value to double");
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotSupportedException">The value is not of type System.Double</exception>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            double? doubleValue = value as double?;
            if (doubleValue.HasValue)
            {
                return doubleValue.Value.ToString(CultureInfo.InvariantCulture);
            }

            throw new NotSupportedException("The value is not of type System.Double");
        }
    }
}




