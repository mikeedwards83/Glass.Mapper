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
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{

    /// <summary>
    /// Class SitecoreFieldIEnumerableMapper
    /// </summary>
    public class SitecoreFieldIEnumerableMapper : AbstractSitecoreFieldMapper
    {
        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        public AbstractSitecoreFieldMapper Mapper { get; private set; }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Type type = config.PropertyInfo.PropertyType;
            //Get generic type
            Type pType = Glass.Mapper.Utilities.GetGenericArgument(type);

            //The enumerator only works with piped lists
            IEnumerable<string> parts = fieldValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            //replace any pipe encoding with an actual pipe
            parts = parts.Select(x => x.Replace(Global.PipeEncoding, "|")).ToArray();
            
            
            
            IEnumerable<object> items = parts.Select(x => Mapper.GetFieldValue(x, Mapper.Configuration as SitecoreFieldConfiguration, context)).ToArray();
            var list = Utilities.CreateGenericType(typeof (List<>), new Type[] {pType}) as IList;
            
            foreach (var item in items)
            {
                if(item != null)
                    list.Add(item);
            }

            return list;
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

            IEnumerable list = value as IEnumerable;

            if (list == null)
            {
                return null;
            }

            List<string> sList = new List<string>();


            foreach (object obj in list)
            {
                string result = Mapper.SetFieldValue(obj, config, context);
                if (!result.IsNullOrEmpty())
                    sList.Add(result);
            }
            if (sList.Any())
                return sList.Aggregate((x, y) => x + "|" + y);
            else
                return null;
        }

        /// <summary>
        /// Determines whether this instance can handle the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            var scConfig = configuration as SitecoreFieldConfiguration;

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

            var scConfig = Configuration as SitecoreFieldConfiguration;

            var property = args.PropertyConfiguration.PropertyInfo;
            var type = Glass.Mapper.Utilities.GetGenericArgument(property.PropertyType);

            var configCopy = scConfig.Copy();
            configCopy.PropertyInfo = new FakePropertyInfo(type, property.Name, property.DeclaringType);

            Mapper =
                args.DataMappers.FirstOrDefault(
                    x => x.CanHandle(configCopy, args.Context) && x is AbstractSitecoreFieldMapper) 
                    as AbstractSitecoreFieldMapper;


            if (Mapper == null)
                throw new MapperException(
                    "No mapper to handle type {0} on property {1} class {2}".Formatted(type.FullName, property.Name,
                                                                                       property.ReflectedType.FullName));

            Mapper.Setup(new DataMapperResolverArgs(args.Context, configCopy));
        }
    }
}




