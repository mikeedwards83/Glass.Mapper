using Umbraco.Web.Mvc;

namespace Glass.Mapper.Umb.Web.Mvc
{
    public class GlassRenderMvcController : RenderMvcController
    {
        public  IUmbracoContext UmbracoContext { get; private set; }

        public GlassRenderMvcController(IUmbracoContext umbracoContext)
        {
            UmbracoContext = umbracoContext;
        }
        public GlassRenderMvcController() : this(new UmbracoContext())
        {
            
        }
    }
}
