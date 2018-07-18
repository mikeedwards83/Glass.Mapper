using System;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{

    

    /// <summary>
    /// Class SitecoreFieldGuidMapper
    /// </summary>
    public class SitecoreFieldGuidMapper : AbstractSitecoreFieldMapper
    {
        protected override object DefaultValue
        {
            get { return Guid.Empty; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldGuidMapper"/> class.
        /// </summary>
        public SitecoreFieldGuidMapper()
            : base(typeof(Guid))
        {

        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config,
                                             SitecoreDataMappingContext context)
        {
            if (fieldValue.IsNullOrEmpty()) return Guid.Empty;


            return Guid.Parse(fieldValue);
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="Glass.Mapper.MapperException">The value is not of type System.Guid</exception>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value is Guid)
            {
                return ((Guid)value).ToString("B").ToUpper();
            }
            else throw new MapperException("The value is not of type System.Guid");
        }
    }
}




