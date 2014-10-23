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
using System.Threading;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Glass.Mapper.Sc.DataMappers;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.FakeDb;
using Sitecore.FakeDb.Resources.Media;
using Sitecore.Globalization;
using Sitecore.Resources.Media;
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
                "/SitecoreInfoMapper", //content path
                "DataMappersEmptyItem DisplayName", //DisplayName
                "/sitecore/content/SitecoreInfoMapper", //FullPath
                "sitecoreinfomapper", //Key
                "{3C09BE13-40D2-436D-B8F0-6EB6033B7F41}media", //MediaUrl
                "SitecoreInfoMapper", //Name
                "/sitecore/content/SitecoreInfoMapper", //Path
                "DataMappersEmptyItem", //TemplateName
                "/en/sitecore/content/SitecoreInfoMapper.aspx", //Url
                1 //version
                )] object expected
            )
        {
            //Arrange
            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null, config));

            string itemName = "SitecoreInfoMapper";
            var templateId = new ID(Guid.NewGuid());
            var itemId = new ID("3C09BE13-40D2-436D-B8F0-6EB6033B7F41");

            //we have to setup a custom media provider for fake db
            var provider = global::Sitecore.Resources.Media.MediaManager.Provider as FakeMediaProvider;
            if (provider != null)
            {
                provider.LocalProvider.Value = new GlassMediaProvider();
            }

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new DbTemplate("DataMappersEmptyItem", templateId)
                {
                    Glass.Mapper.Sc.Global.Fields.DisplayName
                },

                new Sitecore.FakeDb.DbItem(itemName, itemId, templateId)
                {
                    
                    new DbField(Glass.Mapper.Sc.Global.Fields.DisplayName)
                    {
                        Value = "DataMappersEmptyItem DisplayName"
                    }
                }
            })
            {
                var database = Sitecore.Configuration.Factory.GetDatabase("master");

                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                var dataContext = new SitecoreDataMappingContext(null, item, null);

                //Act
                var value = mapper.MapToProperty(dataContext);

                //Assert
                Assert.AreEqual(expected, value);
            }


        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void MapToProperty_SitecoreInfoTypeNotSet_ThrowsException()
        {
            //Assign
            string itemName = "SitecoreInfoMapper";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new DbTemplate("DataMappersEmptyItem")
                {
                    Glass.Mapper.Sc.Global.Fields.DisplayName
                },

                new Sitecore.FakeDb.DbItem(itemName)
                {

                    new DbField(Glass.Mapper.Sc.Global.Fields.DisplayName)
                    {
                        Value = "DataMappersEmptyItem DisplayName"
                    }
                }
            })
            {
                SitecoreInfoType type = SitecoreInfoType.NotSet;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));


                Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                var dataContext = new SitecoreDataMappingContext(null, item, null);

                //Act
                var value = mapper.MapToProperty(dataContext);

                //Assert
                //No asserts expect exception
            }
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeLanguage_ReturnsEnStringType()
        {

            //Assign

            string itemName = "SitecoreInfoMapper";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new DbTemplate("DataMappersEmptyItem")
                {
                    Glass.Mapper.Sc.Global.Fields.DisplayName
                },

                new Sitecore.FakeDb.DbItem(itemName)
                {

                    new DbField(Glass.Mapper.Sc.Global.Fields.DisplayName)
                    {
                        Value = "DataMappersEmptyItem DisplayName"
                    }
                }
            })
            {

                var type = SitecoreInfoType.Language;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                config.PropertyInfo = new FakePropertyInfo(typeof(string), "StringField", typeof(Stub));
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                var expected = item.Language.Name;


                Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                var dataContext = new SitecoreDataMappingContext(null, item, null);

                //Act
                var value = mapper.MapToProperty(dataContext);

                //Assert
                Assert.AreEqual(expected, value);
            }
        }
        [Test]
        public void MapToProperty_SitecoreInfoTypeLanguage_ReturnsEnLanguageType()
        {
            //Assign

            string itemName = "SitecoreInfoMapper";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new DbTemplate("DataMappersEmptyItem")
                {
                    Glass.Mapper.Sc.Global.Fields.DisplayName
                },

                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(Glass.Mapper.Sc.Global.Fields.DisplayName)
                    {
                        Value = "DataMappersEmptyItem DisplayName"
                    }
                }
            })
            {
                var type = SitecoreInfoType.Language;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                var expected = item.Language;

                Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                var dataContext = new SitecoreDataMappingContext(null, item, null);

                //Act
                var value = mapper.MapToProperty(dataContext);

                //Assert
                Assert.AreEqual(expected, value);
            }
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeTemplateId_ReturnsTemplateIdAsGuid()
        {
            //Assign


            string itemName = "SitecoreInfoMapper";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new DbTemplate("DataMappersEmptyItem")
                {
                    Glass.Mapper.Sc.Global.Fields.DisplayName
                },

                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(Glass.Mapper.Sc.Global.Fields.DisplayName)
                    {
                        Value = "DataMappersEmptyItem DisplayName"
                    }
                }
            })
            {
                var type = SitecoreInfoType.TemplateId;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                var expected = item.TemplateID.Guid;

                Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                var dataContext = new SitecoreDataMappingContext(null, item, null);

                //Act
                var value = mapper.MapToProperty(dataContext);

                //Assert
                Assert.AreEqual(expected, value);
            }
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeTemplateId_ReturnsTemplateIdAsID()
        {
            //Assign

            string itemName = "SitecoreInfoMapper";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new DbTemplate("DataMappersEmptyItem")
                {
                    Glass.Mapper.Sc.Global.Fields.DisplayName
                },

                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(Glass.Mapper.Sc.Global.Fields.DisplayName)
                    {
                        Value = "DataMappersEmptyItem DisplayName"
                    }
                }
            })
            {
                var type = SitecoreInfoType.TemplateId;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                config.PropertyInfo = typeof(Stub).GetProperty("TemplateId");

                mapper.Setup(new DataMapperResolverArgs(null, config));


                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                var expected = item.TemplateID;

                Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                var dataContext = new SitecoreDataMappingContext(null, item, null);

                //Act
                var value = mapper.MapToProperty(dataContext);

                //Assert
                Assert.AreEqual(expected, value);
            }
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeBaseTemplateIds_ReturnsBaseTemplateIds()
        {
            //Assign

            string itemName = "SitecoreInfoMapper";
            var templateId = Sitecore.Data.ID.NewID;
              var baseTemplateIdOne = Sitecore.Data.ID.NewID;
  var baseTemplateIdTwo = Sitecore.Data.ID.NewID;

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new Sitecore.FakeDb.DbTemplate("base one", baseTemplateIdOne),
                new Sitecore.FakeDb.DbTemplate("base two", baseTemplateIdTwo),
                new DbTemplate("DataMappersEmptyItem",templateId)
                {
                     BaseIDs = new[] { baseTemplateIdOne, baseTemplateIdTwo },
                },

                new Sitecore.FakeDb.DbItem(itemName, Sitecore.Data.ID.NewID, templateId)
                {
                    new DbField(Glass.Mapper.Sc.Global.Fields.DisplayName)
                    {
                        Value = "DataMappersEmptyItem DisplayName"
                    }
                }
            })
            {
                var type = SitecoreInfoType.BaseTemplateIds;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                config.PropertyInfo = typeof(Stub).GetProperty("BaseTemplateIds");

                mapper.Setup(new DataMapperResolverArgs(null, config));

                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                var dataContext = new SitecoreDataMappingContext(null, item, null);

                //Act
                IEnumerable<ID> results;
                using (new SecurityDisabler())
                {

                    results = mapper.MapToProperty(dataContext) as IEnumerable<ID>;
                }

                //Assert
                Assert.AreEqual(results.Count(), 2);
                Assert.IsTrue(results.All(x => x != item.TemplateID));
            }
        }

        #endregion

        #region Method - MapToCms

        [Test]
        public void MapToCms_SavingDisplayName_UpdatesTheDisplayNameField()
        {
            //Assign

            string itemName = "SitecoreInfoMapper";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new DbTemplate("DataMappersEmptyItem")
                {
                    Glass.Mapper.Sc.Global.Fields.DisplayName
                },

                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(Glass.Mapper.Sc.Global.Fields.DisplayName)
                    {
                        Value = "DataMappersEmptyItem DisplayName"
                    }
                }
            })
            {
                var type = SitecoreInfoType.DisplayName;
                var expected = "new display name";

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

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
            
        }

        [Test]
        public void MapToCms_SavingName_UpdatesTheItemName()
        {
            //Assign

             string itemName = "SitecoreInfoMapper";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new DbTemplate("DataMappersEmptyItem")
                {
                    Glass.Mapper.Sc.Global.Fields.DisplayName
                },

                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(Glass.Mapper.Sc.Global.Fields.DisplayName)
                    {
                        Value = "DataMappersEmptyItem DisplayName"
                    }
                }
            })
            {

                var type = SitecoreInfoType.Name;
                var expected = "new  name";

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));


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
        }

        #endregion

        #region Stubs

        public class Stub
        {
            public ID TemplateId { get; set; }
            public IEnumerable<ID> BaseTemplateIds { get; set; }
            public string StringField { get; set; }
        }

        #endregion
    }
}




