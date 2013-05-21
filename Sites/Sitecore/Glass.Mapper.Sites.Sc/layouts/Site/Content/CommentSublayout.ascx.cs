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
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Ui;
using Glass.Mapper.Sites.Sc.Models.Content;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sites.Sc.layouts.Site.Content
{
    public partial class CommentSublayout : GlassUserControl<CommentPage>
    {
        protected override void OnInit(EventArgs e)
        {
            CommentSubmit.Click += CommentSubmit_Click;
            base.OnInit(e);
        }

        void CommentSubmit_Click(object sender, EventArgs e)
        {
            var contextService = new SitecoreContext();
            var masterService = new SitecoreService("master");

            var page = contextService.GetCurrentItem<CommentPage>();

            var comment = new Comment();

            //This value will be used for the name   of the item
            comment.Name = DateTime.Now.ToString("yy-MM-ddThh-mm-ss");
            comment.Content = CommentContent.Text;
            comment.FullName = CommentName.Text;
            comment.Email = CommentEmail.Text;

            using (new SecurityDisabler())
            {
                masterService.Create(page, comment);
            }

            CommentThankYou.Visible = true;
            comment.Content = string.Empty;
            comment.FullName = string.Empty;
            comment.Email = string.Empty;
        }
    }
}
