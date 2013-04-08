using System;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Glass.Mapper.Sites.Sc.App_Start.GlassMapperSc), "Start")]

namespace Glass.Mapper.Sites.Sc.App_Start
{
    public static class GlassMapperSc
    {
        public static void Start()
        {
            var config = GlassMapperScCustom.GetConfig();

            //create the resolver
            var resolver = DependencyResolver.CreateStandardResolver(config);

            //install the custom services
            var container = (resolver as DependencyResolver).Container;
            GlassMapperScCustom.CastleConfig(container, config);

            //create a context
            var context = Glass.Mapper.Context.Create(resolver);
            context.Load(
                GlassMapperScCustom.GlassLoaders()
                );
        }
    }
}