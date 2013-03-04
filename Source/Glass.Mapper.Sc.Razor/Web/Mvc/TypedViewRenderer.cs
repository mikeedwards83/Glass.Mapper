using Glass.Mapper.Sc.Razor.RenderingTypes;
using System.Web.UI;
using Sitecore.Web.UI;

namespace Glass.Mapper.Sc.Razor.Web.Mvc
{
    public class TypedViewRenderer :AbstractViewRendering
    {
        public string Type{get;set;}
        public string Assembly{get;set;}
       

        public override void Render(System.IO.TextWriter writer)
        {
            WebControl control = TypedRazorRenderingType.CreateControl(Path, Type, Assembly, ContextName) as WebControl;
            if (control != null)
            {
                HtmlTextWriter htmlWriter = new HtmlTextWriter(writer);

                control.DataSource = this.DataSource;

                control.RenderControl(htmlWriter);
            }

        }
        

      
    }
}
