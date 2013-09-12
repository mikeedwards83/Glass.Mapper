using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface
{
    public class MultiInterfacePropertyInterceptor : IInterceptor
    {
        private readonly ObjectConstructionArgs _args;
        Dictionary<string, object> _values;
        bool _isLoaded = false;

          /// <summary>
        /// Initializes a new instance of the <see cref="MultiInterfacePropertyInterceptor"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public MultiInterfacePropertyInterceptor(ObjectConstructionArgs args)
        {
            _args = args;
        }

        public void Intercept(IInvocation invocation)
        {
            var configs = _args.Configurations;

            //do initial gets
            if (!_isLoaded)
            {
                _values = new Dictionary<string, object>();
                var mappingContext = _args.Service.CreateDataMappingContext(_args.AbstractTypeCreationContext, null);

                foreach (var config in configs)
                {
                    foreach (var property in config.Properties)
                    {
                        var result = property.Mapper.MapToProperty(mappingContext);
                        _values[property.PropertyInfo.Name] = result;
                    }
                    _isLoaded = true;
                }
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
                        throw new MapperException("Method with name {0}{1} on types {2} not supported.".Formatted(
                            method, name, configs.Select(x => x.Type.FullName).Aggregate((x, y) => x + "; " + y)));
                }
            }

        }
    }
}
