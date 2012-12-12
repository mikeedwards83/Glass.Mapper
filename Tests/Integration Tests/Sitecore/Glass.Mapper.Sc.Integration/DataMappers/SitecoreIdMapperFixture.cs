using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Glass.Mapper.Sc.DataMappers;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreIdMapperFixture
    {
        private Database _db;

        [SetUp]
        public void Setup()
        {
            _db = Sitecore.Configuration.Factory.GetDatabase("master");
        }


        #region Property - ReadOnly

        [Test]
        public void ReadOnly_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreIdMapper();
            bool expected = true;

            //Act
            bool value = mapper.ReadOnly;


            //Assert
            Assert.AreEqual(expected,value);

        }



        #endregion

        #region Method - MapToProperty

        [Test]
        public void MapToProperty_ItemIdAsGuid_ReturnsIdAsGuid()
        {
            //Assign
            var mapper = new SitecoreIdMapper();
            var config = new SitecoreIdConfiguration();
            var property = typeof (Stub).GetProperty("GuidId");
            var item = _db.GetItem("/sitecore/content/Tests/DataMappers/SitecoreIdMapper/EmptyItem");

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");

            config.PropertyInfo = property;
            
            mapper.Setup(config);

            var dataContext = new SitecoreDataMappingContext(null, item, null);
            var expected = item.ID.Guid;

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void MapToProperty_ItemIdAsID_ReturnsIdAsID()
        {
            //Assign
            var mapper = new SitecoreIdMapper();
            var config = new SitecoreIdConfiguration();
            var property = typeof(Stub).GetProperty("IDId"); 

            config.PropertyInfo = property;

            mapper.Setup(config);

            var item = _db.GetItem("/sitecore/content/Tests/DataMappers/SitecoreIdMapper/EmptyItem");

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item, null);
            var expected = item.ID;

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void MapToProperty_ItemIdAsString_ThrowsException()
        {
            //Assign
            var mapper = new SitecoreIdMapper();
            var config = new SitecoreIdConfiguration();
            var property = typeof(Stub).GetProperty("StringId");

            config.PropertyInfo = property;

            mapper.Setup(config);

            var item = _db.GetItem("/sitecore/content/Tests/DataMappers/SitecoreIdMapper/EmptyItem");

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item, null);
            var expected = item.ID;

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            //Exception, not asserts
        }


        #endregion

        #region Stubs

        public class Stub
        {
            public Guid GuidId { get; set; }

            public ID IDId { get; set; }

            public string StringId { get; set; }
        }

        #endregion
    }
}
