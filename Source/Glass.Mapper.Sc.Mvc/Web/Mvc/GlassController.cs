using System.Collections.Specialized;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Web.Mvc
{
    public class GlassController : SitecoreController
    {

        public ISitecoreContext SitecoreContext { get; set; }
        public IGlassHtml GlassHtml { get; set; }
         
        public GlassController()
        {
            try
            {
                SitecoreContext = Sc.SitecoreContext.GetFromHttpContext();
                GlassHtml = new GlassHtml(SitecoreContext);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Failed to create SitecoreContext", ex, this);
            }
        }

        protected GlassController(ISitecoreContext sitecoreContext, IGlassHtml glassHtml)
        {
            SitecoreContext = sitecoreContext;
            GlassHtml = glassHtml;
        }

        protected virtual T GetRenderingParameters<T>() where T:class
        {
            return
                GlassHtml.GetRenderingParameters<T>(Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering[Sc.GlassHtml.Parameters]);
        }


        /// <summary>
        /// Returns either the item specified by the DataSource or the current context item
        /// </summary>
        /// <value>The layout item.</value>
        public Item LayoutItem
        {
            get
            {
                return DataSourceItem ?? ContextItem;
            }
        }

        /// <summary>
        /// Returns either the item specified by the current context item
        /// </summary>
        /// <value>The layout item.</value>
        public Item ContextItem
        {
            get { return global::Sitecore.Context.Item; }
        }

        /// <summary>
        /// Returns the item specificed by the data source only. Returns null if no datasource set
        /// </summary>
        public Item DataSourceItem
        {
            get
            {
                if (Sitecore.Mvc.Presentation.RenderingContext.Current == null ||
                    Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering == null ||
                    Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.DataSource.IsNullOrEmpty())
                {
                    return null;
                }
                else
                    return global::Sitecore.Context.Database.GetItem(Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.DataSource);
            }
        }

        /// <summary>
        /// Returns the Context Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetContextItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return SitecoreContext.Cast<T>(ContextItem, isLazy, inferType);
        }

        /// <summary>
        /// Returns the Data Source Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDataSourceItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return SitecoreContext.Cast<T>(DataSourceItem, isLazy, inferType);
        }

        /// <summary>
        /// Returns the Layout Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetLayoutItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return SitecoreContext.Cast<T>(LayoutItem, isLazy, inferType);
        }


        /// <summary>
        /// Returns the data source item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        [Obsolete("User GetDataSourceItem")]
        protected virtual T GetRenderingItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            if (Sitecore.Mvc.Presentation.RenderingContext.Current == null ||
                Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering == null ||
                Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.DataSource.IsNullOrEmpty())
            {
                return default(T);
            }

            return SitecoreContext.GetItem<T>(
                Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.DataSource, isLazy, inferType
                );
        }

        /// <summary>
        /// if the rendering context and data source has been set then returns the data source item, otherwise returns the context item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        [Obsolete("User GetLayoutItem")]
        protected virtual T GetControllerItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {

            if (Sitecore.Mvc.Presentation.RenderingContext.Current == null ||
                Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering == null ||
                Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.DataSource.IsNullOrEmpty())
            {
                return SitecoreContext.GetCurrentItem<T>();
            }
            else
            {
                try
                {
                    return GetRenderingItem<T>(isLazy, inferType);
                }
                catch (InvalidOperationException ex)
                {
                    return SitecoreContext.GetCurrentItem<T>();

                }
            }
        }
    }
}
