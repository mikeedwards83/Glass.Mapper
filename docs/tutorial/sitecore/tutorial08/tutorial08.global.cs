using System.Collections.Generic;
using Castle.Windsor;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sites.Sc.App_Start
{

    public static class GlassMapperScCustom
    {
        public static void CastleConfig(IWindsorContainer container)
        {
            var config = new Config();
            container.Install(new SitecoreInstaller(config));
        }

        public static IConfigurationLoader[] GlassLoaders()
        {
            var attributes = new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sites.Sc");

            return new IConfigurationLoader[]
                {
                    attributes, 
                    Models.Config.ContentConfig.Load()
                };
        }
    }
}

}