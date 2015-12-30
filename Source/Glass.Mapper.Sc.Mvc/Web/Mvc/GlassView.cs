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
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc.RenderField;
using Glass.Mapper.Sc.Web.Ui;
using Sitecore.Mvc.Configuration;
using Sitecore.Shell.Applications.Dialogs.ItemLister;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class GlassView<TModel> : WebViewPage<TModel> where TModel : class
    {
        protected IRenderingContext RenderingContext { get; set; }


        public static bool HasDataSource<T>() where T : class
        {
            if (Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull == null || Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering == null)
                return false;

            //this has been taken from Sitecore.Mvc.Presentation.Rendering class

#if (SC70)
            return Sitecore.Context.Database.GetItem(Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering.DataSource) != null;
#else
            return MvcSettings.ItemLocator.GetItem(Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering.DataSource) != null;
#endif

        }



        /// <summary>
        /// 
        /// </summary>
        public IGlassHtml GlassHtml { get; private set; }

        public ISitecoreContext SitecoreContext { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is in editing mode.
        /// </summary>
        /// <value><c>true</c> if this instance is in editing mode; otherwise, <c>false</c>.</value>
        public bool IsInEditingMode
        {
            get { return Sc.GlassHtml.IsInEditingMode; }
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
                return RenderingContext.HasDataSource ? Sitecore.Context.Database.GetItem(RenderingContext.GetDataSource()) : null;
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
        /// Inits the helpers.
        /// </summary>
        public override void InitHelpers()
        {
            base.InitHelpers();
            SitecoreContext = Sc.SitecoreContext.GetFromHttpContext();
            GlassHtml = new GlassHtml(SitecoreContext);
            RenderingContext = new RenderingContextMvcWrapper();

            if (Model == null && this.ViewData.Model == null)
            {
                this.ViewData.Model = GetModel();
            }
        }


        protected virtual TModel GetModel()
        {
            if (RenderingContext.HasDataSource)
            {
                return SitecoreContext.Cast<TModel>(DataSourceItem);
            }
            else
            {
                return SitecoreContext.Cast<TModel>(ContextItem);
            }
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <param name="model">The model object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters"> </param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable<T>(T model, Expression<Func<T, object>> field, object parameters = null)
        {
            return new HtmlString(GlassHtml.Editable(model, field, parameters));
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="model">The model object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable<T>(T model, Expression<Func<T, object>> field,
                                      Expression<Func<T, string>> standardOutput, object parameters = null)
        {
            return new HtmlString(GlassHtml.Editable(model, field, standardOutput, parameters));
        }

        /// <summary>
        /// Renders an image allowing simple page editor support
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="model">The model that contains the image field</param>
        /// <param name="field">A lambda expression to the image field, should be of type Glass.Mapper.Sc.Fields.Image</param>
        /// <param name="parameters">Image parameters, e.g. width, height</param>
        /// <param name="isEditable">Indicates if the field should be editable</param>
        /// <param name="outputHeightWidth">Indicates if the height and width attributes should be rendered to the HTML element</param>
        /// <returns></returns>
        public virtual HtmlString RenderImage<T>(T model, Expression<Func<T, object>> field,
            object parameters = null,
            bool isEditable = false,
            bool outputHeightWidth = false)
        {
            return new HtmlString(GlassHtml.RenderImage<T>(model, field, parameters, isEditable, outputHeightWidth));
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
        public virtual RenderingResult BeginRenderLink<T>(T model, Expression<Func<T, object>> field,
                                                     object attributes = null, bool isEditable = false)
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
        public virtual HtmlString RenderLink<T>(T model, Expression<Func<T, object>> field, object attributes = null, bool isEditable = false, string contents = null)
        {

            return new HtmlString(GlassHtml.RenderLink(model, field, attributes, isEditable, contents));
        }


        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters"></param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable(Expression<Func<TModel, object>> field, object parameters = null)
        {
            return new HtmlString(GlassHtml.Editable(Model, field, parameters));
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable(
            Expression<Func<TModel, object>> field, Expression<Func<TModel, string>> standardOutput, object parameters = null)
        {
            return new HtmlString(GlassHtml.Editable(Model, field, standardOutput, parameters));
        }

        /// <summary>
        /// Renders an image allowing simple page editor support
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="field">A lambda expression to the image field, should be of type Glass.Mapper.Sc.Fields.Image</param>
        /// <param name="parameters">Image parameters, e.g. width, height</param>
        /// <param name="isEditable">Indicates if the field should be editable</param>
        /// <returns></returns>
        public virtual HtmlString RenderImage(Expression<Func<TModel, object>> field,
                                           object parameters = null,
                                           bool isEditable = false,
                                           bool outputHeightWidth = false)
        {
            return new HtmlString(GlassHtml.RenderImage(Model, field, parameters, isEditable, outputHeightWidth ));
        }

        /// <summary>
        /// Render HTML for a link with contents
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="field">The link field to user</param>
        /// <param name="attributes">Any additional link attributes</param>
        /// <param name="isEditable">Make the link editable</param>
        /// <returns></returns>
        public virtual RenderingResult BeginRenderLink(Expression<Func<TModel, object>> field,
                                                     object attributes = null, bool isEditable = false)
        {
            return GlassHtml.BeginRenderLink(this.Model, field, this.Output, attributes, isEditable);

        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="field">The link field to user</param>
        /// <param name="attributes">Any additional link attributes</param>
        /// <param name="isEditable">Make the link editable</param>
        /// <param name="contents">Content to override the default decription or item name</param>
        /// <returns></returns>
        public virtual HtmlString RenderLink(Expression<Func<TModel, object>> field, object attributes = null, bool isEditable = false, string contents = null)
        {

            return new HtmlString(GlassHtml.RenderLink(this.Model, field, attributes, isEditable, contents));
        }




        /// <summary>
        /// Returns an Sitecore Edit Frame
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="path">The path.</param>
        /// <param name="output">The stream to write the editframe output to. If the value is null the HttpContext Response Stream is used.</param>
        /// <returns>
        /// GlassEditFrame.
        /// </returns>
        public GlassEditFrame BeginEditFrame<T>(T model, string title = null, params Expression<Func<T, object>>[] fields)
            where T : class
        {
           return GlassHtml.EditFrame(model, title, this.Output, fields);
        }

        

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="dataSource">The data source.</param>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame(string buttons, string dataSource)
        {
            return GlassHtml.EditFrame(string.Empty, buttons, dataSource, this.Output);
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="dataSource">The data source.</param>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame(string buttons, string dataSource, string title)
        {
            return GlassHtml.EditFrame(title, buttons, dataSource, this.Output);
        }


        /// <summary>
        /// Creates an Edit Frame using the Default Buttons list
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame(string dataSource)
        {
            return GlassHtml.EditFrame(string.Empty, GlassEditFrame.DefaultEditButtons, dataSource, this.Output);
        }

        /// <summary>
        /// Creates an edit frame using the current context item
        /// </summary>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame()
        {
            var frame = new GlassEditFrame(string.Empty, GlassEditFrame.DefaultEditButtons, this.Output);
            frame.RenderFirstPart();
            return frame;
        }

        public T GetRenderingParameters<T>() where T : class
        {
            string renderingParameters = RenderingContext.GetRenderingParameters();
            return renderingParameters.HasValue() ? GlassHtml.GetRenderingParameters<T>(renderingParameters) : null;
        }

    }
}
