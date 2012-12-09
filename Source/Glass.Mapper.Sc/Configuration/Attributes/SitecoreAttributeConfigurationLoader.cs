using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreAttributeConfigurationLoader : AttributeConfigurationLoader<SitecoreTypeConfiguration, SitecorePropertyConfiguration>
    {
        public SitecoreAttributeConfigurationLoader(params string[] assemblies): base(assemblies)
        {

        }

        protected override void ConfigCreated(Mapper.Configuration.AbstractTypeConfiguration config)
        {
            var scConfig = config as SitecoreTypeConfiguration;

            //find the property configs that will be used to link a sitecore item to 
            //a class
            scConfig.IdConfig = config.Properties.FirstOrDefault(x => x is SitecoreIdConfiguration) as SitecoreIdConfiguration;

            var scInfos = config.Properties.Where(x => x is SitecoreInfoConfiguration).Cast<SitecoreInfoConfiguration>();
            scConfig.LanguageConfig = scInfos.FirstOrDefault(x => x.Type == SitecoreInfoType.Language);
            scConfig.VersionConfig = scInfos.FirstOrDefault(x => x.Type == SitecoreInfoType.Version);

            base.ConfigCreated(config);
        }
    }
}
