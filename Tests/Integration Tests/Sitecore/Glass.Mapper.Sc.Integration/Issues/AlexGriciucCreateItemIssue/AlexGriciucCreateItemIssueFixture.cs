using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Integration.Issues.AlexGriciucCreateItemIssue
{
    [TestFixture]
    public class AlexGriciucCreateItemIssueFixture
    {

        [Test]
        public void CreatesItem()
        {
            //Arrange
            var context = Context.Create(Utilities.CreateStandardResolver());

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var scContext = new SitecoreService(db);

            var parentItem = db.GetItem("/sitecore/content/Tests/Issues/AlexGriciucCreateItemIssue");
            var parent = scContext.GetItem<CommentPage>("/sitecore/content/Tests/Issues/AlexGriciucCreateItemIssue");

            using (new SecurityDisabler())
            {
                parentItem.DeleteChildren();
            }

            var newItem = db.GetItem("/sitecore/content/Tests/Issues/AlexGriciucCreateItemIssue/TestName");
            Assert.IsNull(newItem);

            //Act
        //    scContext.GlassContext.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(Comment)));
            var newClass = new Comment();
            newClass.Name = "TestName";
            using (new SecurityDisabler())
            {
                scContext.Create(parent, newClass);
            }

            //Asset

            newItem = db.GetItem("/sitecore/content/Tests/Issues/AlexGriciucCreateItemIssue/TestName");
            Assert.IsNotNull(newItem);
        }

        #region Classes

        public class CommentPage
        {
            public virtual ID Id { get; set; }
            public virtual IEnumerable<Comment> Children { get; set; }
        }

        [SitecoreType(TemplateId = "{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}", AutoMap = true)]
        public class Comment
        {

            public virtual string Name { get; set; }

            public string FullName { get; set; }
            public virtual string Email { get; set; }

            [SitecoreField("Comment")]
            public virtual string Content { get; set; }

            [SitecoreField("__Created")]
            public virtual DateTime Date { get; set; }
        }


        #endregion


    }
}
