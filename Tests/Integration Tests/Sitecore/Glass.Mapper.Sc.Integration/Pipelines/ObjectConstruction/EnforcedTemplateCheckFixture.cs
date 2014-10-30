using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Integration.Pipelines.ObjectConstruction
{
    [TestFixture]
    public class EnforcedTemplateCheckFixture 
    {
        [Test]
        public void Excute_ResultNotNull_NoChanges()
        {
            //Arrange
            
            var task = new EnforcedTemplateCheck();
            var args = new ObjectConstructionArgs(null,null,null,null);
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
         
            
            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var path = "/sitecore/content/Tests/Pipelines/ObjectConstruction/EnforcedTemplateCheck/Target";
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


        [Test]
        public void Execute_EnforeTemplateOnlyInheritsTemplate_PipelineContinues()
        {
            //Arrange
            var task = new EnforcedTemplateCheck();


            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var path = "/sitecore/content/Tests/Pipelines/ObjectConstruction/EnforcedTemplateCheck/Target";
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



        [Test]
        public void Execute_EnforeTemplateAndBaseDoesNotInheritTemplate_AbortsPipeline()
        {
            //Arrange
            var task = new EnforcedTemplateCheck();


            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var path = "/sitecore/content/Tests/Pipelines/ObjectConstruction/EnforcedTemplateCheck/Target";
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


        [Test]
        public void Execute_EnforeTemplateAndBaseInheritsTemplate_PipelineContinues()
        {
            //Arrange
            var task = new EnforcedTemplateCheck();


            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var path = "/sitecore/content/Tests/Pipelines/ObjectConstruction/EnforcedTemplateCheck/Target";
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

        [Test]
        public void Execute_EnforeTemplateAndBaseInheritsTemplateFromBase_PipelineContinues()
        {
            //Arrange
            var task = new EnforcedTemplateCheck();


            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var path = "/sitecore/content/Tests/Pipelines/ObjectConstruction/EnforcedTemplateCheck/Target";
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
}
