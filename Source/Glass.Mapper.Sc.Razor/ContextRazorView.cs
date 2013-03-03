using System;
using Sitecore.Web.UI;
using System.ComponentModel;
using Glass.Mapper.Sc.Razor.Web.Ui;

namespace Glass.Mapper.Sc.Razor
{
    public class ContextRazorView : WebControl
    {

        public string TypeName
        {
            get;
            set;
        }

        public string AssemblyName { get; set; }

        public string Context { get; set; }

        protected override void CreateChildControls()
        {

            Type type =  Type.GetType(TypeName);

            Type razorControlType = typeof(AbstractRazorControl<>);
            Type finalControlType = razorControlType.MakeGenericType(type);

            WebControl finalControl =Activator.CreateInstance(finalControlType) as WebControl;



            ISitecoreContext _context = new SitecoreContext(Context);
            var model = _context.GetCurrentItem(type);

            TypeDescriptor.GetProperties(finalControlType).Find("Model", false).SetValue(finalControl, model);

            this.Controls.Add(finalControl);

            base.CreateChildControls();
        }

        protected override void DoRender(System.Web.UI.HtmlTextWriter output)
        {
           //nothing happens here :-)
        }
    }
}
