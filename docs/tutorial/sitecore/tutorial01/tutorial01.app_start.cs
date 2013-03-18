using System;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sites.Sc.App_Start
{
    public static class GlassMapper
    {
        [assembly: WebActivator.PreApplicationStartMethod(typeof(Glass.Mapper.Sites.Sc.App_Start.GlassMapper), "Start")]
        public static void Start()
        {
            //create the resolver
            var resolver = DependencyResolver.CreateStandardResolver();

            //create a context
            var context = Glass.Mapper.Context.Create(resolver);

            var attributes = new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sites.Sc");

            context.Load(
                attributes
                );
        }
    }
}