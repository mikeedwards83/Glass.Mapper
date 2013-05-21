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
using System.Dynamic;
using System.Linq.Expressions;
using Glass.Mapper.Sc.RenderField;

namespace Glass.Mapper.Sc.Web.Ui
{
    /// <summary>
    /// Class GlassUserControl
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GlassUserControl<T> : AbstractGlassUserControl where T : class
    {

        /// <summary>
        /// Model to render on the sublayout
        /// </summary>
        /// <value>The model.</value>
        public T Model { get; set; }

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
        /// Initializes a new instance of the <see cref="GlassUserControl{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GlassUserControl(ISitecoreContext context) : base(context) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GlassUserControl{T}"/> class.
        /// </summary>
        public GlassUserControl() : base() { }


        /// <summary>
        /// Gets the model.
        /// </summary>
        protected virtual void GetModel()
        {


            Model = SitecoreContext.CreateType<T>(LayoutItem, IsLazy, InferType);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            GetModel();
            base.OnLoad(e);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>System.String.</returns>
        public string Editable(Expression<Func<T, object>> field)
        {
            return base.Editable(this.Model, field);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public string Editable(Expression<Func<T, object>> field, string parameters)
        {
            return base.Editable(this.Model, field, parameters);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public string Editable(Expression<Func<T, object>> field, AbstractParameters parameters)
        {
            return base.Editable(this.Model, field, parameters);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <returns>System.String.</returns>
        public string Editable(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)
        {
            return   base.Editable(this.Model, field, standardOutput);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public string Editable(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput,
                               AbstractParameters parameters)
        {
            return base.Editable(this.Model, field, standardOutput, parameters);
        }
    }
}

