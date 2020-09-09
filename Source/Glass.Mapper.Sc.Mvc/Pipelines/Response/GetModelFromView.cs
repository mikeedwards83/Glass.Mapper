using System;
using System.Diagnostics;
using Glass.Mapper.Sc.ModelCache;
using Glass.Mapper.Sc.Web.Mvc;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Pipelines.Response.GetModel;
using Sitecore.Mvc.Presentation;

namespace Glass.Mapper.Sc.Pipelines.Response
{
    public class GetModelFromView : GetModelProcessor
    {
        private readonly IModelCacheManager modelCacheManager;
        public static IViewTypeResolver ViewTypeResolver { get; set; }



        static GetModelFromView()
        {
            ViewTypeResolver = new RegexViewTypeResolver();
        }
        public GetModelFromView()
            : this(new ModelCacheManager())
        {
        }

        public GetModelFromView(IModelCacheManager modelCacheManager)
        {
            this.modelCacheManager = modelCacheManager;
        }


        public override void Process(GetModelArgs args)
        {
            var key = "GetModelFromView";
            var watch = Stopwatch.StartNew();

            try
            {
                if (!IsValidForProcessing(args))
                {
                    return;
                }

                string path = GetViewPath(args);

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
                    modelType = GetModel(args, path);

                    if (typeof(RenderingModel).IsAssignableFrom(modelType))
                    {
                        //convert RenderingModel to NullModel so it is ignored in the future.
                        modelType = typeof(NullModel);
                    }

                    modelCacheManager.Add(cacheKey, modelType);

                    if (modelType == typeof(NullModel))
                    {
                        // This is not the type we are looking for
                        return;
                    }
                }

                args.Result = GetDataSourceItem(args, modelType);
                args.AbortPipeline();
            }
            finally
            {
                Sitecore.Diagnostics.Log.Debug("GetModelFromView {0} {1}".Formatted(watch.ElapsedMilliseconds, args.Rendering.RenderingItem.ID), this);
            }
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

        protected virtual Type GetModel(GetModelArgs args, string path)
        {
            return ViewTypeResolver.GetType(path);
        }




        protected virtual bool IsValidForProcessing(GetModelArgs args)
        {
            if (args.Result != null)
            {
                return false;
            }
            if (Sitecore.Context.Site != null && Sitecore.Context.Site.Name.ToLowerInvariant() == "shell")
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

        protected virtual object GetDataSourceItem(GetModelArgs args, Type modelType)
        {
            IMvcContext mvcContext = new MvcContext(new SitecoreService(Sitecore.Context.Database));
            Rendering renderingItem = args.Rendering;

            if (renderingItem.DataSource.HasValue())
            {
                var getOptions = new GetItemByPathOptions();
                getOptions.Type = modelType;
                getOptions.Path = renderingItem.DataSource;
                return mvcContext.SitecoreService.GetItem(getOptions);
            }

            if (renderingItem.RenderingItem.DataSource.HasValue())
            {
                var getOptions = new GetItemByPathOptions();
                getOptions.Type = modelType;
                getOptions.Path = renderingItem.RenderingItem.DataSource;

                return mvcContext.SitecoreService.GetItem(getOptions);
            }

            if (renderingItem.Item != null)
            {
                /**
         * Issues #82:
         * Check Item before defaulting to the current item.
         */
                var getOptions = new GetItemByItemOptions();
                getOptions.Type = modelType;
                getOptions.Item = renderingItem.Item;

                return mvcContext.SitecoreService.GetItem(getOptions);

            }
            else
            {
                var getOptions = new GetItemByItemOptions();
                getOptions.Type = modelType;
                getOptions.Item = mvcContext.ContextItem;
                return mvcContext.SitecoreService.GetItem(getOptions);
            }
        }





    }

    public class NullModel
    {

    }
}
