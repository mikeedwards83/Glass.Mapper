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

namespace Glass.Mapper
{
    /// <summary>
    /// The base class for the context loading an item from the CMS
    /// </summary>
    public abstract class AbstractTypeCreationContext
    {
        public AbstractTypeCreationContext()
        {
            Parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether [infer type].
        /// </summary>
        /// <value><c>true</c> if [infer type]; otherwise, <c>false</c>.</value>
        public bool InferType { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is lazy.
        /// </summary>
        /// <value><c>true</c> if this instance is lazy; otherwise, <c>false</c>.</value>
        public bool IsLazy { get; set; }
        /// <summary>
        /// Gets or sets the types requested.
        /// </summary>
        /// <value>The types requested.</value>
        public Type RequestedType { get; set; }
        /// <summary>
        /// Gets or sets the constructor parameters.
        /// </summary>
        /// <value>The constructor parameters.</value>
        public object[] ConstructorParameters { get; set; }

        /// <summary>
        /// Parameters that will be passed to the pipelines
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }

        public abstract bool CacheEnabled { get; }

        public virtual string DataSummary()
        {
            return string.Empty;
        }
    }
}



