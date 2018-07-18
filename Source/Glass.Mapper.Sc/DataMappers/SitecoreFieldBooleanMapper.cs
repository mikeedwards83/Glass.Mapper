using System;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldBooleanMapper
    /// </summary>
    public class SitecoreFieldBooleanMapper : AbstractSitecoreFieldMapper
    {

        protected override object DefaultValue
        {
            get { return false; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldBooleanMapper"/> class.
        /// </summary>
        public SitecoreFieldBooleanMapper() : base(typeof (bool))
        {

        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return fieldValue == "1";
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
            if (value is bool)
            {
                bool actual = (bool)value;
                return actual ? "1" : "0";
            }
            else
                throw new NotSupportedException("The value is not of type System.Boolean");
        }
    }
}




