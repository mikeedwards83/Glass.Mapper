/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

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


