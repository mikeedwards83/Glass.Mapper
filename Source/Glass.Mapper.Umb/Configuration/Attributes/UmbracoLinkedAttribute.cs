using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    public class UmbracoLinkedAttribute : LinkedAttribute
    {
        /// <summary>
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        public UmbracoLinkedOptions Option { get; set; }

        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new UmbracoLinkedConfiguration();
            this.Configure(propertyInfo, config);
            return config;
        }

        public void Configure(PropertyInfo propertyInfo, UmbracoLinkedConfiguration config)
        {
            config.Option = this.Option;

            base.Configure(propertyInfo, config);
        }
    }
}
