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
using System.Web;
using System.Web.Mvc;
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
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable(Expression<Func<TModel, object>> field)
        {
            return Editable(field, (Expression<Func<TModel, string>>)null);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable(Expression<Func<TModel, object>> field,
                                   Expression<Func<TModel, string>> standardOutput)
        {
            return new HtmlString(GlassHtml.Editable(Model, field, standardOutput));
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters"> </param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable(Expression<Func<TModel, object>> field, string parameters)
        {
            return new HtmlString(GlassHtml.Editable(Model, field, parameters));
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable<T>(T target, Expression<Func<T, object>> field)
        {
            return new HtmlString(GlassHtml.Editable(target, field));
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
                                      Expression<Func<T, string>> standardOutput)
        {
            return new HtmlString(GlassHtml.Editable(target, field, standardOutput));
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters"> </param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public HtmlString Editable<T>(T target, Expression<Func<T, object>> field, string parameters)
        {
            return new HtmlString(GlassHtml.Editable(target, field, parameters));
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
    }
}
