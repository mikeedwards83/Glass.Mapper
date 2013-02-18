using System;
using Glass.Mapper.Sc;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.layouts.Site.Misc
{
    public partial class TopNavigationSublayout : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = new SitecoreContext();
            Model = context.GetHomeItem<MenuItem>();
        }
        public MenuItem Model { get; set; }
    }
}