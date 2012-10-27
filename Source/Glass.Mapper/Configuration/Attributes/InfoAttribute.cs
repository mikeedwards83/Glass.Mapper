using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Glass.Mapper.Configuration.Attributes
{
    public abstract class  InfoAttribute : AbstractPropertyAttribute
    {
       public void Configure(PropertyInfo propertyInfo, InfoConfiguration config)
       {
           base.Configure(propertyInfo, config);
       }
    }
}
