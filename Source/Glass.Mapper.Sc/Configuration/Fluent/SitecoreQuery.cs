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

/*
   Copyright 2011 Michael Edwards
 
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Indicates that a query should be execute to load data into the property.
    /// The query can be either absolute or relative to the current item.
    /// </summary>
    public class SitecoreQuery<T> : AbstractPropertyBuilder<T, SitecoreQueryConfiguration>
    {

        public SitecoreQuery(Expression<Func<T, object>> ex):base(ex)
        {
        }

        /// <summary>
        /// Indicates that the results should be loaded lazily
        /// </summary>
        /// <returns></returns>
        public SitecoreQuery<T> IsNotLazy()
        {
            Configuration.IsLazy = false;
            return this;
        }
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        public SitecoreQuery<T> InferType()
        {
            Configuration.InferType = true;
            return this;
        }
        /// <summary>
        /// Use the Sitecore.Data.Query.QueryContext when querying for data
        /// </summary>
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
        public SitecoreQuery<T> Query(string query)
        {
            Configuration.Query = query;
            return this;
        }


      

    }
}



