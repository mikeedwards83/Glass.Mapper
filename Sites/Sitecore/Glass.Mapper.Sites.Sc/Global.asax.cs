using System;
using Glass.Mapper.Sc.Integration;
using Glass.Mapper.Sites.Sc.Tutorial1.Config;
using Glass.Mapper.Sites.Sc.Tutorial2.Config;

namespace Glass.Mapper.Sites.Sc
{
    public class Global : Sitecore.Web.Application
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var context = Glass.Mapper.Context.Create(new GlassConfig());
            context.Load(Tutorial1Config.Config());
            context.Load(Tutorial2Config.Config());
        }
    }
}