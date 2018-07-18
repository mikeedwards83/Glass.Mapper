using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Glass.Mapper.Sc.Web.WebForms.Ui
{
    /// <summary>
    /// Class GlassPage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GlassPage<T> : AbstractGlassPage where T : class
    {

      

        /// <summary>
        /// Model to render on the sublayout
        /// </summary>
        /// <value>The model.</value>
        public T Model { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassPage{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
         public GlassPage(IWebFormsContext context) : base(context) { }
         /// <summary>
         /// Initializes a new instance of the <see cref="GlassPage{T}"/> class.
         /// </summary>
         public GlassPage() : base(new WebFormsContext()) { }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {

            var options = new GetItemByItemOptions()
            {
                Item = LayoutItem
            };

            Model = WebContext.SitecoreService.GetItem<T>(options);
            base.OnLoad(e);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public string Editable(Expression<Func<T, object>> field, object parameters = null)
        {
            return base.Editable(this.Model, field, parameters);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public string Editable(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput,
                               object parameters = null)
        {
            return base.Editable(this.Model, field, standardOutput, parameters);
        }

        /// <summary>
        /// Renders an image allowing simple page editor support
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="model">The model that contains the image field</param>
        /// <param name="field">A lambda expression to the image field, should be of type Glass.Mapper.Sc.Fields.Image</param>
        /// <param name="parameters">Image parameters, e.g. width, height</param>
        /// <param name="isEditable">Indicates if the field should be editable</param>
        /// <returns></returns>
        public virtual string RenderImage(Expression<Func<T, object>> field,
                                             object parameters = null,
                                             bool isEditable = false,
                                            bool outputHeightWidth = false
            )
        {
            return base.RenderImage(this.Model, field, parameters, isEditable, outputHeightWidth);
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
        public virtual RenderingResult BeginRenderLink(Expression<Func<T, object>> field,
                                                     object attributes = null, bool isEditable = false)
        {
            return GlassHtml.BeginRenderLink(this.Model, field, this.Output, attributes, isEditable);

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
        public virtual string RenderLink(Expression<Func<T, object>> field, object attributes = null, bool isEditable = false, string contents = null)
        {

            return GlassHtml.RenderLink(this.Model, field, attributes, isEditable, contents);
        }

       
    }

}

