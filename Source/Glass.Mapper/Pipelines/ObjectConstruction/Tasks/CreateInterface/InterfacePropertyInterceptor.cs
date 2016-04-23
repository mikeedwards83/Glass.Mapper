/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/
//-CRE-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface
{
    /// <summary>
    /// Class InterfacePropertyInterceptor
    /// </summary>
    [Serializable]
    public class InterfacePropertyInterceptor : IInterceptor, ISerializable
    {
        [NonSerialized]
        private ObjectConstructionArgs _args;


        protected IDictionary<string, object> Values { get; private set; }

        private readonly string _fullName;

        [NonSerialized]
        private AbstractDataMappingContext _mappingContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfacePropertyInterceptor"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public InterfacePropertyInterceptor(ObjectConstructionArgs args)
        {
            _args = args;
            _fullName = _args.Configuration.Type.FullName;
            Values = new Dictionary<string, object>();

            _mappingContext = _args.Service.CreateDataMappingContext(_args.AbstractTypeCreationContext, null);
        }

        public InterfacePropertyInterceptor(SerializationInfo info, StreamingContext context)
        {
           Values = (Dictionary<string, object>) info.GetValue("Values", typeof(Dictionary<string, object>));

        }


        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <exception cref="Glass.Mapper.MapperException">Method with name {0}{1} on type {2} not supported..Formatted(method, name, _args.Configuration.Type.FullName)</exception>
        public void Intercept(IInvocation invocation)
        {

            if (invocation.Method.IsSpecialName)
            {
                if (invocation.Method.Name.StartsWith("get_") || invocation.Method.Name.StartsWith("set_"))
                {

                    string method = invocation.Method.Name.Substring(0, 4);
                    string name = invocation.Method.Name.Substring(4);


                    if (method == "get_")//&& Values.ContainsKey(name))
                    {
                        if (Values.ContainsKey(name))
                        {
                            var result = Values[name];
                            invocation.ReturnValue = result;
                            return;
                        }


                        var property = _args == null ? null: _args.Configuration.Properties.FirstOrDefault(x => x.PropertyInfo.Name == name);
                        if (property != null)
                        {
                            LoadValue(property);
                            invocation.ReturnValue = Values[name]; 
                            return;
                        }
                        else
                        {
                            invocation.ReturnValue = Utilities.GetDefault(invocation.Method.ReturnType);
                            return;
                        }

                    }
                    else if (method == "set_")
                    {
                        Values[name] = invocation.Arguments[0];
                    }
                    else
                    {

                        throw new MapperException("Method with name {0}{1} on type {2} not supported.".Formatted(
                            method, name, _fullName));
                    }
                }
            }
        }

        private void LoadValue(AbstractPropertyConfiguration propertyConfiguration)
        {
            var result = propertyConfiguration.Mapper.MapToProperty(_mappingContext) ?? Utilities.GetDefault(propertyConfiguration.PropertyInfo.PropertyType);
            Values[propertyConfiguration.PropertyInfo.Name] = result;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var property in _args.Configuration.Properties)
            {
                LoadValue(property);
            }

            info.AddValue("Values", Values, typeof(Dictionary<string, object>));
          
        }
    }
}




