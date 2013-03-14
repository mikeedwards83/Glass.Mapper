using Glass.Mapper.Sc.Razor.RenderingTypes;
using System.Web.UI;
using Sitecore.Web.UI;

namespace Glass.Mapper.Sc.Razor.Web.Mvc
{
    /// <summary>
    /// Class BehindViewRenderer
    /// </summary>
    public class BehindViewRenderer :AbstractViewRendering
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type{get;set;}
        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>The assembly.</value>
        public string Assembly{get;set;}

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Render(System.IO.TextWriter writer)
        {
            WebControl control = BehindRazorRenderingType.CreateControl(Path, Type, Assembly, ContextName) as WebControl;
            if (control != null)
            {
                HtmlTextWriter htmlWriter = new HtmlTextWriter(writer);

                control.DataSource = this.DataSource;

                control.RenderControl(htmlWriter);
            }

        }
        

      
    }
}
