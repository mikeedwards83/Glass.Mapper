using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.ContentSearch.Pipelines.ObjectConstruction.Tasks.SearchProxy
{
    // Type: Glass.Mapper.Sc7.Pipelines.ObjectConstruction.Tasks.SearchProxy.SearchInterceptor
    // Assembly: Glass.Mapper.Sc7, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
    // Assembly location: E:\Sitecore\Sitecore7\Website\bin\Glass.Mapper.Sc7.dll

   
        public class SearchInterceptor : IInterceptor
        {
            private bool _isLoaded = false;
            private readonly ObjectConstructionArgs _args;
            private Dictionary<string, object> _values;

            public SitecoreTypeConfiguration TypeConfiguration { get; set; }

            public ID Id { get; set; }

            public IEnumerable<string> IndexFields{get; set; }

            public SearchInterceptor(ObjectConstructionArgs args)
            {
                IndexFields = new string[] {};
                _args = args;
                _values = new Dictionary<string, object>();
            }

            public void Intercept(IInvocation invocation)
            {
                if (IndexFields.Any(x => x == invocation.Method.Name))
                {
                    invocation.Proceed();
                }

                if (!invocation.Method.IsSpecialName || !invocation.Method.Name.StartsWith("get_") && !invocation.Method.Name.StartsWith("set_"))
                    return;
                string str = invocation.Method.Name.Substring(0, 4);
                string name = invocation.Method.Name.Substring(4);

                if (!_isLoaded)
                {
                    SitecoreTypeCreationContext typeCreationContext = _args.AbstractTypeCreationContext as SitecoreTypeCreationContext;
                    typeCreationContext.Item = typeCreationContext.SitecoreService.Database.GetItem(Id);
                    SitecoreTypeConfiguration typeConfiguration = TypeConfiguration;
                    AbstractDataMappingContext dataMappingContext = _args.AbstractTypeCreationContext.CreateDataMappingContext(null);

                    //todo filter fieldnames from FieldConfigs!
                    foreach (AbstractPropertyConfiguration propertyConfiguration in typeConfiguration.Properties.Where(x=> IndexFields.All(y=> y != x.PropertyInfo.Name)))
                    {
                        object obj = propertyConfiguration.Mapper.MapToProperty(dataMappingContext);
                        _values[propertyConfiguration.PropertyInfo.Name] = obj;
                    }
                    _isLoaded = true;
                }
                if (str == "get_")
                {
                    if (_values.ContainsKey(name))
                    {
                        object obj = _values[name];
                        invocation.ReturnValue = obj;
                    }
                }
                else if (str == "set_")
                    _values[name] = invocation.Arguments[0];
                else
                    throw new MapperException(Glass.Mapper.ExtensionMethods.Formatted("Method with name {0}{1} on type {2} not supported.", (object)str, (object)name, (object)this._args.Configuration.Type.FullName));
            }
        }
    }


