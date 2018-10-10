using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Glass.Mapper.Sc.IoC;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Controllers;

namespace Glass.Mapper.Sc.Web.Mvc
{
    [Obsolete("This classs will be removed in future releases")]
    public class GlassController : SitecoreController
    {
        public IMvcContext MvcContext { get; set; }

        public IGlassHtml GlassHtml { get { return MvcContext.GlassHtml; } }

        [Obsolete("User MvcContext Property")]
        public ISitecoreContext SitecoreContext { get; set; }
        //   public IRenderingContext RenderingContextWrapper { get; set; }

        [ExcludeFromCodeCoverage] // Chained constructor - no logic
        public GlassController() 
            : this(new MvcContext(new SitecoreService(Sitecore.Context.Database)),new SitecoreContext())
        {

        }

        public GlassController(
            IMvcContext mvcContext,
            ISitecoreContext sitecoreContext
           )
        {
            MvcContext = mvcContext;
            SitecoreContext = sitecoreContext;
        }

      
     
        /// <summary>
        /// Returns either the item specified by the DataSource or the current context item
        /// </summary>
        /// <value>The layout item.</value>
        [ExcludeFromCodeCoverage] // Helper property not to be tested       
        protected virtual Item LayoutItem
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
        protected virtual Item ContextItem
        {
            get { return Sitecore.Context.Item; }
        }

        /// <summary>
        /// Returns the item specificed by the data source only. Returns null if no datasource set
        /// </summary>
        [ExcludeFromCodeCoverage] // Helper property not to be tested
        protected virtual Item DataSourceItem
        {
            get { return MvcContext.DataSourceItem; }
        }

        /// <summary>
        /// Returns the Context Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual T GetContext<T>(GetKnownOptions options = null) where T : class
        {
            return MvcContext.GetContextItem<T>(options);
        }

        /// <summary>
        /// Returns the Data Source Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual T GetDataSource<T>(GetKnownOptions options = null) where T : class
        {
            if (!MvcContext.HasDataSource)
            {
                return null;
            }
            return MvcContext.GetDataSourceItem<T>(options);
        }

        /// <summary>
        /// Returns the Layout Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual T GetLayout<T>(GetKnownOptions options = null) where T : class
        {
            return MvcContext.HasDataSource 
                ? GetDataSource<T>(options) 
                : GetContext<T>(options);
        }


        protected virtual T GetRenderingParameters<T>() where T : class
        {
            string renderingParameters = MvcContext.RenderingParameters;
            return renderingParameters.HasValue() ? GlassHtml.GetRenderingParameters<T>(renderingParameters) : null;

        }

    }
}