using System.Collections.Specialized;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Configuration;
using Sitecore.Mvc.Presentation;

namespace Glass.Mapper.Sc.Web.Mvc
{
    public interface IMvcContext : IRequestContext
    {
        ISitecoreService SitecoreService { get; }
        IGlassHtml GlassHtml { get; set; }

        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.Rendering.DataSource to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be mapped</param>
        /// <returns></returns>
        T GetDataSourceItem<T>(GetKnownOptions options) where T : class;

        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.PageContext.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        T GetPageContextItem<T>(GetKnownOptions options) where T : class;

        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.Rendering.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        T GetRenderingItem<T>(GetKnownOptions options) where T : class;

        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.Rendering.DataSource to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns></returns>
         T GetDataSourceItem<T>() where T : class;
       
        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.PageContext.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns></returns>
         T GetPageContextItem<T>() where T : class;

        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.Rendering.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns></returns>
         T GetRenderingItem<T>() where T : class;

        T GetRenderingParameters<T>() where T : class;

        bool HasDataSource { get; }
        Item DataSourceItem { get; }
        string RenderingParameters { get; }

      

    }

    public class MvcContext : RequestContext, IMvcContext
    {
        public IGlassHtml GlassHtml { get; set; }

        public MvcContext() : this(new SitecoreService(Sitecore.Context.Database))
        {
            
        }
        public MvcContext(ISitecoreService sitecoreService) 
            : this(sitecoreService, ((Sc.IoC.IDependencyResolver)sitecoreService.GlassContext.DependencyResolver).GlassHtmlFactory.GetGlassHtml(sitecoreService))
        {
        }
        public MvcContext(ISitecoreService sitecoreService, IGlassHtml glassHtml) : base(sitecoreService)
        {
            GlassHtml = glassHtml;
        }


        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.Rendering.DataSource to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns></returns>
        public virtual T GetDataSourceItem<T>() where T : class
        {
            return GetDataSourceItem<T>(new GetKnownOptions());
        }

        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.Rendering.DataSource to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be mapped</param>
        /// <returns></returns>
        public virtual T GetDataSourceItem<T>(GetKnownOptions options ) where T : class
        {
            Assert.IsNotNull(options, "options must not be null");

            var item = DataSourceItem;
            options.Item = item;
            return SitecoreService.GetItem<T>(options);
        }

        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.PageContext.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns></returns>
        public virtual T GetPageContextItem<T>() where T : class
        {
            return GetPageContextItem<T>(new GetKnownOptions());
        }
       
        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.PageContext.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        public virtual T GetPageContextItem<T>(GetKnownOptions options) where T : class
        {
            Assert.IsNotNull(options, "options must not be null");

            var item = RenderingContext.CurrentOrNull.PageContext.Item;
            options.Item = item;
            return SitecoreService.GetItem<T>(options);
        }

        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.Rendering.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns></returns>
        public virtual T GetRenderingItem<T>() where T : class
        {
            return GetRenderingItem<T>(new GetKnownOptions());
        }

        /// <summary>
        /// Maps the RenderingContext.CurrentOrNull.Rendering.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        public virtual T GetRenderingItem<T>(GetKnownOptions options) where T : class
        {
            Assert.IsNotNull(options, "options must not be null");

            var item = RenderingContext.CurrentOrNull.Rendering.Item;
            options.Item = item;
            return SitecoreService.GetItem<T>(options);
        }

        /// <summary>
        /// Returns the RenderingContext.CurrentOrNull.Rendering.DataSource as an item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        public virtual Item DataSourceItem
        {
            get
            {
                var dataSource = RenderingContext.CurrentOrNull.Rendering.DataSource;
               

                var item = MvcSettings.ItemLocator.GetItem(dataSource);
                return item;
            }
        }

        public virtual T GetRenderingParameters<T>() where T : class
        {
            return GlassHtml.GetRenderingParameters<T>(RenderingParameters);
        }


        public virtual T GetRenderingParameters<T>(NameValueCollection parameters, ID renderParametersTemplateId) where T : class
        {
            return GlassHtml.GetRenderingParameters<T>(parameters, renderParametersTemplateId);
        }

        public virtual bool HasDataSource
        {
            get { return ContextActive && !RenderingContext.CurrentOrNull.Rendering.DataSource.IsNullOrEmpty(); }
        }
        public virtual  bool ContextActive
        {
            get { return RenderingContext.CurrentOrNull != null && RenderingContext.CurrentOrNull.Rendering != null; }
        }

        public virtual string RenderingParameters
        {
            get
            {
                return ContextActive
                    ? RenderingContext.CurrentOrNull.Rendering[Glass.Mapper.Sc.GlassHtml.Parameters]
                    : string.Empty;
            }
        }

    }


}
