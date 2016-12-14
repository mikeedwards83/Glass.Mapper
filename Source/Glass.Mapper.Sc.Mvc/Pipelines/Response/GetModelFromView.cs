using System;
using System.Web.Compilation;
using Glass.Mapper.Sc.IoC;
using Glass.Mapper.Sc.ModelCache;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.GetModel;
using Sitecore.Mvc.Presentation;

namespace Glass.Mapper.Sc.Pipelines.Response
{
    public class GetModelFromView : GetModelProcessor
    {
        private readonly IModelCacheManager modelCacheManager;

        public GetModelFromView()
            : this(new ModelCacheManager(), IoC.SitecoreContextFactory.Default, new ModelFinder())
        {
        }

        public GetModelFromView(IModelCacheManager modelCacheManager, ISitecoreContextFactory sitecoreContextFactory, ModelFinder modelFinder)
        {
            SitecoreContextFactory = sitecoreContextFactory;
            this.modelCacheManager = modelCacheManager;
            ModelFinder = modelFinder;
        }

        protected virtual ISitecoreContextFactory SitecoreContextFactory { get; private set; }

        protected virtual ModelFinder ModelFinder { get; private set; }

        public override void Process(GetModelArgs args)
        {
            if (!IsValidForProcessing(args))
            {
                return;
            }

            string path = GetViewPath(args);

            Sitecore.Diagnostics.Log.Info($"Finding model for {path}", this);

            if (path.StartsWith("/sitecore/shell", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            string cacheKey = modelCacheManager.GetKey(path);
            Type modelType = modelCacheManager.Get(cacheKey);

            if (modelType == typeof(NullModel))
            {
                // The model has been attempted before and is not useful
                return;
            }

            // The model type hasn't been found before or has been cleared.
            if (modelType == null)
            {
                modelType = ModelFinder.GetModel(args.Rendering.RenderingItem.InnerItem["path"], path);

                modelCacheManager.Add(cacheKey, modelType);

                if (modelType == typeof(NullModel))
                {
                    // This is not the type we are looking for
                    return;
                }
            }

            ISitecoreContext scContext = SitecoreContextFactory.GetSitecoreContext();

            Rendering renderingItem = args.Rendering;

            object model = null;

            if (renderingItem.DataSource.HasValue())
            {
                var item = scContext.Database.GetItem(renderingItem.DataSource);
                model = scContext.CreateType(modelType, item, false, false, null);
            }
            else if (renderingItem.RenderingItem.DataSource.HasValue())
            {
                var item = scContext.Database.GetItem(renderingItem.RenderingItem.DataSource);
                model = scContext.CreateType(modelType, item, false, false, null);
            }
            else if (renderingItem.Item != null)
            {   
                /**
             * Issues #82:
             * Check Item before defaulting to the current item.
             */
                model = scContext.CreateType(modelType, renderingItem.Item, false, false, null);
            }
            else
            {
                model = scContext.GetCurrentItem(modelType);
            }

            args.Result = model;
        }

        protected virtual string GetPathFromLayout(
            Database db,
            ID layoutId)
        {
            Item layout = db.GetItem(layoutId);

            return layout != null
                ? layout["path"]
                : null;
        }

        protected virtual string GetViewPath(GetModelArgs args)
        {
            string path = args.Rendering.RenderingItem.InnerItem["path"];

            if (string.IsNullOrWhiteSpace(path) && args.Rendering.RenderingType == "Layout")
            {
                path = GetPathFromLayout(args.PageContext.Database, new ID(args.Rendering.LayoutId));
            }
            return path;
        }

        protected virtual bool IsValidForProcessing(GetModelArgs args)
        {
            if (args.Result != null)
            {
                return false;
            }

            if (!String.IsNullOrEmpty(args.Rendering.RenderingItem.InnerItem["Model"]))
            {
                return false;
            }

            return args.Rendering.RenderingType == "Layout" ||
                   args.Rendering.RenderingType == "View" ||
                   args.Rendering.RenderingType == "r" ||
                   args.Rendering.RenderingType == String.Empty;
        }
    }

    public class NullModel
    {

    }
}
