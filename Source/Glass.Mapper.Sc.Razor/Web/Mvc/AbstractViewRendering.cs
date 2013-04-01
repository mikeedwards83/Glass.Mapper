using System.Text;
using Sitecore.Mvc.Presentation;
using System.Web.UI;
using System.IO;

namespace Glass.Mapper.Sc.Razor.Web.Mvc
{
    /// <summary>
    /// Class AbstractViewRendering
    /// </summary>
    public abstract class AbstractViewRendering : Renderer
    {
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }
        /// <summary>
        /// Gets or sets the name of the context.
        /// </summary>
        /// <value>The name of the context.</value>
        public string ContextName { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(new StringWriter(sb));

            Render(htmlWriter);
            return sb.ToString();
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public string DataSource { get; set; }
    }
}
