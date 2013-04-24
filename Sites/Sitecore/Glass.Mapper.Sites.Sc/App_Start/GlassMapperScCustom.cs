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
            config.UseWindsorContructor = true;

            container.Install(new SitecoreInstaller(config));
            container.Install(new ServiceInstaller());

        }

        public static IConfigurationLoader[] GlassLoaders()
        {
            var attributes = new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sites.Sc");

            return new IConfigurationLoader[] {attributes, Models.Config.ContentConfig.Load()};
        }

        public static void PostLoad()
        {
            //Comment this code in to activate CodeFist
            /* CODE FIRST START
            var dbs = Sitecore.Configuration.Factory.GetDatabases();
            foreach (var db in dbs)
            {
                var provider = db.GetDataProviders().FirstOrDefault(x => x is GlassDataProvider) as GlassDataProvider;
                if (provider != null)
                {
                    using (new SecurityDisabler())
                    {
                        provider.Initialise(db);
                    }
                }
            }
             * CODE FIRST END
             */
        }
    }
}

  
