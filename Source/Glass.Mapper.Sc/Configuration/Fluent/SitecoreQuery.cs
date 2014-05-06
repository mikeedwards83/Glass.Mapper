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
using System.Linq.Expressions;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Indicates that a query should be execute to load data into the property.
    /// The query can be either absolute or relative to the current item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreQuery<T> : AbstractPropertyBuilder<T, SitecoreQueryConfiguration>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreQuery{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreQuery(Expression<Func<T, object>> ex):base(ex)
        {
            Configuration.IsLazy = true;
        }

        /// <summary>
        /// Indicates that the results should be loaded lazily
        /// </summary>
        public SitecoreQuery<T> IsNotLazy()
        {
            Configuration.IsLazy = false;
            return this;
        }
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <returns>SitecoreQuery{`0}.</returns>
        public SitecoreQuery<T> InferType()
        {
            Configuration.InferType = true;
            return this;
        }
        /// <summary>
        /// Use the Sitecore.Data.Query.QueryContext when querying for data
        /// </summary>
        /// <returns>SitecoreQuery{`0}.</returns>
        public SitecoreQuery<T> UseQueryContext()
        {
            Configuration.UseQueryContext = true;
            return this;
        }
        /// <summary>
        /// Indicates that the field is relative to the current item.
        /// </summary>
        public SitecoreQuery<T> IsRelative()
        {
            Configuration.IsRelative = true;
            return this;
        }
        /// <summary>
        /// The query to execute
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>SitecoreQuery{`0}.</returns>
        public SitecoreQuery<T> Query(string query)
        {
            Configuration.Query = query;
            return this;
        }


      

    }
}




