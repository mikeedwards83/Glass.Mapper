

using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq.Expressions;
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
        ISitecoreService SitecoreService { get; }

        /// <summary>
        /// Returns an Sitecore Edit Frame
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="path">The path.</param>
        /// <param name="output">The stream to write the editframe output to. If the value is null the HttpContext Response Stream is used.</param>
        /// <param name="title">The title for the edit frame</param>
        /// <returns>
        /// GlassEditFrame.
        /// </returns>
        GlassEditFrame EditFrame(string title, string buttons, string path = null, TextWriter output= null);


        /// <summary>
        /// Returns an Sitecore Edit Frame using the fields from the specified model.
        /// </summary>
        /// <param name="model">The model of the item to use.</param>
        /// <param name="title">The title to display with the editframe</param>
        /// <param name="fields">The fields to add to the edit frame</param>
        /// <param name="output">The stream to write the editframe output to. If the value is null the HttpContext Response Stream is used.</param>
        /// <returns>
        /// GlassEditFrame.
        /// </returns>
        GlassEditFrame EditFrame<T>(T model, string title = null, TextWriter output= null, params Expression<Func<T, object>>[] fields)
            where T : class;


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
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="predicate">Predicate to determine if the field should be made editable</para>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters">Additional rendering parameters, e.g. ImageParameters</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        string EditableIf<T>(T target, Func<bool> predicate, Expression<Func<T, object>> field, object parameters = null);


        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="predicate">Predicate to determine if the field should be made editable</para>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <param name="parameters">Additional rendering parameters, e.g. ImageParameters</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        string EditableIf<T>(T target, Func<bool> predicate, Expression<Func<T, object>> field,
            Expression<Func<T, string>> standardOutput, object parameters = null);



        /// <summary>
        /// Renders HTML for an image
        /// </summary>
        /// <param name="field">The image to render</param>
        /// <param name="parameters">Additional parameters to add. Do not include alt or src</param>
        /// <param name="outputHeightWidth">Indicates if the height and width attributes should be outputted when rendering the image</param>
        /// <param name="alwaysRender">Renders an A element even if the link is null</param>
        /// <returns>An img HTML element</returns>
        string RenderImage<T>(T model, Expression<Func<T, object>> field, object parameters = null, bool isEditable = false, bool outputHeightWidth = false);

        RenderingResult BeginRenderLink<T>(T model, Expression<Func<T, object>> field, TextWriter writer,
                                      object attributes = null, bool isEditable = false);

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="field">The link to render</param>
        /// <param name="alwaysRender">Renders an A element even if the link is null</param>
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

        string ProtectMediaUrl(string url);

        /// <summary>
        /// Allows you to get a compiled function form the lambda expression. Typically used with extension methods.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Func<T, object> GetCompiled<T>(Expression<Func<T, object>> expression);
        /// <summary>
        /// Allows you to get a compiled function form the lambda expression. Typically used with extension methods.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Func<T, string> GetCompiled<T>(Expression<Func<T, string>> expression);
    }

}
