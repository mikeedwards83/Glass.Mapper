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


using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreAttributeConfigurationLoader
    /// </summary>
    public class SitecoreAttributeConfigurationLoader : AttributeConfigurationLoader<SitecoreTypeConfiguration, SitecorePropertyConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreAttributeConfigurationLoader"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public SitecoreAttributeConfigurationLoader(params string[] assemblies): base(assemblies)
        {

        }

        /// <summary>
        /// Configs the created.
        /// </summary>
        /// <param name="config">The config.</param>
        protected override void ConfigCreated(Mapper.Configuration.AbstractTypeConfiguration config)
        {
            var scConfig = config as SitecoreTypeConfiguration;
            base.ConfigCreated(config);
        }
    }
}




