using System;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sites.Sc
{
    public class Global : Sitecore.Web.Application
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var context = Glass.Mapper.Context.Create(new GlassConfig());
            var attributes = new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sites.Sc");

            context.Load(
                Models.Config.Landing.Load(),
                Models.Config.Misc.Load(),
                Models.Config.Content.Load(),
                attributes
                );
            
            Mapper.Sc.Razor.GlassRazorModuleLoader.Load();
        }
    }
}