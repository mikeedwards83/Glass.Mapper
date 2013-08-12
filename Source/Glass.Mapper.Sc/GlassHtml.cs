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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.RenderField;
using Glass.Mapper.Sc.Web.Ui;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using Sitecore.Pipelines.RenderField;
using Sitecore.SecurityModel;
using Sitecore.Text;
using Sitecore.Web;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// This class contains a set of helpers that make converting items mapped in Glass.Sitecore.Mapper to HTML
    /// </summary>
    public class GlassHtml : IGlassHtml
    {
        /// <summary>
        /// Gets the sitecore context.
        /// </summary>
        /// <value>
        /// The sitecore context.
        /// </value>
        public  ISitecoreContext SitecoreContext { get; private set; }
        private readonly Context _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassHtml"/> class.
        /// </summary>
        /// <param name="sitecoreContext">The service that will be used to load and save data</param>
        public GlassHtml(ISitecoreContext sitecoreContext)
        {
            SitecoreContext = sitecoreContext;
            _context = sitecoreContext.GlassContext;
        }


        /// <summary>
        /// Edits the frame.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        /// GlassEditFrame.
        /// </returns>
        public GlassEditFrame EditFrame(string buttons, string path = null)
        {
            var frame = new GlassEditFrame(buttons, HttpContext.Current.Response.Output, path);
            frame.RenderFirstPart();
            return frame;
        }


        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The model object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field)
        {
            return MakeEditable(field, null, target);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The model object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters">Additional rendering parameters, e.g. ImageParameters</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field, AbstractParameters parameters)
        {
            return MakeEditable(field, null, target, parameters);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The model object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters">Additional rendering parameters, e.g. class=myCssClass</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field, string parameters)
        {
            return MakeEditable(field, null, target, parameters);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor.  Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The model object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)
        {
            return MakeEditable(field, standardOutput, target);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The model object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <param name="parameters">Additional rendering parameters, e.g. ImageParameters</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, AbstractParameters parameters)
        {
            return MakeEditable(field, null, target, parameters);
        }


        public virtual T GetRenderingParameters<T>(string parameters, ID renderParametersTemplateId) where T : class
        {
            var nameValueCollection = WebUtil.ParseUrlParameters(parameters);
            return GetRenderingParameters<T>(nameValueCollection, renderParametersTemplateId);
        }
        public T GetRenderingParameters<T>(NameValueCollection parameters, ID renderParametersTemplateId) where T:class{

            var item = Utilities.CreateFakeItem(null, renderParametersTemplateId, SitecoreContext.Database, "renderingParameters");

            using (new SecurityDisabler() )
            {
                using (new VersionCountDisabler())
                {
                    item.Editing.BeginEdit();

                    foreach (var key in parameters.AllKeys)
                    {
                        item[key] = parameters[key];
                    }
                    T obj = item.GlassCast<T>();

                    item.Editing.CancelEdit();
                    return obj;
                }
            }

        }


        /// <summary>
        /// Converts rendering parameters to a concrete type. Use this method if you have defined the template ID on the 
        /// model configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual T GetRenderingParameters<T>(string parameters) where T : class
        {
            var config = SitecoreContext.GlassContext[typeof (T)] as SitecoreTypeConfiguration;
            return GetRenderingParameters<T>(parameters, config.TemplateId);
        }


        /// <summary>
        /// Converts rendering parameters to a concrete type. Use this method if you have defined the template ID on the 
        /// model configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual T GetRenderingParameters<T>(NameValueCollection parameters) where T : class
        {
            var config = SitecoreContext.GlassContext[typeof(T)] as SitecoreTypeConfiguration;
            return GetRenderingParameters<T>(parameters, config.TemplateId);
        }

        /// <summary>
        /// The image width
        /// </summary>
        public const string ImageWidth = "width";
        /// <summary>
        /// The image height
        /// </summary>
        public const string ImageHeight = "height";
     
    
        /// <summary>
        /// The image tag format
        /// </summary>
        public const string ImageTagFormat = "<img src='{0}' {1} />";
     
       /// <summary>
       /// Renders an image allowing simple page editor support
       /// </summary>
       /// <typeparam name="T">The model type</typeparam>
       /// <param name="model">The model that contains the image field</param>
       /// <param name="field">A lambda expression to the image field, should be of type Glass.Mapper.Sc.Fields.Image</param>
       /// <param name="parameters">Image parameters, e.g. width, height</param>
       /// <param name="isEditable">Indicates if the field should be editable</param>
       /// <returns></returns>
        public virtual string RenderImage<T>(T model,
                                             Expression<Func<T, object>> field, 
                                             ImageParameters parameters = null,
                                             bool isEditable = false)
        {
            if (IsInEditingMode && isEditable)
            {
                return Editable(model, field, parameters);
            }
            else
            {
                return RenderImage(field.Compile().Invoke(model) as Fields.Image, parameters==null ? null : parameters.Parameters);
            }
        }

        public virtual RenderingResult BeginRenderLink<T>(T model, Expression<Func<T, object>> field, TextWriter writer, NameValueCollection attributes = null, bool isEditable = false)
        {
            if (IsInEditingMode && isEditable)
            {
                return MakeEditable(field, null, model, "haschildren=true", _context, SitecoreContext.Database, writer);
            }
            else
            {
                return BeginRenderLink(field.Compile().Invoke(model) as Fields.Link, attributes, string.Empty, writer);
            }
        }

        


        /// <summary>
        /// Checks it and attribute is part of the NameValueCollection and updates it with the
        /// default if it isn't.
        /// </summary>
        /// <param name="collection">The collection of attributes</param>
        /// <param name="name">The name of the attribute in the collection</param>
        /// <param name="defaultValue">The default value for the attribute</param>
        public static void AttributeCheck(NameValueCollection collection, string name, string defaultValue)
        {
            if (collection[name].IsNullOrEmpty() && !defaultValue.IsNullOrEmpty())
                collection[name] = defaultValue;
        }


        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="model">The model containing the link</param>
        /// <param name="field">An expression that points to the link</param>
        /// <param name="attributes">A collection of attributes to added to the link</param>
        /// <param name="isEditable">Indicate if the link should be editable in the page editor</param>
        /// <param name="contents">Content to go in the link</param>
        /// <returns>An "a" HTML element</returns>
        public virtual string RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection attributes = null, bool isEditable = false, string contents = null)
        {
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            RenderingResult result = null;
            if (IsInEditingMode && isEditable)
            {
                
                result = MakeEditable(
                    field, 
                    null, 
                    model,  
                    contents == null ? string.Empty: "haschildren=true", 
                    _context, SitecoreContext.Database, writer);
            }
            else
            {
                result = BeginRenderLink(
                        field.Compile().Invoke(model) as Fields.Link, attributes, contents, writer
                    );
            }

            result.Dispose();
            writer.Flush();
            writer.Close();
            return sb.ToString();

        }

   
        /// <summary>
        /// Indicates if the site is in editing mode
        /// </summary>
        /// <value><c>true</c> if this instance is in editing mode; otherwise, <c>false</c>.</value>
        public static bool IsInEditingMode
        {
            get
            {
                return
                            global::Sitecore.Context.PageMode.IsPageEditorEditing;
            }
        }




        /// <summary>
        /// Makes the editable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="target">The model.</param>
        /// <returns>System.String.</returns>
        private string MakeEditable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target)
        {
            return MakeEditable(field, standardOutput, target, string.Empty);
        }

        /// <summary>
        /// Makes the editable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="target">The model.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        private string MakeEditable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target, AbstractParameters parameters)
        {
            var parametersString = parameters == null ? string.Empty : parameters.ToString();
            return MakeEditable<T>(field, standardOutput, target, parametersString);
        }



        private string MakeEditable<T>(Expression<Func<T, object>> field,
                                                Expression<Func<T, string>> standardOutput, T target, string parameters)
        {
            StringBuilder sb = new StringBuilder();
            var writer = new StringWriter(sb);
            var result = MakeEditable(field, standardOutput, target, parameters, _context, SitecoreContext.Database, writer);
            result.Dispose();
            writer.Flush();
            writer.Close();
            return sb.ToString();
        }

        #region Statics

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional attributes to add. Do not include href or title</param>
        /// <param name="contents">Content to go in the link instead of the standard text</param>
        /// <returns>An "a" HTML element</returns>
        public static RenderingResult BeginRenderLink(Fields.Link link, NameValueCollection attributes, string contents, TextWriter writer)
        {
            if (link == null) return new RenderingResult(writer, string.Empty, string.Empty);
            if (attributes == null) attributes = new NameValueCollection();

            string format = "<a href='{0}{1}' title='{2}' model='{3}' class='{4}' {5}>{6}";

            string cls = attributes.AllKeys.Any(x => x == "class") ? attributes["class"] : link.Class;
            string anchor = link.Anchor.IsNullOrEmpty() ? "" : "#" + link.Anchor;
            string target = attributes.AllKeys.Any(x => x == "model") ? attributes["model"] : link.Target;


            contents = contents == null ? link.Text ?? link.Title : contents;

            AttributeCheck(attributes, "class", link.Class);
            AttributeCheck(attributes, "model", link.Target);
            AttributeCheck(attributes, "title", link.Title);

            attributes.Remove("href");

            string firstPart = format.Formatted(link.Url, anchor, link.Title, target, cls, Utilities.ConvertAttributes(attributes), contents);
            string lastPart = "</a>";
            return new RenderingResult(writer, firstPart, lastPart);
        }


        /// <summary>
        /// Makes the editable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="model">The model.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="Glass.Mapper.MapperException">
        /// To many parameters in linq expression {0}.Formatted(field.Body)
        /// or
        /// Expression doesn't evaluate to a member {0}.Formatted(field.Body)
        /// or
        /// Page editting error. Could not find property {0} on type {1}.Formatted(memberExpression.Member.Name, config.Type.FullName)
        /// or
        /// Page editting error. Could not find data handler for property {2} {0}.{1}.Formatted(
        ///                         prop.DeclaringType, prop.Name, prop.MemberType)
        /// </exception>
        /// <exception cref="System.NullReferenceException">Context cannot be null</exception>
        private static RenderingResult MakeEditable<T>(
            Expression<Func<T, object>> field, 
            Expression<Func<T, string>> standardOutput, 
            T model, 
            string parameters, 
            Context context, Database database,
            TextWriter writer)
        {

            string firstPart = string.Empty;
            string lastPart = string.Empty;

            try
            {
                if (field == null) throw new NullReferenceException("No field set");
                if (model == null) throw new NullReferenceException("No model set");




                if (IsInEditingMode)
                {
                    if (field.Parameters.Count > 1)
                        throw new MapperException("To many parameters in linq expression {0}".Formatted(field.Body));

                    MemberExpression memberExpression;

                    if (field.Body is UnaryExpression)
                    {
                        memberExpression = ((UnaryExpression) field.Body).Operand as MemberExpression;
                    }
                    else if (!(field.Body is MemberExpression))
                    {
                        throw new MapperException("Expression doesn't evaluate to a member {0}".Formatted(field.Body));
                    }
                    else
                    {
                        memberExpression = (MemberExpression) field.Body;
                    }



                    //we have to deconstruct the lambda expression to find the 
                    //correct model object
                    //For example if we have the lambda expression x =>x.Children.First().Content
                    //we have to evaluate what the first Child object is, then evaluate the field to edit from there.

                    //this contains the expression that will evaluate to the object containing the property
                    var objectExpression = memberExpression.Expression;

                    var finalTarget =
                        Expression.Lambda(objectExpression, field.Parameters).Compile().DynamicInvoke(model);

                    var site = global::Sitecore.Context.Site;

                    if (context == null)
                        throw new NullReferenceException("Context cannot be null");

                    var config = context.GetTypeConfiguration(finalTarget) as SitecoreTypeConfiguration;

                  

                    var scClass = config.ResolveItem(finalTarget, database);

                    //lambda expression does not always return expected memberinfo when inheriting
                    //c.f. http://stackoverflow.com/questions/6658669/lambda-expression-not-returning-expected-memberinfo
                    var prop = config.Type.GetProperty(memberExpression.Member.Name);

                    //interfaces don't deal with inherited properties well
                    if (prop == null && config.Type.IsInterface)
                    {
                        Func<Type, PropertyInfo> interfaceCheck = null;
                        interfaceCheck = (inter) =>
                            {
                                var interfaces = inter.GetInterfaces();
                                var properties =
                                    interfaces.Select(x => x.GetProperty(memberExpression.Member.Name)).Where(
                                        x => x != null);
                                if (properties.Any()) return properties.First();
                                else
                                    return interfaces.Select(x => interfaceCheck(x)).FirstOrDefault(x => x != null);
                            };
                        prop = interfaceCheck(config.Type);
                    }

                    if (prop != null && prop.DeclaringType != prop.ReflectedType)
                    {
                        //properties mapped in data handlers are based on declaring type when field is inherited, make sure we match
                        prop = prop.DeclaringType.GetProperty(prop.Name);
                    }

                    if (prop == null)
                        throw new MapperException(
                            "Page editting error. Could not find property {0} on type {1}".Formatted(
                                memberExpression.Member.Name, config.Type.FullName));

                    //ME - changed this to work by name because properties on interfaces do not show up as declared types.
                    var dataHandler = config.Properties.FirstOrDefault(x => x.PropertyInfo.Name == prop.Name);
                    if (dataHandler == null)
                    {
                        throw new MapperException(
                            "Page editting error. Could not find data handler for property {2} {0}.{1}".Formatted(
                                prop.DeclaringType, prop.Name, prop.MemberType));
                    }



                    using (new ContextItemSwitcher(scClass))
                    {
                        RenderFieldArgs renderFieldArgs = new RenderFieldArgs();
                        renderFieldArgs.Item = scClass;
                        renderFieldArgs.FieldName = ((SitecoreFieldConfiguration) dataHandler).FieldName;

                        renderFieldArgs.Parameters = WebUtil.ParseQueryString(parameters ?? string.Empty);
                        renderFieldArgs.DisableWebEdit = false;

                        CorePipeline.Run("renderField", (PipelineArgs) renderFieldArgs);

                        firstPart = renderFieldArgs.Result.FirstPart;
                        lastPart = renderFieldArgs.Result.LastPart;

                    }
                }
                else
                {
                    if (standardOutput != null)
                        firstPart = standardOutput.Compile().Invoke(model);
                    else
                        firstPart = (field.Compile().Invoke(model) ?? string.Empty).ToString();
                }
            }
            catch (Exception ex)
            {
                firstPart = "<p>{0}</p><pre>{1}</pre>".Formatted(ex.Message, ex.StackTrace);
                Sitecore.Diagnostics.Log.Error("Failed to render field", ex, typeof(IGlassHtml));
            }

            return new RenderingResult(writer, firstPart, lastPart);


            //return field.Compile().Invoke(model).ToString();
        }

        #endregion


        #region Obsolete


        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <returns>An "a" HTML element</returns>
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection attributes = null, bool isEditable = false, string contents = null)")]
        public virtual string RenderLink(Fields.Link link)
        {

            return RenderLink(link, null, string.Empty);

        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional attributes to add. Do not include href or title</param>
        /// <returns>An "a" HTML element</returns>
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection attributes = null, bool isEditable = false, string contents = null)")]
        public virtual string RenderLink(Fields.Link link, NameValueCollection attributes)
        {

            return RenderLink(link, attributes, string.Empty);

        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional attributes to add. Do not include href or title</param>
        /// <param name="contents">Content to go in the link instead of the standard text</param>
        /// <returns>An "a" HTML element</returns>
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection attributes = null, bool isEditable = false, string contents = null)")]
        public virtual string RenderLink(Fields.Link link, NameValueCollection attributes, string contents)
        {
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            BeginRenderLink(link, attributes, contents, writer);
            writer.Flush();
            writer.Close();
            return sb.ToString();
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The model object that contains the item to be edited</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        [Obsolete("Use Editable<T>(T model, Expression<Func<T, object>> field)")]
        public string Editable<T>(Expression<Func<T, object>> field, T target)
        {
            return MakeEditable<T>(field, null, target);
        }
        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor.  Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <param name="target">The model object that contains the item to be edited</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        [Obsolete("Use Editable<T>(T model, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)")]
        public string Editable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target)
        {
            return MakeEditable<T>(field, standardOutput, target);
        }

        /// <summary>
        /// Renders HTML for an image
        /// </summary>
        /// <param name="image">The image to render</param>
        /// <returns>An img HTML element</returns>
        [Obsolete("Use RenderImage<T>(T model, Expression<Func<T, object>> field, ImageParameters parameters = null, bool isEditable = false)")]
        public virtual string RenderImage(Fields.Image image)
        {
            return RenderImage(image, null);
        }

        /// <summary>
        /// Renders HTML for an image
        /// </summary>
        /// <param name="image">The image to render</param>
        /// <param name="attributes">Additional attributes to add. Do not include alt or src</param>
        /// <returns>An img HTML element</returns>
        [Obsolete("Use RenderImage<T>(T model, Expression<Func<T, object>> field, ImageParameters parameters = null, bool isEditable = false)")]
        public virtual string RenderImage(Fields.Image image, NameValueCollection attributes)
        {

            /*
             * ME - This method is used to render images rather than going back to the fieldrender
             * because it stops another call having to be passed to Sitecore.
             */

            if (image == null || image.Src.IsNullOrWhiteSpace()) return "";

            if (attributes == null) attributes = new NameValueCollection();


            var builder = new UrlBuilder(image.Src);

            //append to url values
            if (attributes[ImageWidth].IsNotNullOrEmpty())
                attributes.Add(ImageParameters.WIDTH, attributes[ImageWidth]);
            else
                attributes.Add(ImageParameters.WIDTH, image.Width.ToString());

            if (attributes[ImageHeight].IsNotNullOrEmpty())
                attributes.Add(ImageParameters.HEIGHT, attributes[ImageHeight]);
            else
                attributes.Add(ImageParameters.HEIGHT, image.Height.ToString());

            foreach (var key in attributes.AllKeys)
            {
                if (key == "alt" || key == "class" || key == "style")
                    continue;

                builder[key] = attributes[key];
            }

            //should there be some warning about these removals?
            AttributeCheck(attributes, "class", image.Class);
            AttributeCheck(attributes, "alt", image.Alt);


            return ImageTagFormat.Formatted(builder.ToString(), Utilities.ConvertAttributes(attributes));
        }

        #endregion

    }
}




