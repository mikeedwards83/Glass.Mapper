using System;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using Glass.Mapper.Sc.Web.Ui;

namespace Glass.Mapper.Sc.Web.Mvc
{
    public class GlassHtmlMvc<TK>
    {
        protected IGlassHtml GlassHtml { get; private set; }

        protected TextWriter Output { get; private set; }

        protected TK Model { get; set; }

        public GlassHtmlMvc(IGlassHtml glassHtml, TextWriter output, TK model)
        {
            Output = output;
            Model = model;
            GlassHtml = glassHtml;
        }

        public ISitecoreContext SitecoreContext
        {
            get { return GlassHtml.SitecoreContext; }
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
        /// <typeparam name="T">A class loaded by Glass.Mapper.Sc</typeparam>
        /// <param name="model">The model object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters">Parameters to pass to the render field pipeline if it is used</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable<T>(T model, Expression<Func<T, object>> field,
            Expression<Func<T, string>> standardOutput, object parameters = null)
        {
            return new HtmlString(GlassHtml.Editable(model, field, standardOutput, parameters));
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
            return GlassHtml.BeginRenderLink(model, field, Output, attributes, isEditable);

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
        public virtual HtmlString RenderLink<T>(T model, Expression<Func<T, object>> field, object attributes = null,
            bool isEditable = false, string contents = null)
        {

            return new HtmlString(GlassHtml.RenderLink(model, field, attributes, isEditable, contents));
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="dataSource">The data source.</param>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame(string buttons, string dataSource)
        {
            return BeginEditFrame(buttons, dataSource, string.Empty);
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="title">The title for the edit frame</param>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame(string buttons, string dataSource, string title)
        {
            var frame = new GlassEditFrame(title, buttons, Output, dataSource);
            frame.RenderFirstPart();
            return frame;
        }

        /// <summary>
        /// Creates an Edit Frame using the Default Buttons list
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame(string dataSource)
        {
            var frame = new GlassEditFrame(string.Empty,GlassEditFrame.DefaultEditButtons, Output, dataSource);
            frame.RenderFirstPart();
            return frame;
        }

        /// <summary>
        /// Creates an edit frame using the current context item
        /// </summary>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame()
        {
            var frame = new GlassEditFrame(string.Empty, GlassEditFrame.DefaultEditButtons, Output);
            frame.RenderFirstPart();
            return frame;
        }


        /// <summary>
        /// Gets rendering parameters using the specified template.
        /// </summary>
        /// <typeparam name="T">The Type to construct using the rendering parameters</typeparam>
        /// <returns></returns>
        public T GetRenderingParameters<T>() where T : class
        {
            if (Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull == null)
                return null;

            var parameters = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering[Sc.GlassHtml.Parameters];
            return
                GlassHtml.GetRenderingParameters<T>(parameters);
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
        public virtual HtmlString RenderImage<T>(T model, Expression<Func<T, object>> field,
            object parameters = null,
            bool isEditable = false,
            bool outputHeightWidth = false)

        {
            return new HtmlString(GlassHtml.RenderImage(model, field, parameters, isEditable, outputHeightWidth));
        }




        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters"></param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable(Expression<Func<TK, object>> field, object parameters = null)
        {
            return new HtmlString(GlassHtml.Editable(Model, field, parameters));
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <param name="parameters">Parameters to pass to the render field pipeline if it is used</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable(Expression<Func<TK, object>> field,
            Expression<Func<TK, string>> standardOutput, object parameters = null)
        {
            return new HtmlString(GlassHtml.Editable(Model, field, standardOutput, parameters));
        }

        /// <summary>
        /// Renders an image allowing simple page editor support
        /// </summary>
        /// <param name="field">A lambda expression to the image field, should be of type Glass.Mapper.Sc.Fields.Image</param>
        /// <param name="parameters">Image parameters, e.g. width, height</param>
        /// <param name="isEditable">Indicates if the field should be editable</param>
        /// <returns></returns>
        public virtual HtmlString RenderImage(Expression<Func<TK, object>> field,
            object parameters = null,
            bool isEditable = false)
        {
            return new HtmlString(GlassHtml.RenderImage(Model, field, parameters, isEditable));
        }

        /// <summary>
        /// Render HTML for a link with contents
        /// </summary>
        /// <param name="field">The link field to user</param>
        /// <param name="attributes">Any additional link attributes</param>
        /// <param name="isEditable">Make the link editable</param>
        /// <returns></returns>
        public virtual RenderingResult BeginRenderLink(Expression<Func<TK, object>> field,
            object attributes = null, bool isEditable = false)
        {
            return GlassHtml.BeginRenderLink(Model, field, Output, attributes, isEditable);

        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="field">The link field to user</param>
        /// <param name="attributes">Any additional link attributes</param>
        /// <param name="isEditable">Make the link editable</param>
        /// <param name="contents">Content to override the default decription or item name</param>
        /// <returns></returns>
        public virtual HtmlString RenderLink(Expression<Func<TK, object>> field, object attributes = null,
            bool isEditable = false, string contents = null)
        {

            return new HtmlString(GlassHtml.RenderLink(Model, field, attributes, isEditable, contents));
        }




        /// <summary>
        /// Returns an Sitecore Edit Frame
        /// </summary>
        /// <param name="model">The model of the item to use.</param>
        /// <param name="title">The title to display with the editframe</param>
        /// <param name="fields">The fields to add to the edit frame</param>
        /// <returns>
        /// GlassEditFrame.
        /// </returns>
        public GlassEditFrame BeginEditFrame<T>(T model, string title = null,
            params Expression<Func<T, object>>[] fields)
            where T : class
        {
            return GlassHtml.EditFrame(model, title, Output, fields);
        }




    }
}
