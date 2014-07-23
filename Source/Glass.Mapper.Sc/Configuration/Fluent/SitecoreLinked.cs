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
    /// Indicates that the property should pull from the item links list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreLinked<T> : AbstractPropertyBuilder<T, SitecoreLinkedConfiguration>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreLinked{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreLinked(Expression<Func<T, object>> ex)
            : base(ex)
        {
        }


        /// <summary>
        /// Indicates if linked items should not be  loaded lazily. Default value is true. If set linked items will be loaded when the contain object is created.
        /// </summary>
        public SitecoreLinked<T> IsNotLazy()
        {
            Configuration.IsLazy = false;
            return this;
        }
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <returns>SitecoreLinked{`0}.</returns>
        public SitecoreLinked<T> InferType()
        {
            
            Configuration.InferType = false;
            return this;
        }
        /// <summary>
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>SitecoreLinked{`0}.</returns>
        public SitecoreLinked<T> Option(SitecoreLinkedOptions option)
        {
            Configuration.Option = option;
            return this;
        }

    }
}




