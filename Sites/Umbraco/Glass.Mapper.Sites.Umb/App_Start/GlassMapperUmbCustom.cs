using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sites.Umb.Models.Config;
using Glass.Mapper.Umb.CastleWindsor;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace Glass.Mapper.Sites.Umb.App_Start
{
    public static class GlassMapperUmbCustom
    {
        public static void CastleConfig(IWindsorContainer container)
        {
            var config = new Config();
            container.Install(new UmbracoInstaller(config));
        }

        public static IConfigurationLoader[] GlassLoaders()
        {
            var attributes = new UmbracoAttributeConfigurationLoader("Glass.Mapper.Sites.Umb");

            return new IConfigurationLoader[] { attributes, ContentConfig.Load() };
        }
    }
}