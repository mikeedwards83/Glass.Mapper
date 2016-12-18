using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.FakeDb.Issues.AlexGriciucCreateItemIssue
{
    [TestFixture]
    public class AlexGriciucCreateItemIssueFixture
    {

        [Test]
        public void CreatesItem()
        {
            //Arrange

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                     new Sitecore.FakeDb.DbItem("Child1")
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());

                var scContext = new SitecoreService(database.Database);

                var parentItem = database.GetItem("/sitecore/content/Target");
                var parent = scContext.GetItem<CommentPage>("/sitecore/content/Target");

                using (new SecurityDisabler())
                {
                    parentItem.DeleteChildren();
                }

                var newItem = database.GetItem("/sitecore/content/Target/Child1");
                Assert.IsNull(newItem);

                //Act
                //    scContext.GlassContext.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(Comment)));
                var newClass = new Comment();
                newClass.Name = "Child1";
                using (new SecurityDisabler())
                {
                    scContext.Create(parent, newClass);
                }

                //Asset

                newItem = database.GetItem("/sitecore/content/Target/Child1");
                Assert.IsNotNull(newItem);
            }
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
