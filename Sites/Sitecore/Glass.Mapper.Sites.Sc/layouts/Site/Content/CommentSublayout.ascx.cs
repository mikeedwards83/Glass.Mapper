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