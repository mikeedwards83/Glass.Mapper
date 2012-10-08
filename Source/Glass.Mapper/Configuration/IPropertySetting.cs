using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Configuration
{
    public interface IPropertySetting
    {
        void Configure(PropertyInfo info, AbstractPropertyConfiguration config);

    }
}
