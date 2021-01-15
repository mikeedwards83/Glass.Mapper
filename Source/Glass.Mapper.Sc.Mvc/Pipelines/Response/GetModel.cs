

using System;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.IoC;
using Glass.Mapper.Sc.Web.Mvc;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Configuration;
using Sitecore.Mvc.Data;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.GetModel;
using Sitecore.Mvc.Presentation;

namespace Glass.Mapper.Sc.Pipelines.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class GetModel : GetModelProcessor
    {
        /// <summary>
        /// The model type field
        /// </summary>
        public const string ModelTypeField = "Model Type";

        private readonly Type renderingModelType = typeof (IRenderingModel);

        /// <summary>
        /// The model field
        /// </summary>
        public const string ModelField = "Model";

        /// <summary>
        /// Processes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Process(GetModelArgs args)
        {
            if (args.Result != null)
            {
                return;
            }

            Rendering rendering = args.Rendering;
            if (rendering.RenderingType == "Layout")
            {
                args.Result = GetFromItem(rendering, args);
                if (args.Result == null)
                {
                    args.Result = GetFromLayout(rendering, args);
                }
            }
            if (args.Result == null)
            {
                args.Result = GetFromPropertyValue(rendering, args);
            }
            if (args.Result == null)
            {
                args.Result = GetFromField(rendering, args);
            }
            if (args.Result != null)
            {
                args.AbortPipeline();
            }
        }

        /// <summary>
        /// Gets from field.
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        protected virtual object GetFromField(Rendering rendering, GetModelArgs args)
        {
            Item obj = rendering.RenderingItem.ValueOrDefault(i => i.InnerItem);
            if (obj == null)
                return null;
            return rendering.Item == null 
                ? null 
                : GetObject(obj[ModelField], rendering.Item.Database, rendering);
        }

        /// <summary>
        /// Gets from property value.
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        protected virtual object GetFromPropertyValue(Rendering rendering, GetModelArgs args)
        {
            string model = rendering.Properties[ModelField];
            if (model.IsWhiteSpaceOrNull())
                return null;
            else
                return GetObject(model, rendering.Item.Database, rendering);
        }

        /// <summary>
        /// Gets from layout.
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        protected virtual object GetFromLayout(Rendering rendering, GetModelArgs args)
        {
            string pathOrId = rendering.Properties["LayoutId"];
            if (pathOrId.IsWhiteSpaceOrNull())
                return null;
            string model = MvcSettings.GetRegisteredObject<ItemLocator>().GetItem(pathOrId).ValueOrDefault(i => i["Model"]);
            if (model.IsWhiteSpaceOrNull())
                return null;
            else
                return GetObject(model, rendering.Item.Database, rendering);
        }

        /// <summary>
        /// Gets from item.
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        protected virtual object GetFromItem(Rendering rendering, GetModelArgs args)
        {
            string model = rendering.Item.ValueOrDefault(i => i["MvcLayoutModel"]);
            if (model.IsWhiteSpaceOrNull())
                return null;

            return GetObject(model, rendering.Item.Database, rendering);
        }


        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        /// <exception cref="Glass.Mapper.MapperException">Failed to find context {0}.Formatted(ContextName)</exception>
        public virtual object GetObject(string model, Database db, Rendering renderingItem)
        {

            if (model.IsNullOrEmpty())
                return null;

            //must be a path to a Model item
            if (model.StartsWith("/sitecore"))
            {
                var target = db.GetItem(model);
                if (target == null)
                    return null;

                string newModel = target[ModelTypeField];
                return GetObject(newModel, db, renderingItem);
            }
            //if guid must be that to Model item
            Guid targetId;
            if (Guid.TryParse(model, out targetId))
            {
                var target = db.GetItem(new ID(targetId));
                if (target == null)
                    return null;

                string newModel = target[ModelTypeField];
                return GetObject(newModel, db, renderingItem);
            }


            var type = Type.GetType(model, false);

            if (type == null || renderingModelType.IsAssignableFrom(type))
                return null;

            IMvcContext mvcContext = new MvcContext(new SitecoreService(Sitecore.Context.Database));

            //this is really aggressive
            if (!mvcContext.SitecoreService.GlassContext.TypeConfigurations.ContainsKey(type))
            {
                //if the config is null then it is probably an ondemand mapping so we have to load the ondemand part

                IConfigurationLoader loader =
                    new OnDemandLoader<SitecoreTypeConfiguration>(type);
                mvcContext.SitecoreService.GlassContext.Load(loader);

            }

            if (renderingItem.DataSource.HasValue())
            {
                // Switch to handle AB test datasources which contain Sitecore reference
                // syntax 
                //TODO: repeated code - refactor to a helper and use in IMvcContext 
                var item = renderingItem.DataSource.StartsWith("sitecore://", StringComparison.Ordinal) ?
                    Sitecore.Context.Database.GetItem(DataUri.Parse(renderingItem.DataSource)) :
                    MvcSettings.ItemLocator.GetItem(renderingItem.DataSource);

                var getOptions = new GetItemByItemOptions()
                {
                    Item = item,
                    Type = type
                };

                return mvcContext.SitecoreService.GetItem(getOptions);
            }

            if (renderingItem.RenderingItem.DataSource.HasValue())
            {
                var getOptions = new GetItemByPathOptions()
                {
                    Path = renderingItem.RenderingItem.DataSource,
                    Type = type
                };
                return mvcContext.SitecoreService.GetItem(getOptions);
            }

            /**
             * Issues #82:
             * Check Item before defaulting to the current item.
             */
            if (renderingItem.Item != null)
            {
                var getOptions = new GetItemByItemOptions()
                {
                    Item = renderingItem.Item,
                    Type = type
                };
                return mvcContext.SitecoreService.GetItem(getOptions);
            }
            else
            {

                //TODO? shoudl we use the GetCurrentitem
                var getOptions = new GetItemByItemOptions()
                {
                    Item = mvcContext.ContextItem,
                    Type = type
                };
                return mvcContext.SitecoreService.GetItem(getOptions);
            }
        }
    }
}

