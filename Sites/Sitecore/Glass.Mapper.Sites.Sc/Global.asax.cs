using System;
using Glass.Mapper.Sc;

namespace Glass.Mapper.Sites.Sc
{
    public class Global : Sitecore.Web.Application
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var context = Glass.Mapper.Context.Create(new GlassConfig());
            context.Load(
                Models.Config.Landing.Load(),
                Models.Config.Misc.Load(),
                Models.Config.Content.Load()
                );
        }
    }
}