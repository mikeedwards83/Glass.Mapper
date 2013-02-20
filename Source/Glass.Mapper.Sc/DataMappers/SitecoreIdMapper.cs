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
using Sitecore.Data;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreIdMapper : AbstractDataMapper
    {

        public SitecoreIdMapper()
        {
            this.ReadOnly = true;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {

            SitecoreDataMappingContext context = (SitecoreDataMappingContext)mappingContext;
            var item = context.Item;

            var scConfig = Configuration;

            if (scConfig.PropertyInfo.PropertyType == typeof(Guid))
                return item.ID.Guid;
            else if (scConfig.PropertyInfo.PropertyType == typeof(ID))
                return item.ID;
            else
            {
                throw new NotSupportedException("The type {0} on {0}.{1} is not supported by SitecoreIdMapper".Formatted
                                                    (scConfig.PropertyInfo.ReflectedType.FullName,
                                                        scConfig.PropertyInfo.Name));
            }

        }



        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreIdConfiguration;
        }
    }
}



