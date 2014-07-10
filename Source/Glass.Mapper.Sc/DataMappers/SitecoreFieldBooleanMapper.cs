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
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldBooleanMapper
    /// </summary>
    public class SitecoreFieldBooleanMapper : AbstractSitecoreFieldMapper
    {

        protected override object DefaultValue
        {
            get { return false; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldBooleanMapper"/> class.
        /// </summary>
        public SitecoreFieldBooleanMapper() : base(typeof (bool))
        {

        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return fieldValue == "1";
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        public override string SetFieldValue( object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value is bool)
            {
                bool actual = (bool)value;
                return actual ? "1" : "0";
            }
            else
                throw new NotSupportedException("The value is not of type System.Boolean");
        }
    }
}




