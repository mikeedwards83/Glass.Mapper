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
    public class SitecoreQueryMapper : AbstractDataMapper
    {
        List<ISitecoreQueryParameter> _parameters;


        public SitecoreQueryMapper(IEnumerable<ISitecoreQueryParameter> parameters)
        {

            _parameters = new List<ISitecoreQueryParameter>();
            if (parameters != null)
                _parameters.AddRange(parameters);

            //default parameters
            _parameters.Add(new ItemDateNowParameter());
            _parameters.Add(new ItemIdParameter());
            _parameters.Add(new ItemPathParameter());
            _parameters.Add(new ItemIdNoBracketsParameter());
            _parameters.Add(new ItemEscapedPathParameter());

            ReadOnly = true;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

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
                    Type genericType = Utilities.GetGenericArgument(scConfig.PropertyInfo.PropertyType);

                    Func<IEnumerable<Item>> getItems = null;
                    if (scConfig.IsRelative)
                    {
                        getItems = new Func<IEnumerable<Item>>(() =>
                        {

                            return Utilities.GetLanguageItems(scContext.Item.Axes.SelectItems(query), scContext.Item.Language);
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

                                return Utilities.GetLanguageItems(contextArray.Select(x => scContext.Item.Database.GetItem(x.ID)), scContext.Item.Language);
                            }
                            else
                                return Utilities.GetLanguageItems(scContext.Item.Database.SelectItems(query), scContext.Item.Language);
                        });
                    }
                    var array = getItems.Invoke().ToArray();


                    return scContext.Service.CreateTypes(scConfig.IsLazy, scConfig.InferType, genericType, getItems);
                }
                else throw new NotSupportedException("Generic type not supported {0}. Must be IEnumerable<>.".Formatted(outerType.FullName));
            }
            else
            {
                Item result = null;
                if (scConfig.IsRelative)
                {
                    result = Utilities.GetLanguageItem(scContext.Item.Axes.SelectSingleItem(query), scContext.Item.Language);
                }
                else
                {
                    result = Utilities.GetLanguageItem(scContext.Item.Database.SelectSingleItem(query), scContext.Item.Language);
                }
                return scContext.Service.CreateType(scConfig.PropertyInfo.PropertyType, result, scConfig.IsLazy, scConfig.InferType);
            }
        }
       



        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            if (!(configuration is SitecoreQueryConfiguration)) return false;

            if (configuration.PropertyInfo.PropertyType.IsGenericType)
            {

                Type outerType = Utilities.GetGenericOuter(configuration.PropertyInfo.PropertyType);
                Type innerType = Utilities.GetGenericArgument(configuration.PropertyInfo.PropertyType);

                return typeof (IEnumerable<>) == outerType && context.TypeConfigurations.ContainsKey(innerType);
            }
            else
            {
                return context.TypeConfigurations.ContainsKey(configuration.PropertyInfo.PropertyType);
            }
        }

        public string ParseQuery(string query, Item item)
        {
            StringBuilder sb = new StringBuilder(query);
            foreach (var param in _parameters)
            {
                sb.Replace("{" + param.Name + "}", param.GetValue(item));
            }
            return sb.ToString();
        }

        
    }
}



