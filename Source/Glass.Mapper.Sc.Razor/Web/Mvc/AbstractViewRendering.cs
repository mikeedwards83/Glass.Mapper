using System.Text;
using Sitecore.Mvc.Presentation;
using System.Web.UI;
using System.IO;

namespace Glass.Mapper.Sc.Razor.Web.Mvc
{
    public abstract class AbstractViewRendering : Renderer
    {
        public string Path { get; set; }
        public string ContextName { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(new StringWriter(sb));

            Render(htmlWriter);
            return sb.ToString();
        }

        public string DataSource { get; set; }
    }
}
