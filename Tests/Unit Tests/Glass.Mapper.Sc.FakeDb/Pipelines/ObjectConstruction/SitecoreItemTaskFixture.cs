using System;
using System.Collections.Generic;
using Glass.Mapper.Diagnostics;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;
using NUnit.Framework;

namespace Glass.Mapper.Sc.FakeDb.Pipelines.ObjectConstruction
{
    [TestFixture]
    public class SitecoreItemTaskFixture
    {

        #region Execute

        [Test]
        public void Execute_ResultNotNull_NoChangeToResult()
        {
            //Arrange
            var task = new SitecoreItemTask();
            var context = new SitecoreTypeCreationContext();

            var args = new ObjectConstructionArgs(null, context, new SitecoreTypeConfiguration(), null, new ModelCounter());
            var expected = "some value";
            args.Result = expected;

            //Act
            task.Execute(args);

            //Assert
            Assert.AreEqual(expected, args.Result);
        }

        [Test]
        public void Execute_ResultNotSitecoreItemConfig_NoValueSet()
        {
            //Arrange
            var task = new SitecoreItemTask();
            var context = new SitecoreTypeCreationContext();

            var args = new ObjectConstructionArgs(null, context, new SitecoreTypeConfiguration(), null, new ModelCounter());
        
            //Act
            task.Execute(args);

            //Assert
            Assert.IsNull(args.Result);
        }

        [Test]
        public void Execute_ItemConfig_ItemSet()
        {
            //Arrange
            var task = new SitecoreItemTask();
            var context = new SitecoreTypeCreationContext();
            context.Item = Mapper.Sc.Utilities.CreateFakeItem(new Dictionary<Guid, string>(), "test");
            var args = new ObjectConstructionArgs(null, context, SitecoreItemResolverTask.Config, null, new ModelCounter());
            
            //Act
            task.Execute(args);

            //Assert
            Assert.AreEqual(context.Item, args.Result);
        }

        #endregion

    }
}
