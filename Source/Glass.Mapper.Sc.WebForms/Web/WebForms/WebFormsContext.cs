using System;
using System.Web.UI;
using Glass.Mapper.Sc.Builders;
using Glass.Mapper.Sc.IoC;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web.UI;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.Web.WebForms
{
    public class WebFormsContext : RequestContext, IWebFormsContext
    {
        public IGlassHtml GlassHtml { get; set; }

        public WebFormsContext() : this(new SitecoreService(Sitecore.Context.Database))
        {

        }
        public WebFormsContext(ISitecoreService sitecoreService)
            : this(
                  sitecoreService,
                  ((IDependencyResolver)sitecoreService.GlassContext.DependencyResolver).GlassHtmlFactory.GetGlassHtml(sitecoreService))
        {
        }

        public WebFormsContext(ISitecoreService sitecoreService, IGlassHtml glassHtml)
            : base(sitecoreService)
        {
            GlassHtml = glassHtml;
        }


        /// <summary>
        /// Maps the data source of the control to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        public T GetDataSourceItem<T>(Control control) where T : class
        {
            return GetDataSourceItem<T>(control, new GetKnownOptions());
        }

        /// <summary>
        /// Maps the data source of the control to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be mapped</param>
        /// <returns></returns>
        public T GetDataSourceItem<T>(Control control, GetKnownOptions options) where T : class
        {
            Assert.IsNotNull(options, "options must no be  null");

            var item = GetDataSourceItem(control);
            options.Item = item;
            return SitecoreService.GetItem<T>(options);
        }


        public bool GetHasDataSource(Control control)
        {
            return GetDataSourceItem(control) != null;
        }

        public Item GetDataSourceItem(Control control)
        {
            var path = string.Empty;

            if (control != null)
            {
                WebControl parent = control.Parent as WebControl;
                if (parent != null)
                {
                    path = parent.DataSource;
                }
            }
            if (path.HasValue())
            {
                return SitecoreService.Database.GetItem(path);
            }
            return null;
        }

        public string GetRenderingParameters(Control control)
        {
            if (control == null) return null;

            var sublayout = control as Sublayout;
            if (sublayout != null)
            {
                return sublayout.Parameters;
            }

            return GetRenderingParameters(control.Parent);
        }
    }
}
