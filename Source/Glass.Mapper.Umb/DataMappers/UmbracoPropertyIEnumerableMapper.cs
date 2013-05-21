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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Umb.Configuration;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// Class UmbracoPropertyIEnumerableMapper
    /// </summary>
    public class UmbracoPropertyIEnumerableMapper : AbstractUmbracoPropertyMapper
    {
        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        public AbstractUmbracoPropertyMapper Mapper { get; private set; }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override object GetPropertyValue(object propertyValue, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            Type type = config.PropertyInfo.PropertyType;
            //Get generic type
            Type pType = Utilities.GetGenericArgument(type);

            //The enumerator only works with piped lists
            IEnumerable<string> parts = propertyValue.ToString().Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            IEnumerable<object> items = parts.Select(x => Mapper.GetPropertyValue(x, Mapper.Configuration as UmbracoPropertyConfiguration, context)).ToArray();
            var list = Glass.Mapper.Utilities.CreateGenericType(typeof(List<>), new [] { pType }) as IList;

            foreach (var item in items)
            {
                if (item != null)
                    if (list != null) list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Sets the Property value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        public override object SetPropertyValue(object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            var list = value as IEnumerable;

            if (list == null)
            {
                return string.Empty;
            }

            var sList = new List<string>();
            
            foreach (object obj in list)
            {
                string result = Mapper.SetPropertyValue(obj, config, context).ToString();
                if (!result.IsNullOrEmpty())
                    sList.Add(result);
            }

            if (sList.Any())
                return sList.Aggregate((x, y) => x + "," + y);
            
            return string.Empty;
        }

        /// <summary>
        /// Determines whether this instance can handle the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            var scConfig = configuration as UmbracoPropertyConfiguration;

            if (scConfig == null)
                return false;

            Type type = scConfig.PropertyInfo.PropertyType;

            if (!type.IsGenericType) return false;

            if (type.GetGenericTypeDefinition() != typeof(IEnumerable<>) && type.GetGenericTypeDefinition() != typeof(IList<>))
                return false;

            return true;
        }

        /// <summary>
        /// Sets up the data mapper for a particular property
        /// </summary>
        /// <param name="args">The args.</param>
        /// <exception cref="Glass.Mapper.MapperException">No mapper to handle type {0} on property {1} class {2}.Formatted(type.FullName, property.Name,
        ///                                                                                        property.ReflectedType.FullName)</exception>
        public override void Setup(DataMapperResolverArgs args)
        {
            base.Setup(args);

            var config = Configuration as UmbracoPropertyConfiguration;

            var property = args.PropertyConfiguration.PropertyInfo;
            var type = Utilities.GetGenericArgument(property.PropertyType);

            var configCopy = config.Copy();
            configCopy.PropertyInfo = new FakePropertyInfo(type, property.Name, property.DeclaringType);

            Mapper =
                args.DataMappers.FirstOrDefault(
                    x => x.CanHandle(configCopy, args.Context) && x is AbstractUmbracoPropertyMapper) 
                    as AbstractUmbracoPropertyMapper;
            
            if (Mapper == null)
                throw new MapperException(
                    "No mapper to handle type {0} on property {1} class {2}".Formatted(type.FullName, property.Name,
                                                                                       property.ReflectedType.FullName));

            Mapper.Setup(new DataMapperResolverArgs(args.Context, configCopy));
        }
    }
}

