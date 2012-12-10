using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreIdAttribute : IdAttribute
    {
        public SitecoreIdAttribute():base(new []{typeof(ID), typeof(Guid)} ) { }

        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new SitecoreIdConfiguration();
            base.Configure(propertyInfo, config);
            return config;
        }

        public void Configure(PropertyInfo propertyInfo, SitecoreIdConfiguration config)
        {
            base.Configure(propertyInfo, config);
        }
    }
}
