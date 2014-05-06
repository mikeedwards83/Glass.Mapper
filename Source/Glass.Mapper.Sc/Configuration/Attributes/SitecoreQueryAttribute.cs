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

using System.Reflection;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreQueryAttribute
    /// </summary>
    public class SitecoreQueryAttribute : QueryAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryAttribute" /> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public SitecoreQueryAttribute(string query):base(query)
        {
        }

        /// <summary>
        /// Use the Sitecore.Data.Query.QueryContext when querying for data
        /// </summary>
        /// <value><c>true</c> if [use query context]; otherwise, <c>false</c>.</value>
        public bool UseQueryContext { get; set; }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration.</returns>
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new SitecoreQueryConfiguration();
            Configure(propertyInfo, config);
            return config;
        }
        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, SitecoreQueryConfiguration config)
        {
            config.UseQueryContext = this.UseQueryContext;
            base.Configure(propertyInfo, (QueryConfiguration) config);
        }
    }
}




