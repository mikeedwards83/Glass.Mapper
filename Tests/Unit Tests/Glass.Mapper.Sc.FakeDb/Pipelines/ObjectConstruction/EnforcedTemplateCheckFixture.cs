using System;
using System.Linq;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.FakeDb.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.FakeDb.Pipelines.ObjectConstruction
{
    [TestFixture]
    public class EnforcedTemplateCheckFixture
    {
        [Test]
        public void Excute_ResultNotNull_NoChanges()
        {
            //Arrange

            var task = new EnforcedTemplateCheck();
            var args = new ObjectConstructionArgs(null, null, null, null);
            var expected = new object();
            args.Result = expected;

            //Act
            task.Execute(args);

            //Assert
            Assert.AreEqual(expected, args.Result);

        }

        [Test]
        public void Execute_EnforeTemplateNo_PipelineNotAborted()
        {
            //Arrange
            var task = new EnforcedTemplateCheck();
            var config = new SitecoreTypeConfiguration();
            config.EnforceTemplate = SitecoreEnforceTemplate.No;

            var args = new ObjectConstructionArgs(null, null, config, null);

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNull(args.Result);
            Assert.IsFalse(args.IsAborted);

        }

        [Test]
        public void Execute_EnforeTemplateOnlyDoesNotInheritTemplate_AbortsPipeline()
        {
            //Arrange
            var task = new EnforcedTemplateCheck();
            using (Db database = new Db
            {
                new DbTemplate(new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId)),
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId))
            })
            {
                var path = "/sitecore/content/Target";
                var item = database.GetItem(path);

                var config = new SitecoreTypeConfiguration();
                config.EnforceTemplate = SitecoreEnforceTemplate.Template;
                config.TemplateId = new ID(Guid.NewGuid());

                var typeContext = new SitecoreTypeCreationContext();
                typeContext.Item = item;

                var args = new ObjectConstructionArgs(null, typeContext, config, null);

                //Act
                task.Execute(args);

                //Assert
                Assert.IsNull(args.Result);
                Assert.IsTrue(args.IsAborted);

            }
        }


        [Test]
        public void Execute_EnforeTemplateOnlyInheritsTemplate_PipelineContinues()
        {
            //Arrange
            var task = new EnforcedTemplateCheck();


            using (Db database = new Db
            {
                new DbTemplate(new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId)),
                new Sitecore.FakeDb.DbItem("Target", ID.NewID,
                    new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId))
            })
            {
                var path = "/sitecore/content/Target";
                var item = database.GetItem(path);

                var config = new SitecoreTypeConfiguration();
                config.EnforceTemplate = SitecoreEnforceTemplate.Template;
                config.TemplateId = item.TemplateID;

                var typeContext = new SitecoreTypeCreationContext();
                typeContext.Item = item;

                var args = new ObjectConstructionArgs(null, typeContext, config, null);

                //Act
                task.Execute(args);

                //Assert
                Assert.IsNull(args.Result);
                Assert.IsFalse(args.IsAborted);
            }
        }



        [Test]
        public void Execute_EnforeTemplateAndBaseDoesNotInheritTemplate_AbortsPipeline()
        {
            //Arrange
            var task = new EnforcedTemplateCheck();


            using (Db database = new Db
            {
                new DbTemplate(new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId)),
                new Sitecore.FakeDb.DbItem("Target", ID.NewID,
                    new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId))
            })
            {
                var path = "/sitecore/content/Target";
                var item = database.GetItem(path);

                var config = new SitecoreTypeConfiguration();
                config.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;
                config.TemplateId = new ID(Guid.NewGuid());

                var typeContext = new SitecoreTypeCreationContext();
                typeContext.Item = item;

                var args = new ObjectConstructionArgs(null, typeContext, config, null);

                //Act

                using (new SecurityDisabler())
                {
                    task.Execute(args);
                }

                //Assert
                Assert.IsNull(args.Result);
                Assert.IsTrue(args.IsAborted);

            }
        }


        [Test]
        public void Execute_EnforeTemplateAndBaseInheritsTemplate_PipelineContinues()
        {
            //Arrange
            var task = new EnforcedTemplateCheck();


            using (Db database = new Db
            {
                new DbTemplate(new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId)),
                new Sitecore.FakeDb.DbItem("Target", ID.NewID,
                    new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId))
            })
            {
                var path = "/sitecore/content/Target";
                var item = database.GetItem(path);

                var config = new SitecoreTypeConfiguration();
                config.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;
                config.TemplateId = item.TemplateID;

                var typeContext = new SitecoreTypeCreationContext();
                typeContext.Item = item;

                var args = new ObjectConstructionArgs(null, typeContext, config, null);

                //Act
                task.Execute(args);

                //Assert
                Assert.IsNull(args.Result);
                Assert.IsFalse(args.IsAborted);
            }
        }

        [Test]
        public void Execute_EnforeTemplateAndBaseInheritsTemplateFromBase_PipelineContinues()
        {
            //Arrange
            var task = new EnforcedTemplateCheck();


            using (Db database = new Db
            {
                new DbTemplate(new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId)),
                new Sitecore.FakeDb.DbItem("Target", ID.NewID,
                    new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId))
            })
            {
                var path = "/sitecore/content/Target";
                var item = database.GetItem(path);

                var config = new SitecoreTypeConfiguration();
                config.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;

                using (new SecurityDisabler())
                {
                    config.TemplateId = item.Template.BaseTemplates.First().ID;

                    var typeContext = new SitecoreTypeCreationContext();
                    typeContext.Item = item;

                    var args = new ObjectConstructionArgs(null, typeContext, config, null);

                    //Act
                    task.Execute(args);

                    //Assert
                    Assert.IsNull(args.Result);
                    Assert.IsFalse(args.IsAborted);
                }
            }
        }

        [Test]
        public void Execute_EnforeTemplateAndBaseInheritsTemplateFromDeepBase_PipelineContinues()
        {
            //Arrange
            var task = new EnforcedTemplateCheck();


            ID baseTemplateId1 = ID.NewID;
            ID baseTemplateId2 = ID.NewID;


            using (Db database = new Db
            {
                new DbTemplate(new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId))
                 {
                  new DbField("__Base template")
                  {
                      Value =  baseTemplateId1.ToString()
                  }
                },
                new Sitecore.FakeDb.DbTemplate("base1", baseTemplateId1)
                {
                  new DbField("__Base template")
                  {
                      Value =  baseTemplateId2.ToString()
                  }
                },
                new Sitecore.FakeDb.DbTemplate("base2", baseTemplateId2),
                new Sitecore.FakeDb.DbItem("Target", ID.NewID,
                    new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId))
            })
            {
                var path = "/sitecore/content/Target";
                var item = database.GetItem(path);

                var config = new SitecoreTypeConfiguration();
                config.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;

                using (new SecurityDisabler())
                {
                    config.TemplateId = item.Template.BaseTemplates.First().BaseTemplates.First().ID;

                    var typeContext = new SitecoreTypeCreationContext();
                    typeContext.Item = item;

                    var args = new ObjectConstructionArgs(null, typeContext, config, null);

                    //Act
                    task.Execute(args);

                    //Assert
                    Assert.IsNull(args.Result);
                    Assert.IsFalse(args.IsAborted);
                }
            }
        }

    }
}
