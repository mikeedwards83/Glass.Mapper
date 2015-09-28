using Sitecore.Mvc.Presentation;

namespace Glass.Mapper.Sc.Web.Mvc
{
    public class RenderingContextMvcWrapper : IRenderingContext
    {
        public bool ContextActive
        {
            get { return RenderingContext.CurrentOrNull != null && RenderingContext.CurrentOrNull.Rendering != null; }
        }

        public bool HasDataSource
        {
            get { return ContextActive && !RenderingContext.CurrentOrNull.Rendering.DataSource.IsNullOrEmpty(); }
        }

        public string GetRenderingParameters()
        {
            return ContextActive ? RenderingContext.CurrentOrNull.Rendering[GlassHtml.Parameters] : string.Empty;
        }

        public string GetDataSource()
        {
            return RenderingContext.CurrentOrNull.Rendering.DataSource;
        }
    }
}
