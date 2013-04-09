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
            //create the resolver
            var resolver = DependencyResolver.CreateStandardResolver();

            //install the custom services
            GlassMapperUmbCustom.CastleConfig(resolver.Container);

            //create a context
            var context = Context.Create(resolver);
            context.Load(
                GlassMapperUmbCustom.GlassLoaders()
                );
        }
    }
}