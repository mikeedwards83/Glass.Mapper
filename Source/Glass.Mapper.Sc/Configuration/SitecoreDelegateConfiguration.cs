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

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// The sitecore delegate configuration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreDelegateConfiguration : AbstractPropertyConfiguration
    {
        /// <summary>
        /// Gets or sets the action to take place when mapping to the cms
        /// </summary>
        public Action<SitecoreDataMappingContext> MapToCmsAction { get; set; }

        /// <summary>
        /// Gets or sets the action to take place when mapping to the objects property
        /// </summary>
        public Func<SitecoreDataMappingContext, object> MapToPropertyAction { get; set; }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreDelegateConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreDelegateConfiguration;

            config.MapToCmsAction = MapToCmsAction;
            config.MapToPropertyAction = MapToPropertyAction;

            base.Copy(copy);
        }
    }
}
