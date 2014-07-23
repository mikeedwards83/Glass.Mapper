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
using System.Web.UI;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.Razor.RenderingTypes;
using Glass.Mapper.Sc.Razor.Web.Ui;
using Glass.Mapper.Sc.RenderField;
using Glass.Mapper.Sc.Web.Ui;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Web.UI;

namespace Glass.Mapper.Sc.Razor
{
    /// <summary>
    /// Class GlassHtmlFacade
    /// </summary>
    public class GlassHtmlFacade 
    {
        private readonly HtmlTextWriter _writer;

        private IGlassHtml _glassHtml;

        /// <summary>
        /// Gets the sitecore context.
        /// </summary>
        /// <value>
        /// The sitecore context.
        /// </value>
        public ISitecoreContext SitecoreContext
        {
            get { return _glassHtml.SitecoreContext; }
        }


        /// <summary>
        /// Gets or sets the glass HTML.
        /// </summary>
        /// <value>
        /// The glass HTML.
        /// </value>
        public IGlassHtml GlassHtml
        {
            get { return _glassHtml; }
            set { _glassHtml = value; }
        }

        public ITemplateBase TemplateBase { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassHtmlFacade" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="writer">The writer.</param>
        public GlassHtmlFacade(ISitecoreContext context, HtmlTextWriter writer, ITemplateBase templateBase)

        {
            _writer = writer;
            _glassHtml = new GlassHtml(context);
            TemplateBase = templateBase;
        }
  
        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>RawString.</returns>
        public  RawString Editable<T>(T target, Expression<Func<T, object>> field, object parameters = null)
        {
            return _glassHtml.Editable(target, field, parameters).RawString();
        }

        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>RawString.</returns>
        public RawString Editable<T>(T target, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, object parameters = null)
        {
            return _glassHtml.Editable(target, field, standardOutput, parameters).RawString();
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
        public virtual RawString RenderImage<T>(T model,
                                             Expression<Func<T, object>> field,
                                             object parameters = null,
                                             bool isEditable = false)
        {
            return _glassHtml.RenderImage(model, field, parameters, isEditable).RawString();
        }

        /// <summary>
        /// Indicates if the current page is in Page Editing mode
        /// </summary>
        public bool IsInEditingMode
        {
            get { return Sc.GlassHtml.IsInEditingMode; }
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
            return GlassHtml.BeginRenderLink(model, field, _writer, attributes, isEditable);

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
        public virtual string RenderLink<T>(T model, Expression<Func<T, object>> field, object attributes = null, bool isEditable = false, string contents = null)
        {

            return GlassHtml.RenderLink(model, field, attributes, isEditable, contents);
        }








        /// <summary>
        /// Edits the frame.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="dataSource">The data source.</param>
        /// <returns></returns>
        public GlassEditFrame EditFrame(string buttons, string dataSource = null)
        {
            

            var frame = new GlassEditFrame(buttons, _writer, dataSource);
            frame.RenderFirstPart();
            return frame;
            
        }


        /// <summary>
        /// Renders the partial.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="model">The model.</param>
        public void RenderPartial<T>(string path, T model)
        {
            var item = Sitecore.Context.Database.GetItem(path);

            Assert.IsNotNull(item, "Could not find rendering item {0}".Formatted(path));

            var parameters = new NameValueCollection();

            foreach (Field field in item.Fields)
            {
                parameters.Add(field.Name, field.Value);
            }

            Control  control;
            
            if (item.TemplateID == SitecoreIds.GlassBehindRazorId)
            {
                var renderType = new BehindRazorRenderingType();
                control = renderType.GetControl(parameters, false);
            }
            else
            {
                var renderType = new PartialRazorRenderingType();
                control = renderType.GetControl(parameters, false);

                if (control is PartialControl)
                    control.CastTo<PartialControl>().SetModel(model);
            }
            var webControl = control as WebControl;
            webControl.RenderControl(_writer);
        }


        /// <summary>
        /// Converts rendering parameters to a concrete type. Use this method if you have defined the template ID on the 
        /// model configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual T RenderingParameters<T>(string parameters) where T : class
        {
            return GlassHtml.GetRenderingParameters<T>(parameters);
        }


        /// <summary>
        /// Converts rendering parameters to a concrete type. Use this method if you have defined the template ID on the 
        /// model configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual T RenderingParameters<T>(NameValueCollection parameters) where T : class
        {
            return GlassHtml.GetRenderingParameters<T>(parameters);
        }

       
        #region Obsolete

        /// <summary>
        /// Renders the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>RawString.</returns>
        [Obsolete("Use RenderImage<T>(T model, Expression<Func<T, object>> field, ImageParameters parameters = null, bool isEditable = false)")]
        public RawString RenderImage(Image image)
        {
            return _glassHtml.RenderImage(image).RawString();
        }
        /// <summary>
        /// Renders the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>RawString.</returns>
        [Obsolete("Use RenderImage<T>(T model, Expression<Func<T, object>> field, ImageParameters parameters = null, bool isEditable = false)")]
        public RawString RenderImage(Image image, NameValueCollection attributes)
        {
            return _glassHtml.RenderImage(image, attributes).RawString();
        }

        /// <summary>
        /// Renders the link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns>RawString.</returns>
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection attributes = null, bool isEditable = false, string contents = null)")]
        public RawString RenderLink(Link link)
        {
            return _glassHtml.RenderLink(link).RawString();
        }
        /// <summary>
        /// Renders the link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>RawString.</returns>
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection attributes = null, bool isEditable = false, string contents = null)")]
        public RawString RenderLink(Link link, NameValueCollection attributes)
        {
            return _glassHtml.RenderLink(link, attributes).RawString();
        }
        /// <summary>
        /// Renders the link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="contents">The contents.</param>
        /// <returns>RawString.</returns>
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection attributes = null, bool isEditable = false, string contents = null)")]
        public RawString RenderLink(Link link, NameValueCollection attributes, string contents)
        {
            return _glassHtml.RenderLink(link, attributes, contents).RawString();
        }
        #endregion
    }
}

