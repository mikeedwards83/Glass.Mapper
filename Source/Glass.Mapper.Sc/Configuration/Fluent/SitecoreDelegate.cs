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
    /// Delegates control of a field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreDelegate<T> : AbstractPropertyBuilder<T, SitecoreDelegateConfiguration>
    {
        public SitecoreDelegate(Expression<Func<T, object>> fieldExpression)
            : base(fieldExpression)
        {
        }

        /// <summary>
        /// Sets the value to be stored by the cms
        /// </summary>
        /// <param name="mapAction"></param>
        /// <returns></returns>
        public SitecoreDelegate<T> SetValue(Action<SitecoreDataMappingContext> mapAction)
        {
            Configuration.MapToCmsAction = mapAction;
            return this;
        }

        /// <summary>
        /// Gets the value to be returned in the object
        /// </summary>
        /// <param name="mapFunction"></param>
        /// <returns></returns>
        public SitecoreDelegate<T> GetValue(Func<SitecoreDataMappingContext, object> mapFunction)
        {
            Configuration.MapToPropertyAction = mapFunction;
            return this;
        }
    }
}