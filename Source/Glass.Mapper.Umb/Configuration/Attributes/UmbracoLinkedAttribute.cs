using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    /// <summary>
    /// UmbracoLinkedAttribute
    /// </summary>
    public class UmbracoLinkedAttribute : LinkedAttribute
    {
        /// <summary>
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        /// <value>
        /// The option.
        /// </value>
        public UmbracoLinkedOptions Option { get; set; }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new UmbracoLinkedConfiguration();
            this.Configure(propertyInfo, config);
            return config;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, UmbracoLinkedConfiguration config)
        {
            config.Option = this.Option;

            base.Configure(propertyInfo, config);
        }
    }
}
