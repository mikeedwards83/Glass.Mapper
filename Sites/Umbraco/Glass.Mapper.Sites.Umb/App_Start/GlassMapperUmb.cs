using System;
using Glass.Mapper.Sites.Umb.Models.Config;
using Glass.Mapper.Umb.CastleWindsor;
using Glass.Mapper.Umb.Configuration.Attributes;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Glass.Mapper.Sites.Umb.App_Start.GlassMapperUmb), "Start")]

namespace Glass.Mapper.Sites.Umb.App_Start
{
    public static class GlassMapperUmb
    {
        public static void Start()
        {
            var config = GlassMapperUmbCustom.GetConfig();

            //create the resolver
            var resolver = DependencyResolver.CreateStandardResolver(config);

            //install the custom services
            var container = (resolver as DependencyResolver).Container;
            GlassMapperUmbCustom.CastleConfig(container, config);

            //create a context
            var context = Glass.Mapper.Context.Create(resolver);
            context.Load(
                GlassMapperUmbCustom.GlassLoaders()
                );
        }
    }
}