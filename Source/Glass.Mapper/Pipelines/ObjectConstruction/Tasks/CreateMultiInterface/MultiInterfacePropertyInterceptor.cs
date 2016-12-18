using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface
{
    public class MultiInterfacePropertyInterceptor : IInterceptor
    {
        private readonly ObjectConstructionArgs _args;
		private readonly Lazy<IDictionary<string, object>> _lazyValues;

		protected IDictionary<string, object> Values { get { return _lazyValues.Value; } }
        private IEnumerable<AbstractTypeConfiguration> _configs;
 
          /// <summary>
        /// Initializes a new instance of the <see cref="MultiInterfacePropertyInterceptor"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public MultiInterfacePropertyInterceptor(ObjectConstructionArgs args)
        {
            _args = args;
              _configs =
                  new[] {args.Configuration}.Union(args.Parameters[CreateMultiInferaceTask.MultiInterfaceConfigsKey] as IEnumerable<AbstractTypeConfiguration>);
			  _lazyValues = new Lazy<IDictionary<string, object>>(LoadValues);
        }

	    public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.IsSpecialName)
            {
                if (invocation.Method.Name.StartsWith("get_") || invocation.Method.Name.StartsWith("set_"))
                {

                    string method = invocation.Method.Name.Substring(0, 4);
                    string name = invocation.Method.Name.Substring(4);

                    if (method == "get_")
                    {
                        var result = Values[name];
                        invocation.ReturnValue = result;
                    }
                    else if (method == "set_")
                    {
						Values[name] = invocation.Arguments[0];
                    }
                    else
                        throw new MapperException("Method with name {0}{1} on types {2} not supported.".Formatted(
                            method, name, _configs.Select(x => x.Type.FullName).Aggregate((x, y) => x + "; " + y)));
                }
            }

        }

		private IDictionary<string, object> LoadValues()
		{
		    _args.Counters.ModelsMapped++;

			var values = new Dictionary<string, object>();
			var mappingContext = _args.Service.CreateDataMappingContext(_args.AbstractTypeCreationContext, null);

			foreach (var config in _configs)
			{
				foreach (var property in config.Properties)
				{
					var result = property.Mapper.MapToProperty(mappingContext);
					values[property.PropertyInfo.Name] = result;
				}
			}

			return values;
		}
    }
}
