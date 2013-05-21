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
using Glass.Mapper.Umb.Configuration;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// UmbracoPropertyBooleanMapper
    /// </summary>
    public class UmbracoPropertyBooleanMapper : AbstractUmbracoPropertyMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoPropertyBooleanMapper"/> class.
        /// </summary>
        public UmbracoPropertyBooleanMapper()
            : base(typeof(bool))
        {
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override object SetPropertyValue(object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            if (value is bool && (bool)value)
                return "1";
            return "0";
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override object GetPropertyValue(object propertyValue, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            if (propertyValue != null && propertyValue.ToString() == "1")
                return true;
            return false;
        }
    }
}
