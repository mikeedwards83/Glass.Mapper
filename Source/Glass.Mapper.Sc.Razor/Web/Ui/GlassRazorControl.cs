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

        public readonly ID GlassBehindRazorId = new ID("{9B162562-C999-45BF-B688-1C65D0EBCAAD}");
        public readonly ID GlassDynamicRazorId = new ID("{4432051D-8D3E-48E9-8C06-F1970EE607C5}");
        public readonly ID GlassTypedRazorId = new ID("{7B10C01D-B0DF-4626-BE34-F48E38828FB7}");

        private WebControl _control;


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
            if (item.TemplateID == GlassBehindRazorId)
                renderType = new BehindRazorRenderingType();
            else if (item.TemplateID == GlassDynamicRazorId)
                renderType = new DynamicRazorRenderingType();
            else if (item.TemplateID == GlassTypedRazorId)
                renderType = new TypedRazorRenderingType();

            _control = renderType.GetControl(parameters, false) as WebControl;
            _control.DataSource = this.DataSource;


            base.OnLoad(e);
        }


        protected override void DoRender(HtmlTextWriter output)
        {
            if (_control != null)
                _control.RenderControl(output);
        }
    }
}
