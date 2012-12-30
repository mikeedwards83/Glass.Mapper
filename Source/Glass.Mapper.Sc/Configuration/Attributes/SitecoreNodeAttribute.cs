using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreNodeAttribute : NodeAttribute
    {
        public SitecoreNodeAttribute():base()
        {
        }


        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new SitecoreNodeConfiguration();
            Configure(propertyInfo, config);
            return config;
        }

        public void Configure(PropertyInfo propertyInfo, SitecoreNodeConfiguration config)
        {
            base.Configure(propertyInfo, config);
        }
    }
}
