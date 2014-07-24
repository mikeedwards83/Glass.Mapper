using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.ContentSearch.Pipelines.ObjectConstruction.Tasks.SearchProxy
{
        public class SearchInterceptor : IInterceptor
        {
            private bool _isLoaded = false;
            private static object _loadlock = new object();
            private readonly ObjectConstructionArgs _args;

            public SitecoreTypeConfiguration TypeConfiguration { get; set; }

            public ID Id { get; set; }
            //_defaultdocummenttypemapper will call set_PROPERTYNAME for all index-stored fields, capture those, and only load others when needed
            private readonly IDictionary<string, object> _fieldValues = new Dictionary<string, object>();

            public SearchInterceptor(ObjectConstructionArgs args)
            {
                _args = args;
            }

            public void Intercept(IInvocation invocation)
            {
                if (!invocation.Method.IsSpecialName || !invocation.Method.Name.StartsWith("get_") && !invocation.Method.Name.StartsWith("set_"))
                    return;
                var str = invocation.Method.Name.Substring(0, 4);
                var name = invocation.Method.Name.Substring(4);

                if (str == "get_")
                {
                    if (_fieldValues.ContainsKey(name))
                    {
                        var obj = _fieldValues[name];
                        invocation.ReturnValue = obj;
                        return;
                    }
                    if (_isLoaded) return;
                    lock (_loadlock)
                    { 
                        if (!_isLoaded)
                        {
                            var typeCreationContext = ((SitecoreTypeCreationContext)_args.AbstractTypeCreationContext);
                            typeCreationContext.Item = typeCreationContext.SitecoreService.Database.GetItem(Id);
                            var typeConfiguration = 
                                typeCreationContext.SitecoreService.GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(invocation.TargetType) ??
                                typeCreationContext.SitecoreService.GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(_args.AbstractTypeCreationContext.RequestedType);
                            var dataMappingContext = _args.Service.CreateDataMappingContext(_args.AbstractTypeCreationContext, null);
                            foreach (var propertyConfiguration in typeConfiguration.Properties.Where(x => !_fieldValues.ContainsKey(x.PropertyInfo.Name)))
                            {
                                _fieldValues[propertyConfiguration.PropertyInfo.Name] = propertyConfiguration.Mapper.MapToProperty(dataMappingContext);
                            }
                            _isLoaded = true;
                        }
                    }
                    if (_fieldValues.ContainsKey(name))
                    {
                        var obj = _fieldValues[name];
                        invocation.ReturnValue = obj;
                        return;
                    }
                }
                else if (str == "set_")
                {
                    _fieldValues[name] = invocation.Arguments[0];
                    return;
                }
                else
                    throw new MapperException(
                        Glass.Mapper.ExtensionMethods.Formatted("Method with name {0}{1} on type {2} not supported.",
                            (object) str, (object) name, (object) this._args.Configuration.Type.FullName));
            }
        }
    }


