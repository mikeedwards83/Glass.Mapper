
using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc.IoC;
using Glass.Mapper.Sc.Web.Ui;
using Sitecore.Mvc.Configuration;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    [Obsolete("GlassView is now obsolete. Use @html.Glass() helper methods")]
    public abstract class GlassView<TModel> : WebViewPage<TModel> where TModel : class
    {
        public IMvcContext MvcContext { get; set; }

        public GlassView() 
            : this(new MvcContext(new SitecoreService(Sitecore.Context.Database)))
        {

        }

        public GlassView(
            IMvcContext mvcContext
        )
        {
            MvcContext = mvcContext;
        }


        public static bool HasDataSource<T>() where T : class
        {
            if (Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull == null || Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering == null)
                return false;

            //this has been taken from Sitecore.Mvc.Presentation.Rendering class


            return MvcSettings.ItemLocator.GetItem(Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering.DataSource) != null;

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
            get { return MvcContext.DataSourceItem; }
        }

        /// <summary>
        /// Returns the Context Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetContext<T>(GetKnownOptions options = null) where T : class
        {
            return MvcContext.GetContextItem<T>(options ?? new GetKnownOptions());
        }

        /// <summary>
        /// Returns the Data Source Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDataSource<T>(GetKnownOptions options = null ) where T : class
        {
            return MvcContext.GetDataSourceItem<T>(options ?? new GetKnownOptions());
        }

        /// <summary>
        /// Returns the Layout Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetLayout<T>(GetKnownOptions options = null) where T : class
        {
            return MvcContext.HasDataSource
                ? GetDataSource<T>(options ?? new GetKnownOptions())
                : GetContext<T>(options ?? new GetKnownOptions());
        }

        /// <summary>
        /// Inits the helpers.
        /// </summary>
        public override void InitHelpers()
        {
            if (Model == null && this.ViewData.Model == null)
            {
                this.ViewData.Model = GetModel();
            }
            base.InitHelpers();
        }

        protected virtual TModel GetModel(GetKnownOptions options = null)
        {
                return GetLayout<TModel>(options ?? new GetKnownOptions());
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
            return new HtmlString(MvcContext.GlassHtml.Editable(model, field, parameters));
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
            return new HtmlString(MvcContext.GlassHtml.Editable(model, field, standardOutput, parameters));
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
            return new HtmlString(MvcContext.GlassHtml.RenderImage<T>(model, field, parameters, isEditable, outputHeightWidth));
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
            return MvcContext.GlassHtml.BeginRenderLink(model, field, this.Output, attributes, isEditable);

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

            return new HtmlString(MvcContext.GlassHtml.RenderLink(model, field, attributes, isEditable, contents));
        }


        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters"></param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable(Expression<Func<TModel, object>> field, object parameters = null)
        {
            return new HtmlString(MvcContext.GlassHtml.Editable(Model, field, parameters));
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
            return new HtmlString(MvcContext.GlassHtml.Editable(Model, field, standardOutput, parameters));
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
            return new HtmlString(MvcContext.GlassHtml.RenderImage(Model, field, parameters, isEditable, outputHeightWidth ));
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
            return MvcContext.GlassHtml.BeginRenderLink(this.Model, field, this.Output, attributes, isEditable);

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

            return new HtmlString(MvcContext.GlassHtml.RenderLink(this.Model, field, attributes, isEditable, contents));
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
           return MvcContext.GlassHtml.EditFrame(model, title, this.Output, fields);
        }

        

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="dataSource">The data source.</param>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame(string buttons, string dataSource)
        {
            return MvcContext.GlassHtml.EditFrame(string.Empty, buttons, dataSource, this.Output);
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="dataSource">The data source.</param>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame(string buttons, string dataSource, string title)
        {
            return MvcContext.GlassHtml.EditFrame(title, buttons, dataSource, this.Output);
        }


        /// <summary>
        /// Creates an Edit Frame using the Default Buttons list
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame(string dataSource)
        {
            return MvcContext.GlassHtml.EditFrame(string.Empty, GlassEditFrame.DefaultEditButtons, dataSource, this.Output);
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
            string renderingParameters = MvcContext.RenderingParameters;
            return renderingParameters.HasValue() ? MvcContext.GlassHtml.GetRenderingParameters<T>(renderingParameters) : null;
        }

    }
}
