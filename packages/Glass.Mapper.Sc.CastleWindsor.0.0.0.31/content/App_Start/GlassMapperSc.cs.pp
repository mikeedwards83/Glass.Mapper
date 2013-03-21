using System;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.App_Start.GlassMapperSc), "Start")]

namespace $rootnamespace$.App_Start
{
    public static class  GlassMapperSc
    {
        public static void Start()
        {
            //create the resolver
            var resolver = DependencyResolver.CreateStandardResolver();

            //create a context
            var context = Glass.Mapper.Context.Create(resolver);

            var attributes = new SitecoreAttributeConfigurationLoader("$assemblyname$");

            context.Load(              
                attributes
                );
        }
    }
}