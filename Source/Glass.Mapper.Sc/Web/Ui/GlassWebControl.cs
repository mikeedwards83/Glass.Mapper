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

namespace Glass.Mapper.Sc.Web.Ui
{
    /// <summary>
    ///     Class GlassUserControl
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GlassWebControl<T> : AbstractGlassWebControl where T : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GlassWebControl{T}" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GlassWebControl(ISitecoreContext context, IGlassHtml glassHtml)
            : base(context, glassHtml)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GlassWebControl{T}" /> class.
        /// </summary>
        protected GlassWebControl()
        {
        }

        /// <summary>
        ///     Model to render on the sublayout
        /// </summary>
        /// <value>The model.</value>
        public T Model { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [infer type].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [infer type]; otherwise, <c>false</c>.
        /// </value>
        public bool InferType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is lazy.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is lazy; otherwise, <c>false</c>.
        /// </value>
        public bool IsLazy { get; set; }

        public virtual string RenderingParameters
        {
            get
            {
                return RenderingContext.GetRenderingParameters();
            }   
        }

        /// <summary>
        ///     Gets the model.
        /// </summary>
        protected virtual void GetModel()
        {
            Model = SitecoreContext.CreateType<T>(LayoutItem, IsLazy, InferType);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="T:System.EventArgs" /> object that contains the event data.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            GetModel();
            base.OnLoad(e);
        }

        /// <summary>
        ///     Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public string Editable(Expression<Func<T, object>> field, object parameters = null)
        {
            return base.Editable(Model, field, parameters);
        }

        /// <summary>
        ///     Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public string Editable(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput,
                               object parameters = null)
        {
            return base.Editable(Model, field, standardOutput, parameters);
        }

        /// <summary>
        ///     Renders an image allowing simple page editor support
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="model">The model that contains the image field</param>
        /// <param name="field">A lambda expression to the image field, should be of type Glass.Mapper.Sc.Fields.Image</param>
        /// <param name="parameters">Image parameters, e.g. width, height</param>
        /// <param name="isEditable">Indicates if the field should be editable</param>
        /// <param name="outputHeightWidth">Indicates if the height and width attributes should be outputted when rendering the image</param>
        /// <returns></returns>
        public virtual string RenderImage(Expression<Func<T, object>> field,
                                          object parameters = null,
                                          bool isEditable = false,
                                             bool outputHeightWidth = true)

        {
            return base.RenderImage(Model, field, parameters, isEditable, outputHeightWidth);
        }

        /// <summary>
        ///     Render HTML for a link with contents
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
            return GlassHtml.BeginRenderLink(Model, field, this.Output, attributes, isEditable);
        }

        /// <summary>
        ///     Render HTML for a link
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="model">The model</param>
        /// <param name="field">The link field to user</param>
        /// <param name="attributes">Any additional link attributes</param>
        /// <param name="isEditable">Make the link editable</param>
        /// <param name="contents">Content to override the default decription or item name</param>
        /// <returns></returns>
        public virtual string RenderLink(Expression<Func<T, object>> field, object attributes = null,
                                         bool isEditable = false, string contents = null)
        {
            return GlassHtml.RenderLink(Model, field, attributes, isEditable, contents);
        }

        public virtual K GetRenderingParameters<K>() where K : class
        {
            return GlassHtml.GetRenderingParameters<K>(RenderingParameters);
        }
    }
}
