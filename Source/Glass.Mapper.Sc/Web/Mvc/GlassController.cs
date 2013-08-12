using System.Collections.Specialized;
using Sitecore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Web.Mvc
{
    public class GlassController : SitecoreController
    {
        public ISitecoreContext SitecoreContext { get; set; }
        public IGlassHtml GlassHtml { get; set; }
         
        public GlassController()
        {
            try
            {
                SitecoreContext =
                    new SitecoreContext(Sitecore.Mvc.Presentation.RenderingContext.Current.ContextItem.Database);
                GlassHtml = new GlassHtml(SitecoreContext);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Failed to create SitecoreContext", ex, this);
            }
        }

        public T GetRenderingParameters<T>() where T:class
        {
            var parameters = new NameValueCollection();
            foreach (var pair in Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering.Parameters)
            {
                parameters[pair.Key] = pair.Value;
            }
            return
                GlassHtml.GetRenderingParameters<T>(parameters);
        } 
    }
}
