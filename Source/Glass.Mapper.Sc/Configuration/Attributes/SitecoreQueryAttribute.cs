using System.Reflection;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreQueryAttribute
    /// </summary>
    public class SitecoreQueryAttribute : QueryAttribute
    {
        /// <summary>
        /// A template ID to enforce when mapping the property.EnforceTemplate must also be set.
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// The type of template check to perform when mapping the property. The TemplateId must also be set.
        /// </summary>
        public SitecoreEnforceTemplate EnforceTemplate { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="QueryAttribute" /> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public SitecoreQueryAttribute(string query):base(query)
        {
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration.</returns>
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new SitecoreQueryConfiguration();
            Configure(propertyInfo, config);
            return config;
        }
        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, SitecoreQueryConfiguration config)
        {
            base.Configure(propertyInfo, (QueryConfiguration) config);
        }
    }
}




