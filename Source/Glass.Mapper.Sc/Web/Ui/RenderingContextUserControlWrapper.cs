using System.Web.UI;
using Sitecore.Web.UI;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.Web.Ui
{
    public class RenderingContextUserControlWrapper : IRenderingContext
    {
        private readonly Control _control;

        public RenderingContextUserControlWrapper(Control control)
        {
            _control = control;
        }

        public bool HasDataSource
        {
            get { return GetDataSource().HasValue(); } 
        }

        public string GetRenderingParameters()
        {
            return GetRenderingParametersInternal(_control);
        }
      
        public string GetDataSource()
        {
            if (_control == null) return string.Empty;

            WebControl parent = _control.Parent as WebControl;
            if (parent == null)
                return string.Empty;
            return parent.DataSource;
        }

        private string GetRenderingParametersInternal(Control control)
        {
            if (control == null) return null;

            var sublayout = control as Sublayout;
            if (sublayout != null)
            {
                return sublayout.Parameters;
            }

            return GetRenderingParametersInternal(control.Parent);
        }

    }
}
