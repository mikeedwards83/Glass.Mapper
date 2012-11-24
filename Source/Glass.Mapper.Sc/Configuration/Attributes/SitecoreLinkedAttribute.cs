using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreLinkedAttribute : LinkedAttribute
    {
        /// <summary>
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        public SitecoreLinkedOptions Option { get; set; }

        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new SitecoreLinkedConfiguration();
            this.Configure(propertyInfo, config);
            return config;
        }

        public void Configure(PropertyInfo propertyInfo, SitecoreLinkedConfiguration config)
        {
            config.Option = this.Option;

            base.Configure(propertyInfo, config);
        }
    }
}
