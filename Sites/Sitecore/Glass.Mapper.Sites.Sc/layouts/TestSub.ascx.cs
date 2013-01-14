using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Integration;
using Glass.Mapper.Sites.Sc.Models;

namespace Glass.Mapper.Sites.Sc.layouts
{
    public partial class TestSub : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = Glass.Mapper.Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader(
                "Glass.Mapper.Sites.Sc"
                ));

            SitecoreService service = new SitecoreService(Sitecore.Context.Database);
            var item = service.GetItem<ModelBase>(Sitecore.Context.Item.ID.Guid);

            Text.Text = item.Text;
            Title.Text = item.Title;

        }
    }
}