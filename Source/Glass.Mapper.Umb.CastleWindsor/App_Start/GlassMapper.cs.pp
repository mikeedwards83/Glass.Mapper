using System;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace $rootnamespace$.App_Start
{
    public static class  GlassMapper
    {
        [assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.GlassMapper), "Start")]
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