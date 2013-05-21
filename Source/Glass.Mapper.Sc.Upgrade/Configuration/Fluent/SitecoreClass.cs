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
using System.Linq.Expressions;
using System.Text;
using Glass.Mapper.Sc.Configuration.Fluent;

namespace Glass.Mapper.Sc.Upgrade.Configuration.Fluent
{
    /// <summary>
    /// SitecoreClass
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreClass<T>: SitecoreType<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreClass{T}"/> class.
        /// </summary>
        public SitecoreClass()
        {

        }

        /// <summary>
        /// Map a Sitecore item to a class property
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreNode<T> Item(Expression<Func<T, object>> ex)
        {
            return Node(ex);
        }


        /// <summary>
        /// Map Sitecore items to a class properties
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public SitecoreType<T> Items(Action<ISitecoreClassNodes<T>> items)
        {
            items.Invoke(this);
            return this;
        }
    }
}

