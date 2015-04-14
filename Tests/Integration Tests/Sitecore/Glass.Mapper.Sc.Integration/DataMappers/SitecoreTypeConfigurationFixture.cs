using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Publishing;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreTypeConfigurationFixture
    {
        #region ResolveItem

        [Test]
        public void ResolveItem()
        {
            //Arrange 

            var config = new SitecoreTypeConfiguration();
            config.ItemUriConfig = new SitecoreInfoConfiguration();
            config.ItemUriConfig.PropertyInfo = typeof(StubClass).GetProperty("ItemUri");
            
            //TODO get a proper items
            string path = "/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target";
            var database = Factory.GetDatabase("master");
            var expected = database.GetItem(path);

            var instance = new StubClass();
            instance.ItemUri = new ItemUri(expected.ID, expected.Language, expected.Version, expected.Database);


            //Act
            var result = config.ResolveItem(instance, database);

            //Assert
            Assert.AreEqual(expected.ID, result.ID);
            Assert.AreEqual(expected.Version, result.Version);
            Assert.AreEqual(expected.Language, result.Language);


        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ResolveItem_NoId_ThrowsException()
        {
            //Arrange 

            var config = new SitecoreTypeConfiguration();
            config.Type = typeof (StubClassNoId);

            config.AddProperty(
                new SitecoreFieldConfiguration()
                {
                    PropertyInfo = typeof (StubClassNoId).GetProperty("Field")
                });

            var instance = new StubClassNoId();

            var database = Factory.GetDatabase("master");



            //Act
            var result = config.ResolveItem(instance, database);

            //Assert


        }

        #endregion


        #region Stubs

        public class StubClassNoId : StubInterface
        {
            public virtual string Field { get; set; }
        }

        public interface StubInterface
        {
            
        }
        public class StubClass
        {
            
            public virtual ItemUri ItemUri { get; set; }

        }


        #endregion

    }
}
