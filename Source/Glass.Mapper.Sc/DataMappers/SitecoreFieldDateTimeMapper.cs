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

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldDateTimeMapper
    /// </summary>
    public class SitecoreFieldDateTimeMapper : AbstractSitecoreFieldMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldDateTimeMapper"/> class.
        /// </summary>
        public SitecoreFieldDateTimeMapper():
            base(typeof(DateTime))
        {

        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetFieldValue(string fieldValue, Configuration.SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            var isoDate = Sitecore.DateUtil.IsoDateToDateTime(fieldValue);
#if (SC80 || SC81)
            return fieldValue.EndsWith("Z", StringComparison.OrdinalIgnoreCase) 
                ? Sitecore.DateUtil.ToServerTime(isoDate) 
                : isoDate;
#else
            return isoDate;
#endif
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotSupportedException">The value is not of type System.DateTime</exception>
        public override string SetFieldValue(object value, Configuration.SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value is DateTime)
            {
                DateTime date = (DateTime)value;

#if (SC80 || SC81)
                return global::Sitecore.DateUtil.ToIsoDate(date, false, true);
#else
                return global::Sitecore.DateUtil.ToIsoDate(date);
#endif
            }
            else
                throw new NotSupportedException("The value is not of type System.DateTime");
        }
    }
}




