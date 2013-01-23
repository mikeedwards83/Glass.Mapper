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

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreParentMapper :AbstractDataMapper
    {

        public SitecoreParentMapper()
        {
            ReadOnly = true;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotSupportedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scContext = mappingContext as SitecoreDataMappingContext;
            var scConfig = Configuration as SitecoreParentConfiguration;

            return scContext.Service.CreateType(
                scConfig.PropertyInfo.PropertyType,
                scContext.Item.Parent,
                scConfig.IsLazy,
                scConfig.InferType);
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreParentConfiguration;
        }
    }
}



