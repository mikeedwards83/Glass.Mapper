using System;
using System.Globalization;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldIntegerMapper
    /// </summary>
    public class SitecoreFieldIntegerMapper : AbstractSitecoreFieldMapper
    {

        protected override object DefaultValue { get { return 0; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldIntegerMapper"/> class.
        /// </summary>
        public SitecoreFieldIntegerMapper()
            : base(typeof(int))
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
            if (fieldValue.IsNullOrEmpty()) return 0;
            int dValue = 0;
            if (int.TryParse(fieldValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dValue)) return dValue;
            else throw new MapperException("Could not convert value to Integer");
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
            if (value is int)
            {
                return value.ToString();
            }
            else
                throw new NotSupportedException("The value is not of type System.Integer");
        }
    }
}




