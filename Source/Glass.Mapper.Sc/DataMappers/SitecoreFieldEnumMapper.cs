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
    public class SitecoreFieldEnumMapper : AbstractSitecoreFieldMapper
    {
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Type enumType = config.PropertyInfo.PropertyType;

            int intValue;
            if (int.TryParse(fieldValue, out intValue))
            {
                return Enum.ToObject(enumType, intValue);
            }
            else
            {
                if (Enum.IsDefined(enumType, fieldValue))
                    return Enum.Parse(enumType, fieldValue, true);
                else if (!fieldValue.IsNullOrEmpty())
                    throw new MapperException("Can not convert value {0} to enum type {1}".Formatted(fieldValue, enumType.FullName));
                else
                    throw new MapperException("Can not convert value {0} to enum type {1}".Formatted(fieldValue, enumType.FullName));

            }
        }

        public override string SetFieldValue( object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return Enum.GetName(config.PropertyInfo.PropertyType, value);
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration.PropertyInfo.PropertyType.IsEnum;
        }
    }
}



