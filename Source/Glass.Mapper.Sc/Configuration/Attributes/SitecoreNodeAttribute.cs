using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreNodeAttribute : NodeAttribute
    {
        public SitecoreNodeAttribute():base(typeof(Guid))
        {
        }


        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }
    }
}
