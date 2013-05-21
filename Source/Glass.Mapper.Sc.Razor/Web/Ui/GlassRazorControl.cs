using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.UI;
using Glass.Mapper.Sc.Razor.RenderingTypes;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Web.UI;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    /// <summary>
    /// Use this control to add a razor rendering to a sublayout
    /// </summary>
    public class GlassRazorControl : WebControl
    {
        /// <summary>
        /// The path to the razor rendering in Sitecore
        /// </summary>
        public string Path { get; set; }

       
        private WebControl _control;


        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {

            var item = Sitecore.Context.Database.GetItem(Path);

            if (item == null)
                return;


            NameValueCollection parameters = new NameValueCollection();
            
            foreach (Field field in item.Fields)
            {
                parameters.Add(field.Name, field.Value);
            }

            IRenderingType renderType = null;
            if (item.TemplateID == SitecoreIds.GlassBehindRazorId)
                renderType = new BehindRazorRenderingType();
            else if (item.TemplateID == SitecoreIds.GlassDynamicRazorId)
                renderType = new DynamicRazorRenderingType();
            else if (item.TemplateID == SitecoreIds.GlassTypedRazorId)
                renderType = new TypedRazorRenderingType();

            _control = renderType.GetControl(parameters, false) as WebControl;
            _control.DataSource = this.DataSource;


            base.OnLoad(e);
        }


        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="output">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        /// <remarks>
        /// When developing custom server controls, you can override this method to generate content for an ASP.NET page.
        /// </remarks>
        protected override void DoRender(HtmlTextWriter output)
        {
            if (_control != null)
                _control.RenderControl(output);
        }
    }
}
