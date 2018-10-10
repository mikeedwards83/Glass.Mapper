using System.Collections.Specialized;
using System.Web;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldNameValueCollectionMapper
    /// </summary>
    public class SitecoreFieldNameValueCollectionMapper : AbstractSitecoreFieldMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldNameValueCollectionMapper"/> class.
        /// </summary>
        public SitecoreFieldNameValueCollectionMapper() : base(typeof (NameValueCollection))
        {
        }


        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            NameValueCollection collection = value as NameValueCollection;

            if (collection != null)
            {
                return Utilities.ConstructQueryString(collection);
            }
            
            
            return null;
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
                   return HttpUtility.ParseQueryString(fieldValue);

        }
    }
}




