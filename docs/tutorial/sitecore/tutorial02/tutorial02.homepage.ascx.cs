using System;
using Glass.Mapper.Sc;
using Glass.Mapper.Sites.Sc.Models.Landing;

namespace Glass.Mapper.Sites.Sc.layouts.Site.Landing
{
    public partial class HomePageSublayout : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = new SitecoreContext();
            Model = context.GetCurrentItem<HomePage>();
        }
        public HomePage Model { get; set; }
    }
}