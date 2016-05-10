using System;
using System.Web.Compilation;
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
            : this(new ModelCacheManager())
        {
        }

        public GetModelFromView(IModelCacheManager modelCacheManager)
        {
            this.modelCacheManager = modelCacheManager;
        }

        /// <summary>
        /// Gets or sets the name of the context.
        /// </summary>
        /// <value>
        /// The name of the context.
        /// </value>
        public virtual string ContextName
        {
            get
            {
                var context = AbstractSitecoreContext.GetContextFromSite();
                Sitecore.Diagnostics.Log.Debug("using context " + context, this);
                return context;
            }
        }


        public override void Process(GetModelArgs args)
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

                modelCacheManager.Add(cacheKey, modelType);

                if (modelType == typeof(NullModel))
                {
                    // This is not the type we are looking for
                    return;
                }
            }

            ISitecoreContext scContext = SitecoreContext.GetFromHttpContext(ContextName);

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

        private string GetPathFromLayout(
            Database db,
            ID layoutId)
        {
            Item layout = db.GetItem(layoutId);

            return layout != null
                ? layout["path"]
                : null;
        }

        private string GetViewPath(GetModelArgs args)
        {
            string path = args.Rendering.RenderingItem.InnerItem["path"];

            if (string.IsNullOrWhiteSpace(path) && args.Rendering.RenderingType == "Layout")
            {
                path = GetPathFromLayout(args.PageContext.Database, new ID(args.Rendering.LayoutId));
            }
            return path;
        }

        private Type GetModel(GetModelArgs args, string path)
        {
            Type compiledViewType = BuildManager.GetCompiledType(path);
            Type baseType = compiledViewType.BaseType;

            if (baseType == null || !baseType.IsGenericType)
            {
                Sitecore.Diagnostics.Log.Error(string.Format(
                    "View {0} compiled type {1} base type {2} does not have a single generic argument.",
                    args.Rendering.RenderingItem.InnerItem["path"],
                    compiledViewType,
                    baseType), this);
                return typeof(NullModel);
            }

            Type proposedType = baseType.GetGenericArguments()[0];
            return proposedType == typeof(object)
                ? typeof(NullModel)
                : proposedType;
        }

        private static bool IsValidForProcessing(GetModelArgs args)
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
