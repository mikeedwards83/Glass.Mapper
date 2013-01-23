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

using System.Linq;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    public class UmbracoAttributeConfigurationLoader : AttributeConfigurationLoader<UmbracoTypeConfiguration, UmbracoPropertyConfiguration>
    {
        public UmbracoAttributeConfigurationLoader(params string[] assemblies)
            : base(assemblies)
        {

        }

        protected override void ConfigCreated(AbstractTypeConfiguration config)
        {
            var umbConfig = config as UmbracoTypeConfiguration;

            //find the property configs that will be used to link a umbraco item to a class
            umbConfig.IdConfig = config.Properties.FirstOrDefault(x => x is UmbracoIdConfiguration) as UmbracoIdConfiguration;

            //var scInfos = config.Properties.Where(x => x is SitecoreInfoConfiguration).Cast<SitecoreInfoConfiguration>();
            //umbConfig.LanguageConfig = scInfos.FirstOrDefault(x => x.Type == SitecoreInfoType.Language);
            //umbConfig.VersionConfig = scInfos.FirstOrDefault(x => x.Type == SitecoreInfoType.Version);

            base.ConfigCreated(config);
        }
    }
}



