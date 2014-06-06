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

namespace Glass.Mapper.Sc.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class GlassView<TModel> : WebViewPage<TModel>
    {

        /// <summary>
        /// 
        /// </summary>
        public IGlassHtml GlassHtml { get; private set; }


        /// <summary>
        /// Gets a value indicating whether this instance is in editing mode.
        /// </summary>
        /// <value><c>true</c> if this instance is in editing mode; otherwise, <c>false</c>.</value>
        public bool IsInEditingMode
        {
            get { return Sc.GlassHtml.IsInEditingMode; }
        }

        /// <summary>
        /// Inits the helpers.
        /// </summary>
        public override void InitHelpers()
        {
            base.InitHelpers();
            GlassHtml = new GlassHtml(new SitecoreContext());
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters"> </param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable<T>(T target, Expression<Func<T, object>> field, object parameters = null)
        {
            return new HtmlString(GlassHtml.Editable(target, field, parameters));
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable<T>(T target, Expression<Func<T, object>> field,
                                      Expression<Func<T, string>> standardOutput, object parameters = null)
        {
            return new HtmlString(GlassHtml.Editable(target, field, standardOutput, parameters));
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
        public virtual HtmlString RenderImage<T>(T target, Expression<Func<T, object>> field,
                                           object parameters = null,
                                           bool isEditable = false)
        {
            return new HtmlString(GlassHtml.RenderImage<T>(target, field, parameters, isEditable));
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
        public HtmlString Editable(Expression<Func<TModel, object>> field, Expression<Func<TModel, string>> standardOutput, object parameters = null)
        {
            return new HtmlString(GlassHtml.Editable(Model, field, standardOutput, parameters));
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
        public virtual HtmlString RenderImage(Expression<Func<TModel, object>> field,
                                           object parameters = null,
                                           bool isEditable = false)
        {
            return new HtmlString(GlassHtml.RenderImage(Model, field, parameters, isEditable));
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
        public virtual RenderingResult BeginRenderLink(Expression<Func<TModel, object>> field,
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
        public virtual HtmlString RenderLink(Expression<Func<TModel, object>> field, object attributes = null, bool isEditable = false, string contents = null)
        {

            return new HtmlString(GlassHtml.RenderLink(this.Model, field, attributes, isEditable, contents));
        }





        

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="dataSource">The data source.</param>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame(string buttons, string dataSource)
        {
            var frame = new GlassEditFrame(buttons, this.Output, dataSource);
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
            var frame = new GlassEditFrame(GlassEditFrame.DefaultEditButtons, this.Output, dataSource);
            frame.RenderFirstPart();
            return frame;
        }

        /// <summary>
        /// Creates an edit frame using the current context item
        /// </summary>
        /// <returns></returns>
        public GlassEditFrame BeginEditFrame()
        {
            var frame = new GlassEditFrame(GlassEditFrame.DefaultEditButtons, this.Output);
            frame.RenderFirstPart();
            return frame;
        }

        public T GetRenderingParameters<T>() where T: class
        {
            var parameters = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering[Sc.GlassHtml.Parameters];
            return
                GlassHtml.GetRenderingParameters<T>(parameters);
        }
      
    }
}
