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
using System.Reflection;
using System.Text;

namespace Glass.Mapper.Configuration.Attributes
{
    public abstract class QueryAttribute : AbstractPropertyAttribute
    {
        /// <summary>
        /// The query to execute
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Indicates that the results should be loaded lazily
        /// </summary>
        public bool IsLazy { get; set; }

        /// <summary>
        /// Indicates that the field is relative to the current item.
        /// </summary>
        public bool IsRelative { get; set; }

        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        public bool InferType { get; set; }

        public QueryAttribute(string query)
        {
            IsLazy = true;
            Query = query;
        }

        public  void Configure(PropertyInfo propertyInfo, QueryConfiguration config)
        {
            config.Query = this.Query;
            config.IsLazy = this.IsLazy;
            config.IsRelative = this.IsRelative;
            config.InferType = this.InferType;

            base.Configure(propertyInfo, config);
        }

    }
}



