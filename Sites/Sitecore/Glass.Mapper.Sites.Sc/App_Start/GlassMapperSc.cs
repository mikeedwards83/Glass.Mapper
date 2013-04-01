using System;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Glass.Mapper.Sites.Sc.App_Start.GlassMapperSc), "Start")]

namespace Glass.Mapper.Sites.Sc.App_Start
{
    public static class  GlassMapperSc
    {
        public static void Start()
        {
            var config = new Config();
            config.UseWindsorContructor = true;

            //create the resolver
            var resolver = DependencyResolver.CreateStandardResolver(config);

            //install the custom services
            var container = (resolver as DependencyResolver).Container;
            container.Install(new ServiceInstaller());

            //create a context
            var context = Glass.Mapper.Context.Create(resolver);

            var attributes = new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sites.Sc");

            context.Load(
                attributes,
                Models.Config.ContentConfig.Load()
                );
        }

    }
}