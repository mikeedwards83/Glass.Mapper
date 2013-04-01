using System;
using Glass.Mapper.Sc.Fields;

namespace Glass.Mapper.Sc.Razor
{
    /// <summary>
    /// Class GlassHtmlFacade
    /// </summary>
    public class GlassHtmlFacade 
    {
        private GlassHtml _glassHtml;
        /// <summary>
        /// Initializes a new instance of the <see cref="GlassHtmlFacade"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public GlassHtmlFacade(ISitecoreService service)
            
        {
            _glassHtml = new GlassHtml(service);
        }

        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <returns>RawString.</returns>
        public  RawString Editable<T>(T target, System.Linq.Expressions.Expression<Func<T, object>> field)
        {
            return _glassHtml.Editable<T>(target, field).RawString();
        }
        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>RawString.</returns>
        public  RawString Editable<T>(T target, System.Linq.Expressions.Expression<Func<T, object>> field, RenderField.AbstractParameters parameters)
        {
            return _glassHtml.Editable<T>(target, field, parameters).RawString();
        }
        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>RawString.</returns>
        public RawString Editable<T>(T target, System.Linq.Expressions.Expression<Func<T, object>> field, string parameters)
        {
            return _glassHtml.Editable<T>(target, field, parameters).RawString();
        }
        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <returns>RawString.</returns>
        public  RawString Editable<T>(T target, System.Linq.Expressions.Expression<Func<T, object>> field, System.Linq.Expressions.Expression<Func<T, string>> standardOutput)
        {
            return _glassHtml.Editable<T>(target, field, standardOutput).RawString();
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
        public RawString Editable<T>(T target, System.Linq.Expressions.Expression<Func<T, object>> field, System.Linq.Expressions.Expression<Func<T, string>> standardOutput, RenderField.AbstractParameters parameters)
        {
            return _glassHtml.Editable<T>(target, field, standardOutput, parameters).RawString();
        }
        /// <summary>
        /// Renders the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>RawString.</returns>
        public  RawString RenderImage(Image image)
        {
            return _glassHtml.RenderImage(image).RawString();
        }
        /// <summary>
        /// Renders the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>RawString.</returns>
        public  RawString RenderImage(Image image, System.Collections.Specialized.NameValueCollection attributes)
        {
            return _glassHtml.RenderImage(image, attributes).RawString();
        }
        /// <summary>
        /// Renders the link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns>RawString.</returns>
        public  RawString RenderLink(Link link)
        {
            return _glassHtml.RenderLink(link).RawString();
        }
        /// <summary>
        /// Renders the link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>RawString.</returns>
        public  RawString RenderLink(Link link, System.Collections.Specialized.NameValueCollection attributes)
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
        public  RawString RenderLink(Link link, System.Collections.Specialized.NameValueCollection attributes, string contents)
        {
            return _glassHtml.RenderLink(link, attributes, contents).RawString();
        }
    }
}
