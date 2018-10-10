
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.DataMappers
{

    

    /// <summary>
    /// Class SitecoreFieldGuidMapper
    /// </summary>
    public class SitecoreFieldIdMapper : AbstractSitecoreFieldMapper
    {
        protected override object DefaultValue
        {
            get { return ID.Null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldGuidMapper"/> class.
        /// </summary>
        public SitecoreFieldIdMapper()
            : base(typeof(ID))
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
            if (fieldValue.IsNullOrEmpty()) return ID.Null;


            return ID.Parse(fieldValue);
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
            if (value is ID)
            {
                return ((ID)value).ToString();
            }
            else throw new MapperException("The value is not of type Sitecore.Data.ID");
        }
    }
}




