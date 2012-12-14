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
