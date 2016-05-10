using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Controllers;

namespace Glass.Mapper.Sc.Web.Mvc
{
    public class GlassController<TContext, TDataSource> : GlassController where TContext : class where TDataSource : class
    {
        private TDataSource dataSource;
        private TContext contextItem;

        public GlassController()
        {

        }

        public GlassController(
            ISitecoreContext sitecoreContext, 
            IGlassHtml glassHtml,
            IRenderingContext renderingContextWrapper, 
            HttpContextBase httpContext)
            : base(sitecoreContext, glassHtml, renderingContextWrapper, httpContext)
        {
        }

        public TDataSource DataSource
        {
            get { return dataSource ?? (dataSource = GetDataSourceItem<TDataSource>()); }
        }

        public TContext Context
        {
            get { return contextItem ?? (contextItem = GetContextItem<TContext>()); }
        }
    }

    public class GlassController<T> : GlassController where T : class
    {
        private T dataSourceItem;
        private T contextItem;

        public GlassController()
        {
            
        }

        public GlassController(ISitecoreContext sitecoreContext, IGlassHtml glassHtml,
            IRenderingContext renderingContextWrapper, HttpContextBase httpContext) : base(sitecoreContext, glassHtml, renderingContextWrapper, httpContext)
        {
        }

        public T Layout
        {
            get { return DataSource ?? Context; }
        }

        public T DataSource
        {
            get { return dataSourceItem ?? (dataSourceItem = GetDataSourceItem<T>()); }
        }

        public T Context
        {
            get { return contextItem ?? (contextItem = GetContextItem<T>()); }
        }
    }

    public class GlassController : SitecoreController
    {

        public ISitecoreContext SitecoreContext { get; set; }
        public IGlassHtml GlassHtml { get; set; }
        public IRenderingContext RenderingContextWrapper { get; set; }

        [ExcludeFromCodeCoverage] // Chained constructor - no logic
        public GlassController() 
            : this(GetContextFromHttp())
        {

        }

        [ExcludeFromCodeCoverage] // Chained constructor - no logic
        protected GlassController(ISitecoreContext sitecoreContext) 
            : this(sitecoreContext, sitecoreContext == null? null : new GlassHtml(sitecoreContext), new RenderingContextMvcWrapper(), null)
        {
            
        }

        public GlassController(
            ISitecoreContext sitecoreContext, 
            IGlassHtml glassHtml, 
            IRenderingContext renderingContextWrapper,
            HttpContextBase httpContext)
        {
            SitecoreContext = sitecoreContext;
            GlassHtml = glassHtml;
            RenderingContextWrapper = renderingContextWrapper;
            if (httpContext == null)
            {
                return;
            }

            if (ControllerContext != null)
            {
                ControllerContext.HttpContext = httpContext;
            }
            else
            {
                ControllerContext = new ControllerContext(httpContext, new RouteData(), this);
            }
        }

       


        /// <summary>
        /// Returns either the item specified by the DataSource or the current context item
        /// </summary>
        /// <value>The layout item.</value>
        [ExcludeFromCodeCoverage] // Helper property not to be tested       
        public virtual Item LayoutItem
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
        [ExcludeFromCodeCoverage] // Helper property not to be tested
        public virtual Item ContextItem
        {
            get { return Sitecore.Context.Item; }
        }

        /// <summary>
        /// Returns the item specificed by the data source only. Returns null if no datasource set
        /// </summary>
        [ExcludeFromCodeCoverage] // Helper property not to be tested
        public virtual Item DataSourceItem
        {
            get
            {
                return RenderingContextWrapper.HasDataSource ? Sitecore.Context.Database.GetItem(RenderingContextWrapper.GetDataSource()) : null;
            }
        }

        /// <summary>
        /// Returns the Context Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetContextItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return SitecoreContext.GetCurrentItem<T>(isLazy, inferType);
        }

        /// <summary>
        /// Returns the Data Source Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetDataSourceItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            if (!RenderingContextWrapper.HasDataSource)
            {
                return null;
            }

            string dataSource = RenderingContextWrapper.GetDataSource();
            return !String.IsNullOrEmpty(dataSource) ? SitecoreContext.GetItem<T>(dataSource, isLazy, inferType) : null;
        }

        /// <summary>
        /// Returns the Layout Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetLayoutItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return RenderingContextWrapper.HasDataSource 
                ? GetDataSourceItem<T>(isLazy, inferType) 
                : GetContextItem<T>(isLazy, inferType);
        }


        protected virtual T GetRenderingParameters<T>() where T : class
        {
            string renderingParameters = RenderingContextWrapper.GetRenderingParameters();
            return renderingParameters.HasValue() ? GlassHtml.GetRenderingParameters<T>(renderingParameters) : null;

        }

        /// <summary>
        /// Returns the data source item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        [Obsolete("Use GetDataSourceItem")]
        protected virtual T GetRenderingItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return GetDataSourceItem<T>(isLazy, inferType);
        }

        /// <summary>
        /// if the rendering context and data source has been set then returns the data source item, otherwise returns the context item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        [Obsolete("Use GetLayoutItem")]
        protected virtual T GetControllerItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return GetLayoutItem<T>(isLazy, inferType);
        }

        [ExcludeFromCodeCoverage] // Specific to live implementation
        private static ISitecoreContext GetContextFromHttp()
        {
            try
            {
                return Sc.SitecoreContext.GetFromHttpContext();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Failed to create SitecoreContext", ex, typeof(GlassController));
                return null;
            }
        }
    }
}