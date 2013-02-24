using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    public class UmbracoInfoAttribute : InfoAttribute
    {
        public UmbracoInfoAttribute()
        {
            
        }
        public UmbracoInfoAttribute(UmbracoInfoType infoType)
        {
            Type = infoType;
        }

        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        public UmbracoInfoType Type { get; set; }

        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new UmbracoIdConfiguration();
            Configure(propertyInfo, config);
            return config;
        }

        public void Configure(PropertyInfo propertyInfo, UmbracoInfoConfiguration config)
        {
            config.Type = this.Type;

            base.Configure(propertyInfo, config);
        }
    }
}
