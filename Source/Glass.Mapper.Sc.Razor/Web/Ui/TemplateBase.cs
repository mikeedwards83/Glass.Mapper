using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Web.Mvc;
using Glass.Mapper.Sc.Razor.Web.Mvc;
using RazorEngine.Text;
using Sitecore.Web.UI.WebControls;
using Image = Glass.Mapper.Sc.Fields.Image;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    public class TemplateBase<T>:RazorEngine.Templating.TemplateBase<T>
    {
        HtmlHelper _helper;

        public TemplateBase()
        {
            
            
        }   
      
        public ViewDataDictionary ViewData { get; private set; }

        public GlassHtmlFacade GlassHtml { get; private set; }

        public HtmlHelper Html { get; private set; }

        public IEnumerable<Placeholder> Placeholders
        {
            get
            {
               return ParentControl.Controls.Cast<Control>()
                             .Where(x => x is global::Sitecore.Web.UI.WebControls.Placeholder)
                             .Cast<global::Sitecore.Web.UI.WebControls.Placeholder>();
            }
        }

        public Control ParentControl { get; private set; }

        public void Configure(ISitecoreService service, ViewDataDictionary viewData, Control parentControl)

        {
            GlassHtml = new GlassHtmlFacade(service);
            Html =  new HtmlHelper(new ViewContext(), new ViewDataContainer() { ViewData = ViewData });
            ViewData = viewData;
            ParentControl = parentControl;
        }

        public string RenderHolder(string key)
        {
            key = key.ToLower();
            var placeHolder = Placeholders.FirstOrDefault(x => x.Key == key);

            if (placeHolder == null)
                return "No placeholder with key: {0}".Formatted(key);
            else
            {
                var sb = new StringBuilder();
                placeHolder.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                return sb.ToString();
            }
        }

        public IEncodedString Placeholder(string key)
        {
            key = key.ToLower();
            var placeHolder = Placeholders.FirstOrDefault(x => x.Key == key);

            if (placeHolder == null)
                placeHolder = new global::Sitecore.Web.UI.WebControls.Placeholder { Key = key };
            ParentControl.Controls.Add(placeHolder);

            var sb = new StringBuilder();
            placeHolder.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
            return Raw(sb.ToString());
        }

        public IEncodedString Editable<T1>(T1 target, Expression<Func<T1, object>> field)
        {
            return GlassHtml.Editable(target, field);
        }

        public IEncodedString Editable<T1>(T1 target, Expression<Func<T1, object>> field, string parameters)
        {
            return GlassHtml.Editable(target, field, parameters);
        }

        public IEncodedString Editable<T1>(T1 target, Expression<Func<T1, object>> field, Expression<Func<T1, string>> standardOutput)
        {
            return GlassHtml.Editable(target, field, standardOutput);
        }

        public IEncodedString RenderImage(Image image)
        {
            return GlassHtml.RenderImage(image);
        }

        public IEncodedString RenderImage(Image image, NameValueCollection attributes)
        {
            return GlassHtml.RenderImage(image, attributes);
        }



        public RawString Editable(Expression<Func<T, object>> field)
        {
            if (field == null) throw new NullReferenceException("No field set");

            if (Model == null) throw new NullReferenceException("No model set");

            try
            {
                return GlassHtml.Editable(this.Model, field);
            }
            catch (Exception ex)
            {
                return new RawString(ex.Message);
            }

        }

    }
}
