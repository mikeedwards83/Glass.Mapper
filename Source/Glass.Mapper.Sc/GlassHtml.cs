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
using System.Reflection;
using System.Text;
using System.Web;
using Castle.DynamicProxy;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.RenderField;
using Glass.Mapper.Sc.Web.Ui;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// This class contains a set of helpers that make converting items mapped in Glass.Sitecore.Mapper to HTML
    /// </summary>
    public class GlassHtml
    {
        private readonly ISitecoreService _service;
        private readonly Context _context;

        /// <param name="service">The service that will be used to load and save data</param>
        public GlassHtml(ISitecoreService service)
        {
            _service = service;
            _context = service.GlassContext;
        }


        public GlassEditFrame EditFrame(string buttons)
        {
            var frame = new GlassEditFrame(buttons, HttpContext.Current);
            frame.RenderFirstPart();
            return frame;
        }


        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field)
        {
            return MakeEditable<T>(field, null, target);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
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
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
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
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <param name="service">The service that will be used to load and save data</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public virtual string Editable<T>(T target, Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)
        {
            return MakeEditable<T>(field, standardOutput, target);
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Glass.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
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
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <param name="service">The service that will be used to load and save data</param>
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
        /// Renders HTML for an image
        /// </summary>
        /// <param name="image">The image to render</param>
        /// <param name="attributes">Additional attributes to add. Do not include alt or src</param>
        /// <returns>An img HTML element</returns>
        public virtual string RenderImage(Fields.Image image, NameValueCollection attributes)
        {
            if (image == null) return "";

            if (attributes == null) attributes = new NameValueCollection();

            string format = "<img src='{0}' {1}/>";

            //should there be some warning about these removals?
            AttributeCheck(attributes, "class", image.Class);
            AttributeCheck(attributes, "alt", image.Alt);
            if (image.Height > 0)
                AttributeCheck(attributes, "height", image.Height.ToString());
            if (image.Width > 0)
                AttributeCheck(attributes, "width", image.Width.ToString());

            return format.Formatted(image.Src, Utilities.ConvertAttributes(attributes));
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
        /// <returns>An "a" HTML element </returns>
        public virtual string RenderLink(Fields.Link link)
        {

            return RenderLink(link, null, string.Empty);

        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional attributes to add. Do not include href or title</param>
        /// <returns>An "a" HTML element </returns>
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
        /// <returns>An "a" HTML element </returns>
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
        public static bool IsInEditingMode
        {
            get
            {
                return
                            global::Sitecore.Context.PageMode.IsPageEditorEditing;
            }
        }


        private string MakeEditable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target)
        {
            return MakeEditable(field, standardOutput, target, string.Empty);
        }

        private string MakeEditable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target, AbstractParameters parameters)
        {
            return MakeEditable<T>(field, standardOutput, target, parameters.ToString());
        }

        private string MakeEditable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target,  string parameters)
        {

            if (standardOutput == null || IsInEditingMode)
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
                
                //ME - test if we can remove this
                ////if the class a proxy then we have to get it's base type
                //Type type;
                //if (finalTarget is IProxyTargetAccessor)
                //{
                //    //first try the base type
                //    type = finalTarget.GetType().BaseType;

                //    //if it doesn't contain the base type then we need to check the interfaces
                //    if (!context.TypeConfigurations.ContainsKey(type))
                //    {

                //        var interfaces = finalTarget.GetType().GetInterfaces();

                //        string name = finalTarget.GetType().Name;
                //        //be default castle will use the name of the class it is proxying for it's own name
                //        foreach (var inter in interfaces)
                //        {
                //            if (name.StartsWith(inter.Name))
                //            {
                //                type = inter;
                //                break;
                //            }
                //        }
                //    }
                //}
                //else
                //    type = finalTarget.GetType();


                var config = _context.GetTypeConfiguration(finalTarget) as SitecoreTypeConfiguration;

                //Guid id = Guid.Empty;

                //try
                //{
                //    id = context.GetClassId(type, finalTarget);
                //}
                //catch (SitecoreIdException ex)
                //{
                //    throw new MapperException("Page editting error. Type {0} can not be used for editing. Could not find property with SitecoreID attribute. See inner exception".Formatted(typeof(T).FullName), ex);
                //}

                var scClass = config.ResolveItem(finalTarget, _service.Database);

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
                return standardOutput.Compile().Invoke(target);
            }
            //return field.Compile().Invoke(target).ToString();
        }

    }
}



