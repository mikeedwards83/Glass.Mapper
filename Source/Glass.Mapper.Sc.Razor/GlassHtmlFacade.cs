using System;
using Glass.Mapper.Sc.Fields;

namespace Glass.Mapper.Sc.Razor
{
    public class GlassHtmlFacade 
    {
        private GlassHtml _glassHtml;
        public GlassHtmlFacade(ISitecoreService service)
            
        {
            _glassHtml = new GlassHtml(service);
        }

        public  RawString Editable<T>(T target, System.Linq.Expressions.Expression<Func<T, object>> field)
        {
            return _glassHtml.Editable<T>(target, field).RawString();
        }
        public  RawString Editable<T>(T target, System.Linq.Expressions.Expression<Func<T, object>> field, RenderField.AbstractParameters parameters)
        {
            return _glassHtml.Editable<T>(target, field, parameters).RawString();
        }
        public RawString Editable<T>(T target, System.Linq.Expressions.Expression<Func<T, object>> field, string parameters)
        {
            return _glassHtml.Editable<T>(target, field, parameters).RawString();
        }
        public  RawString Editable<T>(T target, System.Linq.Expressions.Expression<Func<T, object>> field, System.Linq.Expressions.Expression<Func<T, string>> standardOutput)
        {
            return _glassHtml.Editable<T>(target, field, standardOutput).RawString();
        }
        public RawString Editable<T>(T target, System.Linq.Expressions.Expression<Func<T, object>> field, System.Linq.Expressions.Expression<Func<T, string>> standardOutput, RenderField.AbstractParameters parameters)
        {
            return _glassHtml.Editable<T>(target, field, standardOutput, parameters).RawString();
        }
        public  RawString RenderImage(Image image)
        {
            return _glassHtml.RenderImage(image).RawString();
        }
        public  RawString RenderImage(Image image, System.Collections.Specialized.NameValueCollection attributes)
        {
            return _glassHtml.RenderImage(image, attributes).RawString();
        }
        public  RawString RenderLink(Link link)
        {
            return _glassHtml.RenderLink(link).RawString();
        }
        public  RawString RenderLink(Link link, System.Collections.Specialized.NameValueCollection attributes)
        {
            return _glassHtml.RenderLink(link, attributes).RawString();
        }
        public  RawString RenderLink(Link link, System.Collections.Specialized.NameValueCollection attributes, string contents)
        {
            return _glassHtml.RenderLink(link, attributes, contents).RawString();
        }
    }
}
