using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.UI;
using Glass.Mapper.Sc.RenderField;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Web.Ui
{
    public class AbstractGlassPage : Page
    {
        ISitecoreContext _sitecoreContext;
        GlassHtml _glassHtml;

         public AbstractGlassPage(ISitecoreContext context)
        {
            _glassHtml = new GlassHtml(context);
            _sitecoreContext = context;
        }

         public AbstractGlassPage()
             : this(new SitecoreContext())
        {

        }

        /// <summary>
        /// Represents the current Sitecore context
        /// </summary>
        public ISitecoreContext SitecoreContext
        {
            get { return _sitecoreContext; }
            
        }

        /// <summary>
        /// Access to rendering helpers
        /// </summary>
        protected virtual GlassHtml GlassHtml
        {
            get { return _glassHtml; }
        }

        /// <summary>
        /// Returns either the item specified by the DataSource or the current context item
        /// </summary>
        public Item LayoutItem
        {
            get
            {
                return global::Sitecore.Context.Item;
            }
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field)
        {
            return UiUtilities.Editable(GlassHtml, model, field);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, string parameters)
        {
            return UiUtilities.Editable(GlassHtml, model, field, parameters);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, AbstractParameters parameters)
        {
            return UiUtilities.Editable(GlassHtml, model, field, parameters);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)
        {
            return UiUtilities.Editable(GlassHtml, model, field, standardOutput);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, AbstractParameters parameters)
        {
            return UiUtilities.Editable(GlassHtml, model, field, standardOutput, parameters);
        }
    }
}
