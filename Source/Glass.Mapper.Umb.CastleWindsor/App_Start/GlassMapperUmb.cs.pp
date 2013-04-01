using System;
using Glass.Mapper.Umb.CastleWindsor;
using Glass.Mapper.Umb.Configuration.Attributes;

[assembly: WebActivator.PostApplicationStartMethod(typeof($rootnamespace$.App_Start.GlassMapperUmb), "Start")]

namespace $rootnamespace$.App_Start
{
    public static class  GlassMapperUmb
    {
        public static void Start()
        {
            //create the resolver
            var resolver = DependencyResolver.CreateStandardResolver();

            //create a context
            var context = Glass.Mapper.Context.Create(resolver);

            var attributes = new UmbracoAttributeConfigurationLoader("$assemblyname$");

            context.Load(              
                attributes
                );
        }
    }
}