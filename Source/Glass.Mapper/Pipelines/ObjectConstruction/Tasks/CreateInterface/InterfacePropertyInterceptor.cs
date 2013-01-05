using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface
{
    public class InterfacePropertyInterceptor : IInterceptor
    {
        private readonly ObjectConstructionArgs _args;
        Dictionary<string, object> _values;
        bool _isLoaded = false;

        public InterfacePropertyInterceptor(ObjectConstructionArgs args)
        {
            _args = args;
        }

        public void Intercept(IInvocation invocation)
        {
            //do initial gets
            if (!_isLoaded)
            {
                _values = new Dictionary<string, object>();
                var config = _args.Configuration;
                var mappingContext = _args.Service.CreateDataMappingContext(_args.AbstractTypeCreationContext, null);

                foreach (var property in config.Properties)
                {
                    var result = property.Mapper.MapToProperty(mappingContext);
                    _values[property.PropertyInfo.Name] = result;
                }
                _isLoaded = true;
            }

            if (invocation.Method.IsSpecialName)
            {
                if (invocation.Method.Name.StartsWith("get_") || invocation.Method.Name.StartsWith("set_"))
                {

                    string method = invocation.Method.Name.Substring(0, 4);
                    string name = invocation.Method.Name.Substring(4);

                    if (method == "get_")
                    {
                        var result = _values[name];
                        invocation.ReturnValue = result;
                    }
                    else if (method == "set_")
                    {
                        _values[name] = invocation.Arguments[0];
                    }
                    else
                        throw new MapperException("Method with name {0}{1} on type {2} not supported.".Formatted(method, name, _args.Configuration.Type.FullName));
                }
            }
        }
    }
}
