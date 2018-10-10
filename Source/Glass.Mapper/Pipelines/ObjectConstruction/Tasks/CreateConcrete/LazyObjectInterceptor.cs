


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Diagnostics;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{


    /// <summary>
    /// Class LazyObjectInterceptor
    /// </summary>
    [Serializable]
    public class LazyObjectInterceptor : IInterceptor, ISerializable
    {
        protected IDictionary<string, object> Values { get; private set; }

        [NonSerialized]
        private AbstractDataMappingContext _mappingContext;

        [NonSerialized]
        private ObjectConstructionArgs _args;

        private bool _mapRequested = false;

        public LazyObjectInterceptor(ObjectConstructionArgs args, LazyLoadingHelper lazyLoadingHelper)
        {
            _args = args;
            Values = new ConcurrentDictionary<string, object>();
            _mappingContext = _args.AbstractTypeCreationContext.CreateDataMappingContext(null);

            //if lazy loading diabled load all values now
            if (!lazyLoadingHelper.IsEnabled(args.Options))
            {
                LoadAllValues();
            }
        }

        public LazyObjectInterceptor(SerializationInfo info, StreamingContext context)
        {

            var pairs  = (KeyValuePair<string, object>[])info.GetValue("Values", typeof(KeyValuePair<string, object>[]));

            Values = new ConcurrentDictionary<string, object>();

            foreach (var keyValuePair in pairs)
            {
                Values.Add(keyValuePair);
            }
        }

        private void LoadAllValues()
        {
            foreach (var property in _args.Configuration.Properties)
            {
                GetValue(property);
            }
        }

        #region IInterceptor Members

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            using (new Monitor())
            {
                if (invocation.Method.Name.StartsWith("get_") || invocation.Method.Name.StartsWith("set_"))
                {
                    string method = invocation.Method.Name.Substring(0, 4);
                    string name = invocation.Method.Name.Substring(4);


                    var property = _args == null ? null : _args.Configuration[name];

                    if (property != null || Values.ContainsKey(name)
                    ) //check values as well encase the object has already been deserialised
                    {
                        if (method == "get_") //&& Values.ContainsKey(name))
                        {
                            invocation.ReturnValue = GetValue(property);
                            return;
                        }
                        else if (method == "set_")
                        {

                            Values[name] = invocation.Arguments[0];
                            return;
                        }
                        else
                        {
                            throw new MapperException("Method with name {0}{1} on type {2} not supported.".Formatted(
                                method, name, _args.Configuration.Type.FullName));
                        }
                    }

                }

                invocation.Proceed();
            }
        }

        #endregion



        private object GetValue(AbstractPropertyConfiguration propertyConfiguration)
        {
           
                if (_mapRequested == false)
                {
                    _mapRequested = true;
                    ModelCounter.Instance.ModelsMapped++;
                }

                object result;

                var propName = propertyConfiguration.PropertyInfo.Name;

                if (Values.ContainsKey(propName))
                {
                    result = Values[propName];

                }
                else
                {
                    result = propertyConfiguration.Mapper.MapToProperty(_mappingContext) ??
                             Utilities.GetDefault(propertyConfiguration.PropertyInfo.PropertyType);

                    Values[propName] = result;
                }

                return result;
            
        }

       

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            LoadAllValues();
            var pairs = Values.ToArray();
            info.AddValue("Values", pairs, typeof(KeyValuePair<string, object>[]));

        }
    }
}






