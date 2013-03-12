using System;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sites.Sc
{
    public class Global : Sitecore.Web.Application
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //create the resolver
            var resolver = DependencyResolver.CreateStandardResolver();

            //create a context
            var context = Glass.Mapper.Context.Create(resolver);

            var attributes = new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sites.Sc");

            context.Load(
                Models.Config.Landing.Load(),
                Models.Config.Misc.Load(),
                Models.Config.Content.Load(),
                attributes
                );
            
            Mapper.Sc.Razor.GlassRazorModuleLoader.Load(resolver);
        }
    }
}