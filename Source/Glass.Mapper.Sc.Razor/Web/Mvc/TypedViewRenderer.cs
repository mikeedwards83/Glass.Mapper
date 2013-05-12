using Glass.Mapper.Sc.Razor.RenderingTypes;
using System.Web.UI;
using Sitecore.Web.UI;

namespace Glass.Mapper.Sc.Razor.Web.Mvc
{
    /// <summary>
    /// Class TypedViewRenderer
    /// </summary>
    public class TypedViewRenderer :AbstractViewRendering
    {
        
        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Render(System.IO.TextWriter writer)
        {
            WebControl control = TypedRazorRenderingType.CreateControl(Path, ContextName) as WebControl;
            if (control != null)
            {
                HtmlTextWriter htmlWriter = new HtmlTextWriter(writer);

                control.DataSource = this.DataSource;

                control.RenderControl(htmlWriter);
            }

        }
        

      
    }
}
