using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Glass.Mapper.Sc.DataMappers;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Integration.DataMappers
{

    [TestFixture]
    public class SitecoreInfoMapperFixture
    {
        private Database _db;

        [SetUp]
        public void Setup()
        {
            _db = Sitecore.Configuration.Factory.GetDatabase("master");
        }

        #region Method - MapToProperty

        [Test]
        [Sequential]
        public void MapToProperty_SitecoreInfoType_GetsExpectedValueFromSitecore(
            [Values(
                SitecoreInfoType.ContentPath,
                SitecoreInfoType.DisplayName,
                SitecoreInfoType.FullPath,
                SitecoreInfoType.Key,
                SitecoreInfoType.MediaUrl,
                SitecoreInfoType.Name,
                SitecoreInfoType.NotSet,
                SitecoreInfoType.Path,
                SitecoreInfoType.TemplateName,
                SitecoreInfoType.Url,
                SitecoreInfoType.Version
                )] SitecoreInfoType type,
            [Values(
                "/sitecore/content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem",
                "DataMappersEmptyItem DisplayName",
                "/sitecore/content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem",
                "datamappersemptyitem",
                "Not sure what this should be",
                "DataMappersEmptyItem",
                "",
                "/sitecore/content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem",
                "DataMappersEmptyItem",
                "not sure",
                1)] object expected
            )
        {
            //Assign
            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(config);

            using (new SecurityDisabler())
            {
                var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");

                Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                var dataContext = new SitecoreDataMappingContext(null, item);

                //Act
                var value = mapper.MapToProperty(dataContext);

                //Assert
                Assert.AreEqual(expected, value);
            }
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeLanguage_ReturnsEn()
        {
            Assert.Fail("Write test");
        }
        [Test]
        public void MapToProperty_SitecoreInfoTypeTemplateId_ReturnsEn()
        {
            Assert.Fail("Write test");
        }

        [Test]
        public void T()
        {

        }

        #endregion
    }
}
