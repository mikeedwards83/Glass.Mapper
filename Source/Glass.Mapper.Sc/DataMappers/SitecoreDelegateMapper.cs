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
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{

    /// <summary>
    /// The delegate mapper
    /// </summary>
    public class SitecoreDelegateMapper : AbstractDataMapper
    {
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            var config = Configuration as SitecoreDelegateConfiguration;
            var context = mappingContext as SitecoreDataMappingContext;
            if (config == null)
            {
                throw new ArgumentException("A delegate property configuration was expected");
            }

            if (context == null)
            {
                throw new ArgumentException("A sitecore data mapping context was expected");
            }

            if (config.MapToCmsAction == null)
            {
                return;
            }

            config.MapToCmsAction(context);
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var config = Configuration as SitecoreDelegateConfiguration;
            var context = mappingContext as SitecoreDataMappingContext;
            if (config == null)
            {
                throw new ArgumentException("A delegate property configuration was expected");
            }

            if (context == null)
            {
                throw new ArgumentException("A sitecore data mapping context was expected");
            }

            return config.MapToPropertyAction == null
                ? null
                : config.MapToPropertyAction(context);
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreDelegateConfiguration;
        }
    }
}
