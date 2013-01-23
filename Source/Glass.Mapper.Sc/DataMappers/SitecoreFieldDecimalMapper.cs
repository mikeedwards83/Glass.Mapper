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
using System.Globalization;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldDecimalMapper : AbstractSitecoreFieldMapper
    {
        public SitecoreFieldDecimalMapper() : base(typeof (decimal))
        {

        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config,
                                             SitecoreDataMappingContext context)
        {
            if (fieldValue.IsNullOrEmpty()) return 0M;

            decimal dValue = 0;
            if (decimal.TryParse(fieldValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dValue))
                return dValue;
            else throw new MapperException("Could not convert value to decimal");
        }

        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value is decimal)
            {
                return value.ToString();
            }
            else
                throw new NotSupportedException("The value is not of type System.Decimal");
        }
    }
}



