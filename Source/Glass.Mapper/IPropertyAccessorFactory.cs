using System;
using System.Reflection;

namespace Glass.Mapper
{
    public interface IPropertyAccessorFactory
    {
        Func<object, object> GetPropertyFunc(PropertyInfo property);
        Action<object, object> SetPropertyAction(PropertyInfo property);
    }
}
