using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration.Attributes;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreIdAttribute : IdAttribute
    {
        public SitecoreIdAttribute():base(typeof(ID)) { }

        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }
    }
}
