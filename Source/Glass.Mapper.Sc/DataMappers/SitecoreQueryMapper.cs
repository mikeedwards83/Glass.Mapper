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
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using Sitecore.Data.Items;
using Sitecore.Data.Query;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreQueryMapper
    /// </summary>
    public class SitecoreQueryMapper : AbstractDataMapper
    {

        private readonly IEnumerable<ISitecoreQueryParameter> _parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreQueryMapper"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public SitecoreQueryMapper(IEnumerable<ISitecoreQueryParameter> parameters)
        {
            _parameters = parameters;
            ReadOnly = true;
        }

        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>The value to write</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>System.Object.</returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreQueryConfiguration;
            var scContext = mappingContext as SitecoreDataMappingContext;

            string query = ParseQuery(scConfig.Query, scContext.Item);

            if (scConfig.PropertyInfo.PropertyType.IsGenericType)
            {
                Type outerType = Utilities.GetGenericOuter(scConfig.PropertyInfo.PropertyType);

                if (typeof(IEnumerable<>) == outerType)
                {
                    Type genericType = Mapper.Utilities.GetGenericArgument(scConfig.PropertyInfo.PropertyType);

                    Func<IEnumerable<Item>> getItems = null;
                    if (scConfig.IsRelative)
                    {
                        getItems = new Func<IEnumerable<Item>>(() =>
                        {
                            try
                            {
                                return Utilities.GetLanguageItems(scContext.Item.Axes.SelectItems(query),
                                                                  scContext.Item.Language, scContext.Service.Config);
                            }
                            catch (Exception ex)
                            {
                                throw new MapperException("Failed to perform query {0}".Formatted(query), ex);
                            }
                        
                        });
                    }
                    else
                    {
                        getItems = new Func<IEnumerable<Item>>(() =>
                        {
                            if (scConfig.UseQueryContext)
                            {
                                Query conQuery = new Query(query);
                                QueryContext queryContext = new QueryContext(scContext.Item.Database.DataManager);

                                object obj = conQuery.Execute(queryContext);
                                QueryContext[] contextArray = obj as QueryContext[];
                                QueryContext context = obj as QueryContext;

                                if (contextArray == null)
                                    contextArray = new QueryContext[] { context };

                                return Utilities.GetLanguageItems(contextArray.Select(x => scContext.Item.Database.GetItem(x.ID)), scContext.Item.Language,  scContext.Service.Config);
                            }
                            else
                                return Utilities.GetLanguageItems(scContext.Item.Database.SelectItems(query), scContext.Item.Language, scContext.Service.Config);
                        });
                    }

                    var result =  Utilities.CreateGenericType(typeof (LazyItemEnumerable<>), new []{genericType}, getItems, scConfig.IsLazy,
                                                scConfig.InferType, scContext.Service);
                    return result;

                    //return scContext.Service.CreateTypes(scConfig.IsLazy, scConfig.InferType, genericType, getItems);
                }
                else throw new NotSupportedException("Generic type not supported {0}. Must be IEnumerable<>.".Formatted(outerType.FullName));
            }
            else
            {
                Item result = null;
                if (scConfig.IsRelative)
                {
                    result = Utilities.GetLanguageItem(scContext.Item.Axes.SelectSingleItem(query), scContext.Item.Language, scContext.Service.Config);
                }
                else
                {
                    result = Utilities.GetLanguageItem(scContext.Item.Database.SelectSingleItem(query), scContext.Item.Language, scContext.Service.Config);
                }
                return scContext.Service.CreateType(scConfig.PropertyInfo.PropertyType, result, scConfig.IsLazy, scConfig.InferType, null);
            }
        }




        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            if (!(configuration is SitecoreQueryConfiguration)) return false;

            if (configuration.PropertyInfo.PropertyType.IsGenericType)
            {

                Type outerType = Utilities.GetGenericOuter(configuration.PropertyInfo.PropertyType);
                Type innerType = Mapper.Utilities.GetGenericArgument(configuration.PropertyInfo.PropertyType);

                return typeof (IEnumerable<>) == outerType;// && context.TypeConfigurations.ContainsKey(innerType);
            }
            else
            {
                //We are now assuming auto-mapping works
                return true;
                //return context.TypeConfigurations.ContainsKey(configuration.PropertyInfo.PropertyType);
            }
        }

        /// <summary>
        /// Parses the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        public string ParseQuery(string query, Item item)
        {
            StringBuilder sb = new StringBuilder(query);
            if (_parameters != null)
            {
                foreach (var param in _parameters)
                {
                    sb.Replace("{" + param.Name + "}", param.GetValue(item));
                }
            }
            return sb.ToString();
        }

        
    }
}




