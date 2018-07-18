using System.Reflection;
using Glass.Mapper.Configuration.Attributes;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreLinkedAttribute
    /// </summary>
    public class SitecoreLinkedAttribute : LinkedAttribute
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
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        /// <value>The option.</value>
        public SitecoreLinkedOptions Option { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreLinkedAttribute"/> class.
        /// </summary>
        public SitecoreLinkedAttribute()
        {
            Option = SitecoreLinkedOptions.All;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration.</returns>
        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new SitecoreLinkedConfiguration();
            this.Configure(propertyInfo, config);
            return config;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, SitecoreLinkedConfiguration config)
        {
            config.Option = this.Option;
            if (TemplateId.HasValue())
            {
                config.TemplateId = new ID(TemplateId);
            }
            config.EnforceTemplate = EnforceTemplate;

            base.Configure(propertyInfo, config);
        }
    }
}




