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
    /// Used to map item information to a class property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreItem<T> : AbstractPropertyBuilder<T, SitecoreItemConfiguration>
    {
        private readonly SitecoreTypeConfiguration _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInfo{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreItem(Expression<Func<T, object>> ex, SitecoreTypeConfiguration owner):base(ex)
        {
            _owner = owner;
        }
    }
}




