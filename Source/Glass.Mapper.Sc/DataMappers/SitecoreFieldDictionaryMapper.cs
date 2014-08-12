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
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web;

using Glass.Mapper;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldDictionaryMapper
    /// </summary>
    public class SitecoreFieldDictionaryMapper : AbstractSitecoreFieldMapper
    {
        /// <summary>
        /// Gets the mapper for keys.
        /// </summary>
        /// <value>The mapper.</value>
        public AbstractSitecoreFieldMapper KeyMapper { get; private set; }


        /// <summary>
        /// Gets the mapper for values.
        /// </summary>
        /// <value>The mapper.</value>
        public AbstractSitecoreFieldMapper ValueMapper { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldDictionaryMapper"/> class.
        /// </summary>
        public SitecoreFieldDictionaryMapper()
            : base(new Type[0])
        {
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            var collection = HttpUtility.ParseQueryString(fieldValue);

            Type[] genericArguments = config.PropertyInfo.PropertyType.GetGenericArguments();

            IDictionary dictionary = Utilities.CreateGenericType(typeof(Dictionary<,>), genericArguments, new object[0]) as IDictionary;

            foreach (var k in collection.AllKeys)
            {
                var key = this.KeyMapper.GetFieldValue(k, this.KeyMapper.Configuration as SitecoreFieldConfiguration, context);

                var value = this.ValueMapper.GetFieldValue(collection[k], this.ValueMapper.Configuration as SitecoreFieldConfiguration, context);

                dictionary.Add(key, value);
            }

            return dictionary;
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            var dictionary = value as IDictionary;
            
            if (dictionary == null)
            {
                return (string)null;
            }

            var parameters = new NameValueCollection();

            foreach (DictionaryEntry obj in dictionary)
            {
                string dictionaryKey = this.KeyMapper.SetFieldValue(obj.Key, config, context);

                string dictionaryValue = this.ValueMapper.SetFieldValue(obj.Value, config, context);

                if (!dictionaryKey.IsNullOrEmpty() || !dictionaryValue.IsNullOrEmpty())
                {
                    parameters.Add(dictionaryKey, dictionaryValue);
                }
            }

            return Utilities.ConstructQueryString(parameters);
        }

        /// <summary>
        /// Determines whether this instance can handle the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            var fieldConfiguration = configuration as SitecoreFieldConfiguration;

            if (fieldConfiguration == null)
            {
                return false;
            }

            Type propertyType = fieldConfiguration.PropertyInfo.PropertyType;

            return propertyType.IsGenericType && (!(propertyType.GetGenericTypeDefinition() != typeof(IDictionary<,>)));
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
            
            var fieldConfiguration = this.Configuration as SitecoreFieldConfiguration;
            
            PropertyInfo propertyInfo = args.PropertyConfiguration.PropertyInfo;

            Type[] genericArguments = propertyInfo.PropertyType.GetGenericArguments();
            
            this.KeyMapper = this.GetMapper(genericArguments[0], fieldConfiguration, propertyInfo, args);

            this.ValueMapper = this.GetMapper(genericArguments[1], fieldConfiguration, propertyInfo, args);
        }

        private AbstractSitecoreFieldMapper GetMapper(Type genericArgument, SitecoreFieldConfiguration fieldConfiguration, PropertyInfo propertyInfo, DataMapperResolverArgs args)
        {
            SitecoreFieldConfiguration configCopy = fieldConfiguration.Copy();

            configCopy.PropertyInfo = new FakePropertyInfo(genericArgument, propertyInfo.Name, propertyInfo.DeclaringType);

            var mapper = args.DataMappers.FirstOrDefault(
                    x => x.CanHandle(configCopy, args.Context) && x is AbstractSitecoreFieldMapper) 
                    as AbstractSitecoreFieldMapper;

            if (mapper == null)
            {
                throw new MapperException(Glass.Mapper.ExtensionMethods.Formatted(
                    "No mapper to handle type {0} on property {1} class {2}", 
                    (object)genericArgument.FullName, 
                    (object)propertyInfo.Name, 
                    (object)propertyInfo.ReflectedType.FullName));
            }
            else
            { 
                mapper.Setup(new DataMapperResolverArgs(args.Context, configCopy));
            }

            return mapper;
        }
    }
}