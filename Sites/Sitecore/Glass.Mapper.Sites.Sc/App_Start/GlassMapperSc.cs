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
            //create the resolver
            var resolver = DependencyResolver.CreateStandardResolver();

            //install the custom services
            GlassMapperScCustom.CastleConfig(resolver.Container);

            //create a context
            var context = Glass.Mapper.Context.Create(resolver);
            context.Load(
                GlassMapperScCustom.GlassLoaders()
                );

            GlassMapperScCustom.PostLoad();
        }
    }
}