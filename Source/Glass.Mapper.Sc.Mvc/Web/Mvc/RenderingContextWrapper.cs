using Sitecore.Mvc.Presentation;

namespace Glass.Mapper.Sc.Web.Mvc
{
    public class RenderingContextWrapper : IRenderingContextWrapper
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
            return RenderingContext.CurrentOrNull.Rendering[GlassHtml.Parameters];
        }

        public string GetDataSource()
        {
            return RenderingContext.CurrentOrNull.Rendering.DataSource;
        }
    }
}
