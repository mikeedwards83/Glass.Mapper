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
using System.IO;
using System.Linq.Expressions;
using Glass.Mapper.Sc.RenderField;
using Glass.Mapper.Sc.Web.Ui;
using Sitecore.Data;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// IGlassHtml
    /// </summary>
    public interface IGlassHtml
    {
        /// <summary>
        /// Gets the sitecore context.
        /// </summary>
        /// <value>
        /// The sitecore context.
        /// </value>
        ISitecoreContext SitecoreContext { get; }

        /// <summary>
        /// Edits the frame.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        /// GlassEditFrame.
        /// </returns>
        GlassEditFrame EditFrame(string buttons, string path = null);

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters">Additional rendering parameters, e.g. ImageParameters</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        string Editable<T>(T target, Expression<Func<T, object>> field, object parameters = null);

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <param name="parameters">Additional rendering parameters, e.g. ImageParameters</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        string Editable<T>(T target, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, object parameters = null);

        /// <summary>
        /// Renders an image allowing simple page editor support
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="field"></param>
        /// <param name="parameters"></param>
        /// <param name="isEditable"></param>
        /// <returns></returns>
        string RenderImage<T>(T model, Expression<Func<T, object>> field, object parameters = null, bool isEditable = false);

        RenderingResult BeginRenderLink<T>(T model, Expression<Func<T, object>> field, TextWriter writer,
                                      object attributes = null, bool isEditable = false);

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <returns>An "a" HTML element</returns>
        string RenderLink<T>(T model, Expression<Func<T, object>> field, object attributes = null,
                             bool isEditable = false, string contents = null);


        /// <summary>
        /// Gets rendering parameters using the specified template.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="renderParametersTemplateId">The template used by the rendering parameters</param>
        /// <returns></returns>
        T GetRenderingParameters<T>(string parameters, ID renderParametersTemplateId) where T : class;

        /// <summary>
        /// Converts rendering parameters to a concrete type. Use this method if you have defined the template ID on the 
        /// model configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        T GetRenderingParameters<T>(string parameters) where T : class;


        /// <summary>
        /// Converts rendering parameters to a concrete type. Use this method if you have defined the template ID on the 
        /// model configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        T GetRenderingParameters<T>(NameValueCollection parameters) where T : class;




        /// <summary>
        /// Gets rendering parameters using the specified template.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="renderParametersTemplateId">The template used by the rendering parameters</param>
        /// <returns></returns>
        T GetRenderingParameters<T>(NameValueCollection parameters, ID renderParametersTemplateId) where T : class;

        #region Obsolete

        /// <summary>
        /// Renders HTML for an image
        /// </summary>
        /// <param name="image">The image to render</param>
        /// <returns>An img HTML element</returns>
        [Obsolete("RenderImage<T>(T model, Expression<Func<T, object>> field, ImageParameters parameters = null, bool isEditable = false);")]
        string RenderImage(Fields.Image image);

        /// <summary>
        /// Renders HTML for an image
        /// </summary>
        /// <param name="image">The image to render</param>
        /// <param name="attributes">Additional parameters to add. Do not include alt or src</param>
        /// <returns>An img HTML element</returns>
        [Obsolete("RenderImage<T>(T model, Expression<Func<T, object>> field, ImageParameters parameters = null, bool isEditable = false);")]
        string RenderImage(Fields.Image image, NameValueCollection attributes);

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <returns>An "a" HTML element</returns>
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection parameters = null, bool isEditable = false, string contents = null);")]
        string RenderLink(Fields.Link link);

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional parameters to add. Do not include href or title</param>
        /// <returns>An "a" HTML element</returns>
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection parameters = null, bool isEditable = false, string contents = null);")]
        string RenderLink(Fields.Link link, NameValueCollection attributes);

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional parameters to add. Do not include href or title</param>
        /// <param name="contents">Content to go in the link instead of the standard text</param>
        /// <returns>An "a" HTML element</returns>
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection parameters = null, bool isEditable = false, string contents = null);")]
        string RenderLink(Fields.Link link, NameValueCollection attributes, string contents);

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        [Obsolete("Use Editable<T>(T target, Expression<Func<T, object>> field)")]
        string Editable<T>(Expression<Func<T, object>> field, T target);

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor.  Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        [Obsolete("Use Editable<T>(T target, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)")]
        string Editable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target);

        #endregion
       
    }
}
