using System;
using System.Linq.Expressions;
using System.Web.UI;
using Glass.Mapper.Sc.RenderField;
using Sitecore.Data.Items;
using Sitecore.Web.UI;

namespace Glass.Mapper.Sc.Web.Ui
{
    public class AbstractGlassUserControl : UserControl
    {

        public AbstractGlassUserControl(ISitecoreContext context)
        {
            _glassHtml = new GlassHtml(context);
        }

        public AbstractGlassUserControl() : this(new SitecoreContext())
        {

        }

ISitecoreContext _sitecoreContext;
        GlassHtml _glassHtml;

        public bool IsInEditingMode
        {
            get { return GlassHtml.IsInEditingMode; }
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
        /// The custom data source for the sublayout
        /// </summary>
        public string DataSource
        {
            get
            {
                WebControl parent = Parent as WebControl;
                if (parent == null)
                    return string.Empty;
                return parent.DataSource;
            }
        }
        /// <summary>
        /// Returns either the item specified by the DataSource or the current context item
        /// </summary>
        public Item LayoutItem
        {
            get
            {
                if (DataSource.IsNullOrEmpty())
                    return global::Sitecore.Context.Item;
                else
                    return global::Sitecore.Context.Database.GetItem(DataSource);

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

            if (field == null) throw new NullReferenceException("No field set");

            if (model == null) throw new NullReferenceException("No model set");

            try
            {
                var result = GlassHtml.Editable<T>(model, field);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, string parameters)
        {
            if (field == null) throw new NullReferenceException("No field set");
            if (model == null) throw new NullReferenceException("No model set");

            try
            {
                var result = GlassHtml.Editable<T>(model, field, parameters);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, AbstractParameters parameters)
        {

            if (field == null) throw new NullReferenceException("No field set");

            if (model == null) throw new NullReferenceException("No model set");

            try
            {
                var result = GlassHtml.Editable<T>(model, field, parameters);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)
        {

            if (field == null) throw new NullReferenceException("No field set");

            if (standardOutput == null) throw new NullReferenceException("No standardoutput set");

            if (model == null) throw new NullReferenceException("No model set");

            try
            {
                var result = GlassHtml.Editable<T>(model, field, standardOutput);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, AbstractParameters parameters)
        {

            if (field == null) throw new NullReferenceException("No field set");

            if (standardOutput == null) throw new NullReferenceException("No standardoutput set");

            if (model == null) throw new NullReferenceException("No model set");

            try
            {
                var result = GlassHtml.Editable<T>(model, field, standardOutput, parameters);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }

}
