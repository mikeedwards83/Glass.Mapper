/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-
using System;
using System.Linq.Expressions;
using System.Web.UI;
using Glass.Mapper.Sc.RenderField;
using Sitecore.Data.Items;
using Sitecore.Web.UI;

namespace Glass.Mapper.Sc.Web.Ui
{
    /// <summary>
    /// Class AbstractGlassUserControl
    /// </summary>
    public class AbstractGlassUserControl : UserControl
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGlassUserControl"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public AbstractGlassUserControl(ISitecoreContext context)
        {
            _glassHtml = new GlassHtml(context);
            _sitecoreContext = context;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGlassUserControl"/> class.
        /// </summary>
        public AbstractGlassUserControl() : this(new SitecoreContext())
        {

        }

        ISitecoreContext _sitecoreContext;
        IGlassHtml _glassHtml;

        /// <summary>
        /// Gets a value indicating whether this instance is in editing mode.
        /// </summary>
        /// <value><c>true</c> if this instance is in editing mode; otherwise, <c>false</c>.</value>
        public bool IsInEditingMode
        {
            get { return Sc.GlassHtml.IsInEditingMode; }
        }

        /// <summary>
        /// Represents the current Sitecore context
        /// </summary>
        /// <value>The sitecore context.</value>
        public ISitecoreContext SitecoreContext
        {
            get { return _sitecoreContext; }
        }

        /// <summary>
        /// Access to rendering helpers
        /// </summary>
        /// <value>The glass HTML.</value>
        protected virtual IGlassHtml GlassHtml
        {
            get { return _glassHtml; }
            set { _glassHtml = value; }
        }

        /// <summary>
        /// The custom data source for the sublayout
        /// </summary>
        /// <value>The data source.</value>
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
        /// <value>The layout item.</value>
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
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="field">The field.</param>
        /// <returns>System.String.</returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field)
        {
           return  UiUtilities.Editable(GlassHtml, model, field);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, string parameters)
        {
            return UiUtilities.Editable(GlassHtml, model, field, parameters);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, AbstractParameters parameters)
        {
            return UiUtilities.Editable(GlassHtml, model, field, parameters);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <returns>System.String.</returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)
        {
            return UiUtilities.Editable(GlassHtml, model, field, standardOutput);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, AbstractParameters parameters)
        {
            return UiUtilities.Editable(GlassHtml, model, field, standardOutput, parameters);
        }
    }

}

