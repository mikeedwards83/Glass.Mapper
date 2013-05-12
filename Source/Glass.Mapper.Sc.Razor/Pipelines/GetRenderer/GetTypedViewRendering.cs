using Sitecore.Mvc.Pipelines.Response.GetRenderer;
using Glass.Mapper.Sc.Razor.Web.Mvc;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Razor.Pipelines.GetRenderer
{
    /// <summary>
    /// Class GetTypedViewRendering
    /// </summary>
    public class GetTypedViewRendering : AbstractGetViewRendering
    {
        /// <summary>
        /// Gets the renderer.
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        /// <param name="args">The args.</param>
        /// <returns>Sitecore.Mvc.Presentation.Renderer.</returns>
        protected override global::Sitecore.Mvc.Presentation.Renderer GetRenderer(
            global::Sitecore.Mvc.Presentation.Rendering rendering, 
            GetRendererArgs args)
        {

            var renderItem = rendering.Item.Database.GetItem(new ID(rendering.RenderingItemPath));
            if (renderItem.TemplateName == "GlassTypedRazor")
            {
                TypedViewRenderer render = new TypedViewRenderer();
                render.Path = renderItem["Name"];
                render.ContextName = renderItem["ContextName"];

                render.DataSource = rendering.DataSource;
                return render;
            }

            return null;
        }
    }
}
