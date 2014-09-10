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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;
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
        private static readonly Type ImageType = typeof(Fields.Image);
        private static readonly Type LinkType = typeof(Fields.Link );
        public const string Parameters = "Parameters";


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
        /// <param name="parameters">Additional rendering parameters, e.g. ImageParameters</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field, object parameters = null)
        {
            return MakeEditable(field, null, target, parameters);
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
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, object parameters = null)
        {
            
            return MakeEditable(field, standardOutput, target, parameters);
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
                    T obj = item.GlassCast<T>(this.SitecoreContext);

                    item.Editing.EndEdit();
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
            var config = SitecoreContext.GlassContext.GetTypeConfiguration < SitecoreTypeConfiguration>(typeof(T));
            return GetRenderingParameters<T>(parameters, config.TemplateId);
        }


        /// <summary>
        /// Converts rendering parameters to a concrete type. Use this method if you have defined the template ID on the 
        /// model configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual T    GetRenderingParameters<T>(NameValueCollection parameters) where T : class
        {
            var config = SitecoreContext.GlassContext[typeof(T)] as SitecoreTypeConfiguration;

            if (config == null)
            {
                SitecoreContext.GlassContext.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(T)));
            }
            config = SitecoreContext.GlassContext[typeof(T)] as SitecoreTypeConfiguration;

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
        public const string ImageTagFormat = "<img src='{0}' {1}/>";
     
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
                                             object parameters = null,
                                             bool isEditable = false)
        {

            if (parameters is ImageParameters)
            {
                var imageParameters = parameters as ImageParameters;
                if (IsInEditingMode && isEditable)
                {
                    return Editable(model, field, imageParameters);
                }
                else
                {
                    return RenderImage(field.Compile().Invoke(model) as Fields.Image, parameters == null ? null : imageParameters.Parameters);
                }

            }
            else
            {
                var attrs = Utilities.GetPropertiesCollection(parameters, true);

                if (IsInEditingMode && isEditable)
                {
                    var url = new UrlString();
                    url.Parameters.Add(attrs);
                    return Editable(model, field, url.Query);
                }
                else
                {
                    return RenderImage(field.Compile().Invoke(model) as Fields.Image, parameters == null ? null : attrs);
                }
            }
        }

        public virtual RenderingResult BeginRenderLink<T>(T model, Expression<Func<T, object>> field, TextWriter writer, object parameters = null, bool isEditable = false)
        {
            NameValueCollection attrs;

            if (parameters is NameValueCollection)
            {
                attrs = parameters as NameValueCollection;
            }
            else if (parameters is AbstractParameters)
            {
                attrs = ((AbstractParameters) parameters).Parameters;
            }
            else
            {
                attrs = Utilities.GetPropertiesCollection(parameters, true);
            }

            if (IsInEditingMode && isEditable)
            {
                attrs["haschildren"] = "true";

                return MakeEditable(field, null, model, Utilities.ConstructQueryString(attrs), _context, SitecoreContext.Database, writer);
            }
            else
            {
                return BeginRenderLink(field.Compile().Invoke(model) as Fields.Link, attrs, string.Empty, writer);
            }
        }

        


        /// <summary>
        /// Checks it and attribute is part of the NameValueCollection and updates it with the
        /// default if it isn't.
        /// </summary>
        /// <param name="collection">The collection of parameters</param>
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
        /// <param name="attributes">A collection of parameters to added to the link</param>
        /// <param name="isEditable">Indicate if the link should be editable in the page editor</param>
        /// <param name="contents">Content to go in the link</param>
        /// <returns>An "a" HTML element</returns>
        public virtual string RenderLink<T>(T model, Expression<Func<T, object>> field, object attributes = null, bool isEditable = false, string contents = null)
        {
            NameValueCollection attrs = null;

            if (attributes is NameValueCollection)
            {
                attrs = attributes as NameValueCollection;
            }
            else if (attributes is AbstractParameters)
            {
                attrs = ((AbstractParameters) attributes).Parameters;
            }
            else
            {
                attrs = Utilities.GetPropertiesCollection(attributes, true);
                
            }

            var sb = new StringBuilder();
            var writer = new StringWriter(sb);
            var linkField = field.Compile().Invoke(model) as Fields.Link;

            RenderingResult result = null;
            if (IsInEditingMode && isEditable)
            {
                if (!string.IsNullOrEmpty(contents))
                    attrs["haschildren"] = "true";
                if (contents.IsNotNullOrEmpty())
                {
                    attrs.Add("haschildren", "true");
                }

                if (linkField != null)
                {
                    AttributeCheck(attrs, "class", linkField.Class);
                    AttributeCheck(attrs, "title", linkField.Title);
                }

                result = MakeEditable(
                    field,
                    null, 
                    model,
                    Utilities.ConstructQueryString(attrs), 
                    _context, SitecoreContext.Database, writer);

                if (contents.IsNotNullOrEmpty())
                {
                    sb.Append(contents);
            }
            }
            else
            {
                result = BeginRenderLink(
                        field.Compile().Invoke(model) as Fields.Link, attrs, contents, writer
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

        private string MakeEditable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target, object parameters)
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
        /// <param name="attributes">Addtiional parameters to add. Do not include href or title</param>
        /// <param name="contents">Content to go in the link instead of the standard text</param>
        /// <returns>An "a" HTML element</returns>
        public static RenderingResult BeginRenderLink(Fields.Link link, NameValueCollection attributes, string contents, TextWriter writer)
        {
            if (link == null) return new RenderingResult(writer, string.Empty, string.Empty);
            if (attributes == null) attributes = new NameValueCollection();

            string format = "<a href='{0}' {1}>{2}";

            contents = contents == null ? link.Text ?? link.Title : contents;
            
            AttributeCheck(attributes, "class", link.Class);
            AttributeCheck(attributes, "target", link.Target);
            AttributeCheck(attributes, "title", link.Title);

            string firstPart = format.Formatted(link.BuildUrl(attributes), Utilities.ConvertAttributes(attributes), contents);
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
        private RenderingResult MakeEditable<T>(
            Expression<Func<T, object>> field, 
            Expression<Func<T, string>> standardOutput, 
            T model, 
            object parameters, 
            Context context, Database database,
            TextWriter writer)
        {

            string firstPart = string.Empty;
            string lastPart = string.Empty;

            try
            {
                if (field == null) throw new NullReferenceException("No field set");
                if (model == null) throw new NullReferenceException("No model set");

                string parametersString = string.Empty;

                if (parameters == null)
                {
                    parametersString = string.Empty;
                }
                else if (parameters is string)
                {
                    parametersString = parameters as string;
                }
                else if (parameters is AbstractParameters)
                {
                    parametersString = ((AbstractParameters)parameters).ToString();
                }
                else if (parameters is NameValueCollection)
                {
                    parametersString = Utilities.ConstructQueryString(parameters as NameValueCollection);
                }
                else
                {
                    NameValueCollection attrs = Utilities.GetPropertiesCollection(parameters, true);
                    parametersString = Utilities.ConstructQueryString(attrs);
                }


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

                    var config = context.GetTypeConfiguration<SitecoreTypeConfiguration>(finalTarget);

                  

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

                        var fieldConfig = (SitecoreFieldConfiguration) dataHandler;
                        if (fieldConfig.FieldId != (Sitecore.Data.ID)null && fieldConfig.FieldId != ID.Null)
                        {
                            renderFieldArgs.FieldName = fieldConfig.FieldId.ToString();
                        }
                        else
                        {
                            renderFieldArgs.FieldName = fieldConfig.FieldName;
                        }

                        renderFieldArgs.Parameters = WebUtil.ParseQueryString(parametersString ?? string.Empty, true);
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
                    {
                        var type = field.Body.Type;
                        object target = (field.Compile().Invoke(model) ?? string.Empty);

                        if (type == ImageType)
                        {
                            var image = target as Image;
                            firstPart  = RenderImage(image, WebUtil.ParseUrlParameters(parametersString));
                        }
                        else if (type == LinkType)
                        {
                            var link = target as Link;
                            var sb = new StringBuilder();
                            var linkWriter = new StringWriter(sb);
                            var result = BeginRenderLink(link, WebUtil.ParseUrlParameters(parametersString),null, linkWriter);
                            result.Dispose();
                            linkWriter.Flush();
                            linkWriter.Close();
                            
                            firstPart = sb.ToString();

                        }
                        else
                        {
                            firstPart = target.ToString();
                        }
                    }
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
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection parameters = null, bool isEditable = false, string contents = null)")]
        public virtual string RenderLink(Fields.Link link)
        {

            return RenderLink(link, null, string.Empty);

        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional parameters to add. Do not include href or title</param>
        /// <returns>An "a" HTML element</returns>
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection parameters = null, bool isEditable = false, string contents = null)")]
        public virtual string RenderLink(Fields.Link link, NameValueCollection attributes)
        {

            return RenderLink(link, attributes, string.Empty);

        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional parameters to add. Do not include href or title</param>
        /// <param name="contents">Content to go in the link instead of the standard text</param>
        /// <returns>An "a" HTML element</returns>
        [Obsolete("Use RenderLink<T>(T model, Expression<Func<T, object>> field, NameValueCollection parameters = null, bool isEditable = false, string contents = null)")]
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
            return MakeEditable<T>(field, null, target, string.Empty);
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
            return MakeEditable<T>(field, standardOutput, target, string.Empty);
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
        /// <param name="attributes">Additional parameters to add. Do not include alt or src</param>
        /// <returns>An img HTML element</returns>
        [Obsolete(
            "Use RenderImage<T>(T model, Expression<Func<T, object>> field, ImageParameters parameters = null, bool isEditable = false)"
            )]
        public virtual string RenderImage(Fields.Image image, NameValueCollection attributes)
        {

            var urlParams = new NameValueCollection();
            var htmlParams = new NameValueCollection();

            /*
             * ME - This method is used to render images rather than going back to the fieldrender
             * because it stops another call having to be passed to Sitecore.
             */

            if (image == null || image.Src.IsNullOrWhiteSpace()) return "";

            if (attributes == null) attributes = new NameValueCollection();

            Action<string> remove = key => attributes.Remove(key);
            Action<string> url = key =>
            {
                urlParams.Add(key, attributes[key]);
                remove(key);
            };
            Action<string> html = key =>
            {
                htmlParams.Add(key, attributes[key]);
                remove(key);
            };
            Action<string> both = key =>
            {
                htmlParams.Add(key, attributes[key]);
                urlParams.Add(key, attributes[key]);
                remove(key);
            };

            foreach (var key in attributes.AllKeys)
            {
                switch (key)
                {
                    case ImageParameters.BORDER:
                    case ImageParameters.ALT:
                    case ImageParameters.HSPACE:
                    case ImageParameters.VSPACE:
                    case ImageParameters.CLASS:
                        html(key);
                        break;
                    case ImageParameters.OUTPUT_METHOD:
                    case ImageParameters.ALLOW_STRETCH:
                    case ImageParameters.IGNORE_ASPECT_RATIO:
                    case ImageParameters.SCALE:
                    case ImageParameters.MAX_WIDTH:
                    case ImageParameters.MAX_HEIGHT:
                    case ImageParameters.THUMBNAIL:
                    case ImageParameters.BACKGROUND_COLOR:
                    case ImageParameters.DATABASE:
                    case ImageParameters.LANGUAGE:
                    case ImageParameters.VERSION:
                    case ImageParameters.DISABLE_MEDIA_CACHE:
                        url(key);
                        break;
                    case ImageParameters.WIDTH:
                    case ImageParameters.HEIGHT:
                        both(key);
                        break;
                    default:
                        both(key);
                        break;
                }
            }

            var builder = new UrlBuilder(image.Src);

            foreach (var key in urlParams.AllKeys)
            {
                builder[key] = urlParams[key];
            }

            //should there be some warning about these removals?
            AttributeCheck(htmlParams, ImageParameters.CLASS, image.Class);
            AttributeCheck(htmlParams, ImageParameters.ALT, image.Alt);
            AttributeCheck(htmlParams, ImageParameters.BORDER, image.Border);
            if(image.HSpace >0)
                AttributeCheck(htmlParams, ImageParameters.HSPACE, image.HSpace.ToString(CultureInfo.InvariantCulture));
            if(image.VSpace >0)
                AttributeCheck(htmlParams, ImageParameters.VSPACE, image.VSpace.ToString(CultureInfo.InvariantCulture));

            if (htmlParams.AllKeys.Any(x => x == ImageParameters.HEIGHT))
            {
                htmlParams["height"] = htmlParams[ImageParameters.HEIGHT];
                htmlParams.Remove(ImageParameters.HEIGHT);
            }

            if (htmlParams.AllKeys.Any(x => x == ImageParameters.WIDTH))
            {
                htmlParams["width"] = htmlParams[ImageParameters.WIDTH];
                htmlParams.Remove(ImageParameters.WIDTH);
            }

            return ImageTagFormat.Formatted(builder.ToString(), Utilities.ConvertAttributes(htmlParams));
        }

        #endregion

    }
}




