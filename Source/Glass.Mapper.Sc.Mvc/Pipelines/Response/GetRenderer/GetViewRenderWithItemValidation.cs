using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.GetRenderer;
using Sitecore.Mvc.Presentation;

namespace Glass.Mapper.Sc.Mvc.Pipelines.Response.GetRenderer
{
    /// <summary>
    /// This class overrides Sitecores default view rendering. Sitecore by default will return a RenderingItem even
    /// it the target item does not exist.
    /// </summary>
    public class GetViewRendererWithItemValidation : GetViewRenderer
    {
        protected override Renderer GetRenderer(Rendering rendering, GetRendererArgs args)
        {
            var viewRenderer = base.GetRenderer(rendering, args) as ViewRenderer;
            if (viewRenderer == null)
                return null;

            // Ignore item check when in page editor
            if (Sitecore.Context.PageMode.IsPageEditor || Sitecore.Context.PageMode.IsPageEditorEditing)
                return viewRenderer;

            // Override renderer to null when there is an unpublished item refererenced by underlying view
            return viewRenderer.Rendering.Item != null && viewRenderer.Rendering.RenderingItem.ValueOrDefault(i => i.InnerItem) != null
                ? viewRenderer
                : null;
        }
    }
}
