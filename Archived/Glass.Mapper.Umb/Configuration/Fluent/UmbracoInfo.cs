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

namespace Glass.Mapper.Umb.Configuration.Fluent
{
    /// <summary>
    /// Used to map item information to a class property
    /// </summary>
    public class UmbracoInfo<T> : AbstractPropertyBuilder<T, UmbracoInfoConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoInfo{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public UmbracoInfo(Expression<Func<T, object>> ex)
            : base(ex)
        {
        }

        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public UmbracoInfo<T> InfoType(UmbracoInfoType type)
        {
            Configuration.Type = type;
            return this;
        }
    }
}




