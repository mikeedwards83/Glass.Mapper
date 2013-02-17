using System;
using Glass.Mapper.Sc;
using Glass.Mapper.Sites.Sc.Tutorial1.Model;

namespace Glass.Mapper.Sites.Sc.layouts.Tutorial1
{
    public partial class Tutorial1Sublayout : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = new SitecoreContext();
            Model = context.GetCurrentItem<DemoClass>();
        }
        public DemoClass Model { get; set; }
    }
}