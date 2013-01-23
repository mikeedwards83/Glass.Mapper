/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Glass.Mapper.Sc.DataMappers;
using Sitecore.Data;
using Sitecore.Data.Managers;
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
                SitecoreInfoType.Path,
                SitecoreInfoType.TemplateName,
                SitecoreInfoType.Url,
                SitecoreInfoType.Version
                )] SitecoreInfoType type,
            [Values(
                "/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem", //content path
                "DataMappersEmptyItem DisplayName", //DisplayName
                "/sitecore/content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem", //FullPath
                "datamappersemptyitem", //Key
                "/~/media/031501A9C7F24596BD659276DA3A627A.ashx", //MediaUrl
                "DataMappersEmptyItem", //Name
                "/sitecore/content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem", //Path
                "DataMappersEmptyItem", //TemplateName
                "/en/sitecore/content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem.aspx", //Url
                1 //version
                )] object expected
            )
        {
            //Assign
            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null,config));

            Sitecore.Context.Site = null;

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item, null);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        [ExpectedException(typeof (MapperException))]
        public void MapToProperty_SitecoreInfoTypeNotSet_ThrowsException()
        {
            //Assign
            SitecoreInfoType type = SitecoreInfoType.NotSet;

            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null,config));

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item, null);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            //No asserts expect exception
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeLanguage_ReturnsEn()
        {

            //Assign
            var type = SitecoreInfoType.Language;

            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null,config));

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");

            var expected = item.Language;


            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item, null);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeTemplateId_ReturnsTemplateIdAsGuid()
        {
            //Assign
            var type = SitecoreInfoType.TemplateId;

            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null,config));

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");
            var expected = item.TemplateID.Guid;

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item, null);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeTemplateId_ReturnsTemplateIdAsID()
        {
            //Assign
            var type = SitecoreInfoType.TemplateId;

            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            config.PropertyInfo = typeof (Stub).GetProperty("TemplateId");

            mapper.Setup(new DataMapperResolverArgs(null,config));

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");
            var expected = item.TemplateID;

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item, null);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        #endregion

        #region Method - MapToCms

        [Test]
        public void MapToCms_SavingDisplayName_UpdatesTheDisplayNameField()
        {
            //Assign
            var type = SitecoreInfoType.DisplayName;
            var expected = "new display name";

            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null,config));

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");


            var dataContext = new SitecoreDataMappingContext(null, item, null);
            dataContext.PropertyValue = expected;

            string actual = string.Empty;

            //Act
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                mapper.MapToCms(dataContext);
                actual = item[Global.Fields.DisplayName];
                item.Editing.CancelEdit();
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MapToCms_SavingName_UpdatesTheItemName()
        {
            //Assign
            var type = SitecoreInfoType.Name;
            var expected = "new  name";

            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null,config));

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");


            var dataContext = new SitecoreDataMappingContext(null, item, null);
            dataContext.PropertyValue = expected;

            string actual = string.Empty;

            //Act
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                mapper.MapToCms(dataContext);
                actual = item.Name;
                item.Editing.CancelEdit();
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Stubs

        public class Stub
        {
            public ID TemplateId { get; set; }
        }

        #endregion
    }
}



