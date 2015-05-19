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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.Pipelines.GetChromeData;
using Glass.Mapper.Sc.RenderField;
using Glass.Mapper.Sc.Web.Ui;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using Sitecore.Pipelines.RenderField;
using Sitecore.Platform;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;
using Sitecore.Text;
using Sitecore.Web;
using Image = Glass.Mapper.Sc.Fields.Image;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// This class contains a set of helpers that make converting items mapped in Glass.Sitecore.Mapper to HTML
    /// </summary>
    public class GlassHtml : IGlassHtml
    {
        private static readonly Type ImageType = typeof(Fields.Image);
        private static readonly Type LinkType = typeof(Fields.Link);
        private static ConcurrentDictionary<string, object> _compileCache = new ConcurrentDictionary<string, object>();



        public const string Parameters = "Parameters";
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
        public static string ImageTagFormat = "<img src={2}{0}{2} {1}/>";
        public static string LinkTagFormat = "<a href={3}{0}{3} {1}>{2}";
        public static string QuotationMark = "'";

        protected Func<T, string> GetCompiled<T>(Expression<Func<T, string>> expression)
        {
            if (!SitecoreContext.Config.UseGlassHtmlLambdaCache)
            {
                return expression.Compile();
            }

            var key = typeof(T).FullName + expression.Body.ToString();

            if (_compileCache.ContainsKey(key))
            {
                return (Func<T, string>)_compileCache[key];
            }

            var compiled = expression.Compile();
            _compileCache.TryAdd(key, compiled);
            return compiled;
        }

        protected Func<T, object> GetCompiled<T>(Expression<Func<T, object>> expression)
        {
            if (SitecoreContext.Config == null || !SitecoreContext.Config.UseGlassHtmlLambdaCache)
            {
                return expression.Compile();
            }

            var key = typeof(T).FullName + expression.Body.ToString();

            if (_compileCache.ContainsKey(key))
            {
                return (Func<T, object>)_compileCache[key];
            }
            var compiled = expression.Compile();
            _compileCache.TryAdd(key, compiled);
            return compiled;
        }


        /// <summary>
        /// Gets the sitecore context.
        /// </summary>
        /// <value>
        /// The sitecore context.
        /// </value>
        public ISitecoreContext SitecoreContext { get; private set; }
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
        public GlassEditFrame EditFrame(string buttons, string path = null, TextWriter output = null)
        {
            if (output == null)
            {
                output = HttpContext.Current.Response.Output;
            }
            var frame = new GlassEditFrame(buttons, output, path);
            frame.RenderFirstPart();
            return frame;
        }


        public GlassEditFrame EditFrame<T>(T model, string title = null, TextWriter output = null, params Expression<Func<T, object>>[] fields) where T : class
        {
            if (IsInEditingMode && model != null)
            {
                if (fields.Any())
                {
                    var fieldNames = fields.Select(x => Glass.Mapper.Utilities.GetGlassProperty<T, SitecoreTypeConfiguration>(x, this.SitecoreContext.GlassContext, model))
                        .Cast<SitecoreFieldConfiguration>()
                        .Where(x => x != null)
                        .Select(x => x.FieldName);

                    var buttonPath = "{0}{1}".Formatted(
                        EditFrameBuilder.BuildToken,
                        fieldNames.Aggregate((x, y) => x + "|" + y));

                    if (title.IsNotNullOrEmpty())
                    {
                        buttonPath += "<title>{0}<title>".Formatted(title);
                    }

                    var field = fields.FirstOrDefault();


                    var config = Glass.Mapper.Utilities.GetTypeConfig<T, SitecoreTypeConfiguration>(field, SitecoreContext.GlassContext, model);
                    var pathConfig = config.Properties
                        .OfType<SitecoreInfoConfiguration>()
                        .FirstOrDefault(x => x.Type == SitecoreInfoType.Path);

                    var path = string.Empty;

                    if (pathConfig == null)
                    {
                        var id = config.GetId(model);
                        var item = SitecoreContext.Database.GetItem(id);
                        path = item.Paths.Path;

                    }
                    else
                    {
                        path = pathConfig.PropertyGetter(model) as string;
                    }


                    return EditFrame(buttonPath, path, output);
                }
            }
            return EditFrame("/sitecore");

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
        public T GetRenderingParameters<T>(NameValueCollection parameters, ID renderParametersTemplateId) where T : class
        {

            var item = Utilities.CreateFakeItem(null, renderParametersTemplateId, SitecoreContext.Database, "renderingParameters");

            using (new SecurityDisabler())
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
            var nameValueCollection = WebUtil.ParseUrlParameters(parameters);
            return GetRenderingParameters<T>(nameValueCollection);
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

            if (config == null)
            {
                SitecoreContext.GlassContext.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(T)));
            }
            config = SitecoreContext.GlassContext[typeof(T)] as SitecoreTypeConfiguration;

            return GetRenderingParameters<T>(parameters, config.TemplateId);
        }




        public virtual RenderingResult BeginRenderLink<T>(T model, Expression<Func<T, object>> field, TextWriter writer, object parameters = null, bool isEditable = false)
        {
            NameValueCollection attrs;

            if (parameters is NameValueCollection)
            {
                attrs = parameters as NameValueCollection;
            }
            else
            {
                attrs = Utilities.GetPropertiesCollection(parameters, true);
            }

            if (IsInEditingMode && isEditable)
            {

                if (attrs != null)
                {
                    attrs.Add("haschildren", "true");
                    return MakeEditable(field, null, model, attrs, _context, SitecoreContext.Database, writer);
                }
                return MakeEditable(field, null, model, "haschildren=true", _context, SitecoreContext.Database, writer);
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
        public static void AttributeCheck(SafeDictionary<string> collection, string name, string defaultValue)
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
            else
            {
                attrs = Utilities.GetPropertiesCollection(attributes, true);

            }

            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            RenderingResult result = null;
            if (IsInEditingMode && isEditable)
            {
                if (!string.IsNullOrEmpty(contents))
                    attrs["haschildren"] = "true";
                if (contents.IsNotNullOrEmpty())
                {
                    attrs.Add("haschildren", "true");
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
                        GetCompiled(field).Invoke(model) as Fields.Link, attrs, contents, writer
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
        [Obsolete("Use the SafeDictionary Overload")]
        public static RenderingResult BeginRenderLink(Fields.Link link, NameValueCollection attributes, string contents, TextWriter writer)
        {
            return BeginRenderLink(link, attributes.ToSafeDictionary(), contents, writer);
        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional parameters to add. Do not include href or title</param>
        /// <param name="contents">Content to go in the link instead of the standard text</param>
        /// <returns>An "a" HTML element</returns>
        public static RenderingResult BeginRenderLink(Fields.Link link, SafeDictionary<string> attributes, string contents,
            TextWriter writer)
        {
            if (link == null) return new RenderingResult(writer, string.Empty, string.Empty);
            if (attributes == null) attributes = new SafeDictionary<string>();


            contents = contents == null ? link.Text ?? link.Title : contents;

            AttributeCheck(attributes, "class", link.Class);
            AttributeCheck(attributes, "target", link.Target);
            AttributeCheck(attributes, "title", link.Title);

            string firstPart = LinkTagFormat.Formatted(link.BuildUrl(attributes), Utilities.ConvertAttributes(attributes, QuotationMark), contents, QuotationMark);
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

                string parametersStringTemp = string.Empty;

                SafeDictionary<string> dictionary = new SafeDictionary<string>();

                if (parameters == null)
                {
                    parametersStringTemp = string.Empty;
                }
                else if (parameters is string)
                {
                    parametersStringTemp = parameters as string;
                    dictionary = WebUtil.ParseQueryString(parametersStringTemp ?? string.Empty);
                }
                else if (parameters is NameValueCollection)
                {
                    var collection = (NameValueCollection)parameters;
                    foreach (var key in collection.AllKeys)
                    {
                        dictionary.Add(key, collection[key]);
                    }
                }
                else
                {
                    var collection = Utilities.GetPropertiesCollection(parameters, true);
                    foreach (var key in collection.AllKeys)
                    {
                        dictionary.Add(key, collection[key]);
                    }
                }


                if (IsInEditingMode)
                {
                    MemberExpression memberExpression;
                    var finalTarget = Mapper.Utilities.GetTargetObjectOfLamba(field, model, out memberExpression);
                    var config = Mapper.Utilities.GetTypeConfig<T, SitecoreTypeConfiguration>(field, context, model);
                    var dataHandler = Mapper.Utilities.GetGlassProperty<T, SitecoreTypeConfiguration>(field, context, model);

                    var scClass = config.ResolveItem(finalTarget, database);

                    using (new ContextItemSwitcher(scClass))
                    {
                        RenderFieldArgs renderFieldArgs = new RenderFieldArgs();
                        renderFieldArgs.Item = scClass;

                        var fieldConfig = (SitecoreFieldConfiguration)dataHandler;
                        if (fieldConfig.FieldId != (Sitecore.Data.ID)null && fieldConfig.FieldId != ID.Null)
                        {
                            renderFieldArgs.FieldName = fieldConfig.FieldId.ToString();
                        }
                        else
                        {
                            renderFieldArgs.FieldName = fieldConfig.FieldName;
                        }

                        renderFieldArgs.Parameters = dictionary;
                        renderFieldArgs.DisableWebEdit = false;

                        CorePipeline.Run("renderField", (PipelineArgs)renderFieldArgs);

                        firstPart = renderFieldArgs.Result.FirstPart;
                        lastPart = renderFieldArgs.Result.LastPart;

                    }
                }
                else
                {
                    if (standardOutput != null)
                    {
                        firstPart = GetCompiled<T>(standardOutput)(model).ToString();
                    }
                    else
                    {
                        var type = field.Body.Type;
                        object target = (GetCompiled<T>(field)(model) ?? string.Empty);

                        if (type == ImageType)
                        {
                            var image = target as Image;
                            firstPart = RenderImage(image, dictionary);
                        }
                        else if (type == LinkType)
                        {
                            var link = target as Link;
                            var sb = new StringBuilder();
                            var linkWriter = new StringWriter(sb);
                            var result = BeginRenderLink(link, dictionary, null, linkWriter);
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

        /// <summary>
        /// Renders an image allowing simple page editor support
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="model">The model that contains the image field</param>
        /// <param name="field">A lambda expression to the image field, should be of type Glass.Mapper.Sc.Fields.Image</param>
        /// <param name="parameters">Image parameters, e.g. width, height</param>
        /// <param name="isEditable">Indicates if the field should be editable</param>
        /// <param name="outputHeightWidth">Indicates if the height and width attributes should be output when rendering the image</param>
        /// <returns></returns>
        public virtual string RenderImage<T>(T model,
            Expression<Func<T, object>> field,
            object parameters = null,
            bool isEditable = false,
            bool outputHeightWidth = false)
        {

            var attrs = Utilities.GetPropertiesCollection(parameters, true).ToSafeDictionary();

            if (IsInEditingMode && isEditable)
            {
                var url = new UrlString();
                foreach (var pair in attrs)
                {
                    url.Parameters.Add(pair.Key, pair.Value);
                }
                return Editable(model, field, url.Query);
            }
            else
            {
                return RenderImage(GetCompiled(field).Invoke(model) as Fields.Image, parameters == null ? null : attrs, outputHeightWidth);
            }
        }

        /// <summary>
        /// Renders HTML for an image
        /// </summary>
        /// <param name="image">The image to render</param>
        /// <param name="attributes">Additional parameters to add. Do not include alt or src</param>
        /// <param name="outputHeightWidth">Indicates if the height and width attributes should be output when rendering the image</param>
        /// <returns>An img HTML element</returns>
        public virtual string RenderImage(
            Fields.Image image,
            SafeDictionary<string> attributes,
            bool outputHeightWidth = false
            )
        {

            if (image == null)
            {
                return string.Empty;
            }

            if (attributes == null)
            {
                attributes = new SafeDictionary<string>();
            }

            var origionalKeys = attributes.Keys.ToList();

            //should there be some warning about these removals?
            AttributeCheck(attributes, ImageParameterKeys.CLASS, image.Class);
            AttributeCheck(attributes, ImageParameterKeys.ALT, image.Alt);
            AttributeCheck(attributes, ImageParameterKeys.BORDER, image.Border);
            if (image.HSpace > 0)
                AttributeCheck(attributes, ImageParameterKeys.HSPACE, image.HSpace.ToString(CultureInfo.InvariantCulture));
            if (image.VSpace > 0)
                AttributeCheck(attributes, ImageParameterKeys.VSPACE, image.VSpace.ToString(CultureInfo.InvariantCulture));
            if (image.Width > 0)
                AttributeCheck(attributes, ImageParameterKeys.WIDTHHTML, image.Width.ToString(CultureInfo.InvariantCulture));
            if (image.Height > 0)
                AttributeCheck(attributes, ImageParameterKeys.HEIGHTHTML, image.Height.ToString(CultureInfo.InvariantCulture));

            var urlParams = new SafeDictionary<string>();
            var htmlParams = new SafeDictionary<string>();

            /*
             * ME - This method is used to render images rather than going back to the fieldrender
             * because it stops another call having to be passed to Sitecore.
             */

            if (image == null || image.Src.IsNullOrWhiteSpace()) return "";

            if (attributes == null) attributes = new SafeDictionary<string>();

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

            var keys = attributes.Keys.ToList();
            foreach (var key in keys)
            {
                switch (key)
                {
                    case ImageParameterKeys.BORDER:
                    case ImageParameterKeys.ALT:
                    case ImageParameterKeys.HSPACE:
                    case ImageParameterKeys.VSPACE:
                    case ImageParameterKeys.CLASS:
                    case ImageParameterKeys.WIDTHHTML:
                    case ImageParameterKeys.HEIGHTHTML:
                        html(key);
                        break;
                    case ImageParameterKeys.OUTPUT_METHOD:
                    case ImageParameterKeys.ALLOW_STRETCH:
                    case ImageParameterKeys.IGNORE_ASPECT_RATIO:
                    case ImageParameterKeys.SCALE:
                    case ImageParameterKeys.MAX_WIDTH:
                    case ImageParameterKeys.MAX_HEIGHT:
                    case ImageParameterKeys.THUMBNAIL:
                    case ImageParameterKeys.BACKGROUND_COLOR:
                    case ImageParameterKeys.DATABASE:
                    case ImageParameterKeys.LANGUAGE:
                    case ImageParameterKeys.VERSION:
                    case ImageParameterKeys.DISABLE_MEDIA_CACHE:
                    case ImageParameterKeys.WIDTH:
                    case ImageParameterKeys.HEIGHT:
                        url(key);
                        break;
                    default:
                        html(key);
                        break;
                }
            }

            //copy width and height across to url
            if (!urlParams.ContainsKey(ImageParameterKeys.WIDTH) && !urlParams.ContainsKey(ImageParameterKeys.HEIGHT))
            {
                if (origionalKeys.Contains(ImageParameterKeys.WIDTHHTML))
                {
                    urlParams[ImageParameterKeys.WIDTH] = htmlParams[ImageParameterKeys.WIDTHHTML];
                }
                if (origionalKeys.Contains(ImageParameterKeys.HEIGHTHTML))
                {
                    urlParams[ImageParameterKeys.HEIGHT] = htmlParams[ImageParameterKeys.HEIGHTHTML];
                }
            }


            //calculate size

            var finalSize = Utilities.ResizeImage(
                image.Width,
                image.Height,
                urlParams[ImageParameterKeys.SCALE].ToFlaot(),
                urlParams[ImageParameterKeys.WIDTH].ToInt(),
                urlParams[ImageParameterKeys.HEIGHT].ToInt(),
                urlParams[ImageParameterKeys.MAX_WIDTH].ToInt(),
                urlParams[ImageParameterKeys.MAX_HEIGHT].ToInt());

          

            urlParams[ImageParameterKeys.HEIGHT] = finalSize.Height.ToString();
            urlParams[ImageParameterKeys.WIDTH] = finalSize.Width.ToString();

            Action<string, string> originalAttributeClean = (exists, missing) =>
            {
                if (origionalKeys.Contains(exists) && !origionalKeys.Contains(missing))
                {
                    urlParams.Remove(missing);
                    htmlParams.Remove(missing);
                }
            };
            //we do some smart clean up
            originalAttributeClean(ImageParameterKeys.WIDTHHTML, ImageParameterKeys.HEIGHTHTML);
            originalAttributeClean(ImageParameterKeys.HEIGHTHTML, ImageParameterKeys.WIDTHHTML);

            if (!outputHeightWidth)
            {
                htmlParams.Remove(ImageParameterKeys.WIDTHHTML);
                htmlParams.Remove(ImageParameterKeys.HEIGHTHTML);
            }

            var builder = new UrlBuilder(image.Src);



            foreach (var key in urlParams.Keys)
            {
                builder.AddToQueryString(key, urlParams[key]);
            }

            string mediaUrl = builder.ToString();

#if (SC80 || SC75)
            mediaUrl = ProtectMediaUrl(mediaUrl);
#endif
            return ImageTagFormat.Formatted(mediaUrl, Utilities.ConvertAttributes(htmlParams, QuotationMark), QuotationMark);
        }

#if (SC80 || SC75)
        public virtual string ProtectMediaUrl(string url)
        {
            return HashingUtils.ProtectAssetUrl(url);
        }
#endif



    }
}




