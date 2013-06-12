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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.RenderField;
using Glass.Mapper.Sc.Web.Ui;
using Sitecore.Data.Items;
using Sitecore.Text;
using Sitecore.Web.UI.WebControls;

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
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field)
        {
            return MakeEditable<T>(field, null, target);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters">Additional rendering parameters, e.g. ImageParameters</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field, AbstractParameters parameters)
        {
            return MakeEditable<T>(field, null, target, parameters);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="parameters">Additional rendering parameters, e.g. class=myCssClass</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field, string parameters)
        {
            return MakeEditable<T>(field, null, target, parameters);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor.  Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)
        {
            return MakeEditable<T>(field, standardOutput, target);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <param name="parameters">Additional rendering parameters, e.g. ImageParameters</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, AbstractParameters parameters)
        {
            return MakeEditable<T>(field, null, target, parameters);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        [Obsolete("Use Editable<T>(T target, Expression<Func<T, object>> field)")]
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
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        [Obsolete("Use Editable<T>(T target, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)")]
        public string Editable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target)
        {
            return MakeEditable<T>(field, standardOutput, target);
        }

        /// <summary>
        /// Renders HTML for an image
        /// </summary>
        /// <param name="image">The image to render</param>
        /// <returns>An img HTML element</returns>
        public virtual string RenderImage(Fields.Image image)
        {
            return RenderImage(image, null);
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

        /// <summary>
        /// Renders HTML for an image
        /// </summary>
        /// <param name="image">The image to render</param>
        /// <param name="attributes">Additional attributes to add. Do not include alt or src</param>
        /// <returns>An img HTML element</returns>
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
            if (attributes[ImageHeight].IsNotNullOrEmpty())
                attributes.Add(ImageParameters.HEIGHT, attributes[ImageHeight]);

            foreach (var key in attributes.AllKeys)
            {
                if(key=="alt" || key=="class" || key=="style")
                    continue;
                
                builder[key] = attributes[key];
            }

            //should there be some warning about these removals?
            AttributeCheck(attributes, "class", image.Class);
            AttributeCheck(attributes, "alt", image.Alt);


            return ImageTagFormat.Formatted(builder.ToString(), Utilities.ConvertAttributes(attributes));
        }

        /// <summary>
        /// Checks it and attribute is part of the NameValueCollection and updates it with the
        /// default if it isn't.
        /// </summary>
        /// <param name="collection">The collection of attributes</param>
        /// <param name="name">The name of the attribute in the collection</param>
        /// <param name="defaultValue">The default value for the attribute</param>
        public virtual void AttributeCheck(NameValueCollection collection, string name, string defaultValue)
        {
            if (collection[name].IsNullOrEmpty() && !defaultValue.IsNullOrEmpty())
                collection[name] = defaultValue;
        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <returns>An "a" HTML element</returns>
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
        public virtual string RenderLink(Fields.Link link, NameValueCollection attributes, string contents)
        {
            if (link == null) return "";
            if (attributes == null) attributes = new NameValueCollection();

            string format = "<a href='{0}{1}' title='{2}' target='{3}' class='{4}' {5}>{6}</a>";

            string cls = attributes.AllKeys.Any(x => x == "class") ? attributes["class"] : link.Class;
            string anchor = link.Anchor.IsNullOrEmpty() ? "" : "#" + link.Anchor;
            string target = attributes.AllKeys.Any(x => x == "target") ? attributes["target"] : link.Target;


            AttributeCheck(attributes, "class", link.Class);
            AttributeCheck(attributes, "target", link.Target);
            AttributeCheck(attributes, "title", link.Title);

            attributes.Remove("href");


            return format.Formatted(link.Url, anchor, link.Title, target, cls, Utilities.ConvertAttributes(attributes), contents.IsNullOrEmpty() ? link.Text : contents);

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
        /// <param name="target">The target.</param>
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
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        private string MakeEditable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target, AbstractParameters parameters)
        {
            return MakeEditable<T>(field, standardOutput, target, parameters.ToString());
        }

        /// <summary>
        /// Makes the editable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="target">The target.</param>
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
        private string MakeEditable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target,  string parameters)
        {

            if (IsInEditingMode)
            {
                if (field.Parameters.Count > 1)
                    throw new MapperException("To many parameters in linq expression {0}".Formatted(field.Body));



                MemberExpression memberExpression;

                if (field.Body is UnaryExpression)
                {
                    memberExpression = ((UnaryExpression)field.Body).Operand as MemberExpression;
                }
                else if (!(field.Body is MemberExpression))
                {
                    throw new MapperException("Expression doesn't evaluate to a member {0}".Formatted(field.Body));
                }
                else
                {
                    memberExpression = (MemberExpression)field.Body;
                }



                //we have to deconstruct the lambda expression to find the 
                //correct target object
                //For example if we have the lambda expression x =>x.Children.First().Content
                //we have to evaluate what the first Child object is, then evaluate the field to edit from there.

                //this contains the expression that will evaluate to the object containing the property
                var objectExpression = memberExpression.Expression;

                var finalTarget = Expression.Lambda(objectExpression, field.Parameters).Compile().DynamicInvoke(target);

                var site = global::Sitecore.Context.Site;

                if (_context == null) 
                    throw new NullReferenceException("Context cannot be null");

                var config = _context.GetTypeConfiguration(finalTarget) as SitecoreTypeConfiguration;

              
                var scClass = config.ResolveItem(finalTarget, SitecoreContext.Database);

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
                    throw new MapperException("Page editting error. Could not find property {0} on type {1}".Formatted(memberExpression.Member.Name, config.Type.FullName));

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
                    FieldRenderer renderer = new FieldRenderer();
                    renderer.Item = scClass;
                    //TODO: - check this works with field ID as well
                    renderer.FieldName = ((SitecoreFieldConfiguration)dataHandler).FieldName;
                    renderer.Parameters = parameters;
                    return renderer.Render();
                }
            }
            else
            {
                if (standardOutput != null)
                    return standardOutput.Compile().Invoke(target);
                else
                    return (field.Compile().Invoke(target) ?? string.Empty).ToString();
            }
            //return field.Compile().Invoke(target).ToString();
        }

    }
}




