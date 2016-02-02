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
using System.IO;
using System.Linq.Expressions;
using System.Web.UI;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Web.Ui
{
    /// <summary>
    /// Class AbstractGlassUserControl
    /// </summary>
    public abstract class AbstractGlassUserControl : UserControl
    {
        private TextWriter _writer;
        private IRenderingContext _renderingContext;
        private ISitecoreContext _sitecoreContext;
        private IGlassHtml _glassHtml;

        protected AbstractGlassUserControl(ISitecoreContext context, IGlassHtml glassHtml, IRenderingContext renderingContext)
        {
            _renderingContext = renderingContext;
            _glassHtml = glassHtml;
            _sitecoreContext = context;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGlassUserControl"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="glassHtml">The glass html</param>
        protected AbstractGlassUserControl(ISitecoreContext context, IGlassHtml glassHtml) : this(context, glassHtml, null)
        {


        }

        protected AbstractGlassUserControl(ISitecoreContext context) : this(context, new GlassHtml(context))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGlassUserControl"/> class.
        /// </summary>
        protected AbstractGlassUserControl()
        {
        }

        public virtual IRenderingContext RenderingContext
        {
            get { return _renderingContext ?? (_renderingContext = new RenderingContextUserControlWrapper(this)); }
            set { _renderingContext = value; }
        }

        protected TextWriter Output
        {
            get { return _writer ?? this.Response.Output; }
        }

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
        public virtual ISitecoreContext SitecoreContext
        {
            get { return _sitecoreContext ?? ( _sitecoreContext = Sc.SitecoreContext.GetFromHttpContext()); }
            set { _sitecoreContext = value; }
        }

        /// <summary>
        /// Access to rendering helpers
        /// </summary>
        /// <value>The glass HTML.</value>
        public virtual IGlassHtml GlassHtml
        {
            get { return _glassHtml ?? (_glassHtml = new GlassHtml(SitecoreContext)); }
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
                return RenderingContext.GetDataSource();
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
                return DataSourceItem ?? ContextItem;
            }
        }


        /// <summary>
        /// Returns either the item specified by the current context item
        /// </summary>
        /// <value>The layout item.</value>
        public Item ContextItem
        {
            get { return Sitecore.Context.Item; }
        }

        /// <summary>
        /// Returns the item specificed by the data source only. Returns null if no datasource set
        /// </summary>
        public Item DataSourceItem
        {
            get {
                return DataSource.IsNullOrEmpty() 
                    ? null 
                    : Sitecore.Context.Database.GetItem(DataSource);
            }
        }

        /// <summary>
        /// Returns the Context Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetContextItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return SitecoreContext.GetCurrentItem<T>(isLazy, inferType);
        }

        /// <summary>
        /// Returns the Data Source Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDataSourceItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            string dataSource = RenderingContext.GetDataSource();
            return !String.IsNullOrEmpty(dataSource) 
                ? SitecoreContext.GetItem<T>(dataSource, isLazy, inferType) 
                : null;
        }

        /// <summary>
        /// Returns the DataSource item or the Context Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetLayoutItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            string dataSource = RenderingContext.GetDataSource();
            return !String.IsNullOrEmpty(dataSource)
                ? GetDataSourceItem<T>(isLazy, inferType)
                : GetContextItem<T>(isLazy, inferType);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public string Editable<T>(T model, Expression<Func<T, object>> field, object parameters = null)
        {
            return GlassHtml.Editable(model, field, parameters);
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
        public string Editable<T>(T model, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, object parameters = null)
        {
            return GlassHtml.Editable(model, field, standardOutput, parameters);
        }


        /// <summary>
        /// Renders an image allowing simple page editor support
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="model">The model that contains the image field</param>
        /// <param name="field">A lambda expression to the image field, should be of type Glass.Mapper.Sc.Fields.Image</param>
        /// <param name="parameters">Image parameters, e.g. width, height</param>
        /// <param name="isEditable">Indicates if the field should be editable</param>
        /// <param name="outputHeightWidth">Indicates if the height and width attributes should be outputted when rendering the image</param>
        /// <returns></returns>
        public virtual string RenderImage<T>(T model,
                                             Expression<Func<T, object>> field,
                                             object parameters = null,
                                             bool isEditable = false,
                                             bool outputHeightWidth = true)

        {
            return GlassHtml.RenderImage(model, field, parameters, isEditable, outputHeightWidth);
        }


        /// <summary>
        /// Render HTML for a link with contents
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="model">The model</param>
        /// <param name="field">The link field to user</param>
        /// <param name="attributes">Any additional link attributes</param>
        /// <param name="isEditable">Make the link editable</param>
        /// <returns></returns>
        public virtual RenderingResult BeginRenderLink<T>(T model, Expression<Func<T, object>> field, object attributes = null, bool isEditable = false)
        {
            return GlassHtml.BeginRenderLink(model, field, this.Output, attributes, isEditable);

        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="model">The model</param>
        /// <param name="field">The link field to user</param>
        /// <param name="attributes">Any additional link attributes</param>
        /// <param name="isEditable">Make the link editable</param>
        /// <param name="contents">Content to override the default decription or item name</param>
        /// <returns></returns>
        public virtual string RenderLink<T>(T model, Expression<Func<T, object>> field, object attributes = null,  bool isEditable = false, string contents=null)
        {
            return GlassHtml.RenderLink(model, field, attributes, isEditable, contents);
        }

        /// <summary>
        /// Returns an Sitecore Edit Frame
        /// </summary>
        /// <returns>
        /// GlassEditFrame.
        /// </returns>
        public GlassEditFrame BeginEditFrame<T>(T model, string title = null, params Expression<Func<T, object>>[] fields) where T : class
        {
            return GlassHtml.EditFrame(model, title, this.Output, fields);
        }


        public override void RenderControl(HtmlTextWriter writer)
        {
            this._writer = writer;
            base.RenderControl(writer);
        }

        public virtual string RenderingParameters
        {
            get { return RenderingContext.GetRenderingParameters(); }
        }

        public virtual TParam GetRenderingParameters<TParam>() where TParam : class
        {
            return RenderingParameters.HasValue()
                ? GlassHtml.GetRenderingParameters<TParam>(RenderingParameters)
                : default(TParam);
        }
    }
}

