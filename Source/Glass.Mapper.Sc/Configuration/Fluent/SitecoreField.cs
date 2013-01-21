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
using System.Reflection;
using System.Linq.Expressions;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Used to populate the property with data from a Sitecore field
    /// </summary>
    public class SitecoreField<T> : AbstractPropertyBuilder<T, SitecoreFieldConfiguration>
	{

        public SitecoreField(Expression<Func<T, object>> ex)
            : base(ex)
        {
        }

        /// <summary>
        /// Indicate that the field can not be written to Sitecore
        /// </summary>
        public SitecoreField<T> ReadOnly()
        {
            Configuration.ReadOnly = true;
            return this;
        }
        /// <summary>
        /// The name of the field  to use if it is different to the property name
        /// </summary>
        public SitecoreField<T> FieldName(string name)
        {
            Configuration.FieldName = name;
            return this;
        }
        /// <summary>
        /// Options to override the behaviour of certain fields.
        /// </summary>
        public SitecoreField<T> Setting(SitecoreFieldSettings setting)
        {
            Configuration.Setting = setting;
            return this;
        }



    }
}
