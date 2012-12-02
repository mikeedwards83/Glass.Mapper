using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreInfoAttribute : InfoAttribute
    {
        public SitecoreInfoAttribute()
        {
            
        }
        public SitecoreInfoAttribute(SitecoreInfoType infoType)
        {
            Type = infoType;
        }

        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        public SitecoreInfoType Type { get; set; }

        /// <summary>
        /// UrlOptions, use in conjunction with SitecoreInfoType.Url
        /// </summary>
        public SitecoreInfoUrlOptions UrlOptions { get; set; }

        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new SitecoreInfoConfiguration();
            Configure(propertyInfo, config);
            return config;
        }

        public void Configure(PropertyInfo propertyInfo, SitecoreInfoConfiguration config)
        {
            config.Type = this.Type;
            config.UrlOptions = this.UrlOptions;

            base.Configure(propertyInfo, config);
        }
    }
}
