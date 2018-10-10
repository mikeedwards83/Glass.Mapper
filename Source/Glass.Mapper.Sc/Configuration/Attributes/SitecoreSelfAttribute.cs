using System.Reflection;
using Glass.Mapper.Configuration.Attributes;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Maps the Sitecore Item to typed property.
    /// </summary>
    public class SitecoreSelfAttribute : ItemAttribute
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
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration.</returns>
        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new SitecoreSelfConfiguration();
            Configure(propertyInfo, config);

            if (TemplateId.HasValue())
            {
                config.TemplateId = new ID(TemplateId);
            }

            config.EnforceTemplate = EnforceTemplate;

            return config;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, SitecoreSelfConfiguration config)
        {
            base.Configure(propertyInfo, config);
        }
    }
}