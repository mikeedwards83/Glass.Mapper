using System;
using Glass.Mapper.Sc;
using Glass.Mapper.Sites.Sc.Tutorial2.Model;

namespace Glass.Mapper.Sites.Sc.Tutorial2.Layouts
{
    public partial class Tutorial2Sublayout : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = new SitecoreContext();
            this.Model = context.GetCurrentItem<DemoClass>();
        }
        public DemoClass Model { get; set; }
    }
}