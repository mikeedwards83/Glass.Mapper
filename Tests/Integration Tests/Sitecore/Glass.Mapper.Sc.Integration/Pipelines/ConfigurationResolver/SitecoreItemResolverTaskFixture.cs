using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Pipelines.ConfigurationResolver;
using NUnit.Framework;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Integration.Pipelines.ConfigurationResolver
{
    [TestFixture]
    public class SitecoreItemResolverTaskFixture
    {
        #region Execute

        [Test]
        public void Execute_ResultIsNotNull_DoesNothing()
        {
            //Arrange
            var expected = new SitecoreTypeConfiguration();
            var task = new SitecoreItemResolverTask();
            var args = new ConfigurationResolverArgs(null, null, null, null);
            args.Result = expected;

            //Act
            task.Execute(args);

            //Arrange
            Assert.AreEqual(expected, args.Result);
        }

        [Test]
        public void Execute_RequestedTypeNotItem_ResultNull()
        {
            //Arrange
            SitecoreTypeConfiguration expected = null;
            var task = new SitecoreItemResolverTask();
            var args = new ConfigurationResolverArgs(null, null, typeof(string), null);

            //Act
            task.Execute(args);

            //Arrange
            Assert.AreEqual(expected, args.Result);
        }

        [Test]
        public void Execute_RequestedTypeItem_ConfigForItem()
        {
            //Arrange
            SitecoreTypeConfiguration expected = SitecoreItemResolverTask.Config;
            var task = new SitecoreItemResolverTask();
            var args = new ConfigurationResolverArgs(null, null, typeof(Item), null);

            //Act
            task.Execute(args);

            //Arrange
            Assert.AreEqual(expected, args.Result);
        }



        #endregion
    }
}
