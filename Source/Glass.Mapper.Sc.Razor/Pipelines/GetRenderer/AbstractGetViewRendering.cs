using Sitecore.Mvc.Pipelines.Response.GetRenderer;

namespace Glass.Mapper.Sc.Razor.Pipelines.GetRenderer
{
    public abstract class AbstractGetViewRendering : GetRendererProcessor
    {

        public override void Process(GetRendererArgs args)
        {
            if (args.Result != null)
                return;
            args.Result = this.GetRenderer(args.Rendering, args);
        }

        protected abstract global::Sitecore.Mvc.Presentation.Renderer GetRenderer(global::Sitecore.Mvc.Presentation.Rendering rendering, GetRendererArgs args);

    }
}
