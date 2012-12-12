using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    public class UmbracoIdAttribute : IdAttribute
    {
        public UmbracoIdAttribute() : base(new[] { typeof(int) }) { }

        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new UmbracoIdConfiguration();
            base.Configure(propertyInfo, config);
            return config;
        }

        public void Configure(PropertyInfo propertyInfo, UmbracoIdConfiguration config)
        {
            base.Configure(propertyInfo, config);
        }
    }
}
