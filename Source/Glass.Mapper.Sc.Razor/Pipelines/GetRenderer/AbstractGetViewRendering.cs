using Sitecore.Mvc.Pipelines.Response.GetRenderer;

namespace Glass.Mapper.Sc.Razor.Pipelines.GetRenderer
{
    /// <summary>
    /// Class AbstractGetViewRendering
    /// </summary>
    public abstract class AbstractGetViewRendering : GetRendererProcessor
    {

        /// <summary>
        /// Processes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Process(GetRendererArgs args)
        {
            if (args.Result != null)
                return;
            args.Result = this.GetRenderer(args.Rendering, args);
        }

        /// <summary>
        /// Gets the renderer.
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        /// <param name="args">The args.</param>
        /// <returns>Sitecore.Mvc.Presentation.Renderer.</returns>
        protected abstract global::Sitecore.Mvc.Presentation.Renderer GetRenderer(global::Sitecore.Mvc.Presentation.Rendering rendering, GetRendererArgs args);

    }
}
