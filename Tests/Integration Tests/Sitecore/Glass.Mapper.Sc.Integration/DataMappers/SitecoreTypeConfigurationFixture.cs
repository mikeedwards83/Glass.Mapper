using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        #endregion


        #region Stubs

        public class StubClass
        {
            
            public virtual ItemUri ItemUri { get; set; }

        }


        #endregion

    }
}
