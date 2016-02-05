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
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Glass.Mapper.Sc.Web.Ui
{
    /// <summary>
    /// Class GlassUserControl
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GlassUserControl<T> : AbstractGlassUserControl where T : class
    {
        private T model;

        public GlassUserControl(ISitecoreContext context, IGlassHtml glassHtml, IRenderingContext renderingContext)
            : base(context, glassHtml, renderingContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassUserControl{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        [ExcludeFromCodeCoverage] // No logic to test
        public GlassUserControl(ISitecoreContext context)
            : base(context)
        {
            // todo: NM - consider this constructor - I don't think webforms will allow you to set it ??
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassUserControl{T}"/> class.
        /// </summary>
        public GlassUserControl()
        {
        }

        /// <summary>
        /// Model to render on the sublayout
        /// </summary>
        /// <value>The model.</value>
        public T Model
        {
            get
            {
                if (model == null)
                {
                    GetModel();
                }

                return model;
            }
            set { model = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [infer type].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [infer type]; otherwise, <c>false</c>.
        /// </value>
        public bool InferType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is lazy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is lazy; otherwise, <c>false</c>.
        /// </value>
        public bool IsLazy { get; set; }

       

        /// <summary>
        /// Gets the model.
        /// </summary>
        protected virtual void GetModel()
        {
            model = GetLayoutItem<T>(IsLazy, InferType);
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
        /// <param name="field">A lambda expression to the image field, should be of type Glass.Mapper.Sc.Fields.Image</param>
        /// <param name="parameters">Image parameters, e.g. width, height</param>
        /// <param name="isEditable">Indicates if the field should be editable</param>
        /// <param name="outputHeightWidth">Whether to output the height and width attributes</param>
        /// <returns></returns>
        public virtual string RenderImage(Expression<Func<T, object>> field, object parameters = null, bool isEditable = false, bool outputHeightWidth = false)
        {
            return base.RenderImage(this.Model, field, parameters, isEditable, outputHeightWidth);
        }

        /// <summary>
        /// Render HTML for a link with contents
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
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

