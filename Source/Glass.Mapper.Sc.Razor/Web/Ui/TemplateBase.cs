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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Web.Mvc;
using Glass.Mapper.Sc.Razor.Web.Mvc;
using RazorEngine.Text;
using Sitecore.Web.UI;
using Sitecore.Web.UI.WebControls;
using Image = Glass.Mapper.Sc.Fields.Image;
using System.Web;
using Glass.Mapper.Sc.RenderField;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    /// <summary>
    /// Class TemplateBase
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class TemplateBase<TModel> : RazorEngine.Templating.TemplateBase<TModel>, ITemplateBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateBase{T}"/> class.
        /// </summary>
        public TemplateBase()
        {


        }

        /// <summary>
        /// Gets or sets the sitecore context.
        /// </summary>
        /// <value>
        /// The sitecore context.
        /// </value>
        public ISitecoreContext SitecoreContext { get; set; }

        /// <summary>
        /// Gets the view data.
        /// </summary>
        /// <value>The view data.</value>
        public ViewDataDictionary ViewData { get; private set; }


        private GlassHtmlFacade _glassHtml;

        /// <summary>
        /// Gets the glass HTML.
        /// </summary>
        /// <value>The glass HTML.</value>
        public GlassHtmlFacade GlassHtml
        {
            get
            {
                if (_glassHtml == null)
                    _glassHtml = new GlassHtmlFacade(SitecoreContext, new HtmlTextWriter(this.CurrentWriter));

                return _glassHtml;
            }
        }

        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <value>The HTML.</value>
        public HtmlHelper Html { get; private set; }

        /// <summary>
        /// Gets the placeholders.
        /// </summary>
        /// <value>The placeholders.</value>
        public IEnumerable<Placeholder> Placeholders
        {
            get
            {
                return ParentControl.Controls.Cast<Control>()
                                    .Where(x => x is global::Sitecore.Web.UI.WebControls.Placeholder)
                                    .Cast<global::Sitecore.Web.UI.WebControls.Placeholder>();
            }
        }

        /// <summary>
        /// Gets the parent control.
        /// </summary>
        /// <value>The parent control.</value>
        public WebControl ParentControl { get; private set; }



        /// <summary>
        /// Configures the specified service.
        /// </summary>
        /// <param name="sitecoreContext">The sitecore context.</param>
        /// <param name="viewData">The view data.</param>
        /// <param name="parentControl">The parent control.</param>
        public void Configure(ISitecoreContext sitecoreContext, ViewDataDictionary viewData, WebControl parentControl)
        {
            SitecoreContext = sitecoreContext;

            Html = new HtmlHelper(new ViewContext(), new ViewDataContainer() {ViewData = ViewData});
            ViewData = viewData;
            ParentControl = parentControl;
            if (parentControl != null && parentControl.Page != null)
                IsPostback = parentControl.Page.IsPostBack;
            else if(
                HttpContext.Current !=null
                && HttpContext.Current.Request != null
                && HttpContext.Current.Request.HttpMethod != null)
                    IsPostback = HttpContext.Current.Request.HttpMethod.ToUpperInvariant() == "POST";
        }

        /// <summary>
        /// Renders the holder.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public string RenderHolder(string key)
        {
            key = key.ToLower();
            var placeHolder = Placeholders.FirstOrDefault(x => x.Key == key);

            if (placeHolder == null)
                return "No placeholder with key: {0}".Formatted(key);
            else
            {
                var sb = new StringBuilder();
                placeHolder.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Placeholders the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>IEncodedString.</returns>
        public IEncodedString Placeholder(string key)
        {
            key = key.ToLower();
            var placeHolder = Placeholders.FirstOrDefault(x => x.Key == key);

            if (placeHolder == null)
                placeHolder = new global::Sitecore.Web.UI.WebControls.Placeholder {Key = key};
            ParentControl.Controls.Add(placeHolder);

            var sb = new StringBuilder();
            placeHolder.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
            return Raw(sb.ToString());
        }





        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T">The type of the t1.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <returns>IEncodedString.</returns>
        public IEncodedString Editable<T>(T target, Expression<Func<T, object>> field)
        {
            return GlassHtml.Editable(target, field);
        }

        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T">The type of the t1.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <returns>IEncodedString.</returns>
        public IEncodedString Editable<T>(T target, Expression<Func<T, object>> field, AbstractParameters parameters)
        {
            return GlassHtml.Editable(target, field, parameters);
        }

        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T">The type of the t1.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IEncodedString.</returns>
        public IEncodedString Editable<T>(T target, Expression<Func<T, object>> field, string parameters)
        {
            return GlassHtml.Editable(target, field, parameters);
        }
        
        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T">The type of the t1.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <returns>
        /// IEncodedString.
        /// </returns>
        public IEncodedString Editable<T>(T target, Expression<Func<T, object>> field,
                                           Expression<Func<T, string>> standardOutput)
        {
            return GlassHtml.Editable(target, field, standardOutput);
        }

        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T">The type of the t1.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <returns>
        /// IEncodedString.
        /// </returns>
        public IEncodedString Editable<T>(T target, Expression<Func<T, object>> field,
                                           Expression<Func<T, string>> standardOutput, AbstractParameters parameters)
        {
            return GlassHtml.Editable(target, field, standardOutput,parameters);
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
        public virtual IEncodedString RenderImage<T>(T target, Expression<Func<T, object>> field,
                                             ImageParameters parameters = null,
                                             bool isEditable = false)
        {
            return GlassHtml.RenderImage(target, field, parameters, isEditable);
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
        public virtual IEncodedString RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection attributes = null, bool isEditable = false, string contents = null)
        {

            return Raw(_glassHtml.RenderLink(model, field, attributes, isEditable, contents));
        }















        /// <summary>
        /// Editables the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>RawString.</returns>
        /// <exception cref="System.NullReferenceException">
        /// No field set
        /// or
        /// No model set
        /// </exception>
        public IEncodedString Editable(Expression<Func<TModel, object>> field)
        {
                return GlassHtml.Editable(this.Model, field);
        }

        /// <summary>
        /// Editables the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>RawString.</returns>
        /// <exception cref="System.NullReferenceException">
        /// No field set
        /// or
        /// No model set
        /// </exception>
        public IEncodedString Editable(Expression<Func<TModel, object>> field, AbstractParameters parameters)
        {
            return GlassHtml.Editable(this.Model, field, parameters);
        }

        /// <summary>
        /// Editables the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>RawString.</returns>
        /// <exception cref="System.NullReferenceException">
        /// No field set
        /// or
        /// No model set
        /// </exception>
        public IEncodedString Editable(Expression<Func<TModel, object>> field, string parameters)
        {
            return GlassHtml.Editable(this.Model, field, parameters);
        }


        /// <summary>
        /// Editables the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <returns>
        /// RawString.
        /// </returns>
        /// <exception cref="System.NullReferenceException">No field set
        /// or
        /// No model set</exception>
        public RawString Editable(Expression<Func<TModel, object>> field,
                                  Expression<Func<TModel, string>> standardOutput)
        {
            return GlassHtml.Editable(this.Model, field, standardOutput);
        }


        /// <summary>
        /// Editables the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <returns>
        /// RawString.
        /// </returns>
        /// <exception cref="System.NullReferenceException">No field set
        /// or
        /// No model set</exception>
        public RawString Editable(Expression<Func<TModel, object>> field,
                                  Expression<Func<TModel, string>> standardOutput, AbstractParameters parameters)
        {
            return GlassHtml.Editable(this.Model, field, standardOutput, parameters);
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
        public virtual IEncodedString RenderImage(Expression<Func<TModel, object>> field,
                                             ImageParameters parameters = null,
                                             bool isEditable = false)
        {
            return GlassHtml.RenderImage(this.Model, field, parameters, isEditable);
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
                                                     NameValueCollection attributes = null, bool isEditable = false)
        {
            return GlassHtml.GlassHtml.BeginRenderLink(this.Model, field, this.CurrentWriter, attributes, isEditable);

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
        public virtual IEncodedString RenderLink(Expression<Func<TModel, object>> field, NameValueCollection attributes = null, bool isEditable = false, string contents = null)
        {

            return this.RenderLink(this.Model, field, attributes, isEditable, contents);
        }


        /// <summary>
        /// Gets a value indicating whether this instance is in editing mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in editing mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsInEditingMode
        {
            get { return Sc.GlassHtml.IsInEditingMode; }

        }


        /// <summary>
        /// Converts rendering parameters to a concrete type. Use this method if you have defined the template ID on the 
        /// model configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual T GetRenderingParameters<T>() where T : class
        {
            return GlassHtml.RenderingParameters<T>(this.ParentControl.Parameters);
        }












        /// <summary>
        /// Gets a value indicating whether this instance is in editing mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in editing mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsPostback
        {
            get;
            private set;

        }



        #region Obsolete


        /// <summary>
        /// Renders the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>IEncodedString.</returns>
        [Obsolete("Use RenderImage(Expression<Func<T, object>> field, ImageParameters parameters = null, bool isEditable = false)")]
        public IEncodedString RenderImage(Image image)
        {
            return GlassHtml.RenderImage(image);
        }

        /// <summary>
        /// Renders the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>IEncodedString.</returns>
        [Obsolete("Use RenderImage(Expression<Func<T, object>> field, ImageParameters parameters = null, bool isEditable = false)")]
        public IEncodedString RenderImage(Image image, NameValueCollection attributes)
        {
            return GlassHtml.RenderImage(image, attributes);
        }


        #endregion

       
    }
}
