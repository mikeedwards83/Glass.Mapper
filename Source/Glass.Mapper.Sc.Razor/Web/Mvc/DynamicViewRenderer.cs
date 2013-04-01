using Glass.Mapper.Sc.Razor.RenderingTypes;
using System.Web.UI;
using Glass.Mapper.Sc.Razor.Web.Ui;

namespace Glass.Mapper.Sc.Razor.Web.Mvc
{
    /// <summary>
    /// Class DynamicViewRenderer
    /// </summary>
    public class DynamicViewRenderer :AbstractViewRendering
    {

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
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
