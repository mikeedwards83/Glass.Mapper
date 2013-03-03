using Sitecore.Mvc.Pipelines.Response.GetRenderer;
using Glass.Mapper.Sc.Razor.Web.Mvc;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Razor.Pipelines.GetRenderer
{
    public class GetTypedViewRendering : AbstractGetViewRendering
    {
        protected override global::Sitecore.Mvc.Presentation.Renderer GetRenderer(
            global::Sitecore.Mvc.Presentation.Rendering rendering, 
            GetRendererArgs args)
        {

            var renderItem = rendering.Item.Database.GetItem(new ID(rendering.RenderingItemPath));
            if (renderItem.TemplateName == "GlassTypedRazor")
            {
                TypedViewRenderer render = new TypedViewRenderer();
                render.Path = renderItem["Name"];
                render.Type = renderItem["Type"];
                render.Assembly = renderItem["assembly"];
                render.ContextName = renderItem["ContextName"];

                render.DataSource = rendering.DataSource;
                return render;
            }

            return null;
        }
    }
}
