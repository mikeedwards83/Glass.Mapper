using Glass.Mapper.Sc.Razor.RenderingTypes;
using System.Web.UI;
using Glass.Mapper.Sc.Razor.Web.Ui;

namespace Glass.Mapper.Sc.Razor.Web.Mvc
{
    public class DynamicViewRenderer :AbstractViewRendering
    {

        public override void Render(System.IO.TextWriter writer)
        {
            DynamicControl control = DynamicRazorRenderingType.CreateControl(Path, ContextName) as DynamicControl;
            if (control != null)
            {
                HtmlTextWriter htmlWriter = new HtmlTextWriter(writer);

                control.DataSource = this.DataSource;

                control.RenderControl(htmlWriter);
            }

        }
        

      
    }
}
