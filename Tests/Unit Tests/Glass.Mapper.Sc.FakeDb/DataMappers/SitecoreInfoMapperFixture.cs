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
using System.Linq.Expressions;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.FakeDb.Infrastructure;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Query;
using Sitecore.FakeDb;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{

    [TestFixture]
    public class SitecoreInfoMapperFixture
    {
        #region Method - MapToProperty

        [Test]
        [TestCase(SitecoreInfoType.ContentPath, "/TestItem")]
        [TestCase(SitecoreInfoType.DisplayName, "DataMappersEmptyItem DisplayName")]
        [TestCase(SitecoreInfoType.FullPath, "/sitecore/content/TestItem")]
        [TestCase(SitecoreInfoType.Key, "testitem")]
        [TestCase(SitecoreInfoType.MediaUrl, "/~/media/Test.ashx")]
        [TestCase(SitecoreInfoType.Name, "TestItem")]
        [TestCase(SitecoreInfoType.Path, "/sitecore/content/TestItem")]
        [TestCase(SitecoreInfoType.TemplateName, "TestTemplate")]
        [TestCase(SitecoreInfoType.Url, "/en/sitecore/content/TestItem.aspx")]
        [TestCase(SitecoreInfoType.Version, 1)]
        public void MapToProperty_SitecoreInfoType_GetsExpectedValueFromSitecore(
            SitecoreInfoType type, object expected
            )
        {
            //Assign
            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null, config));

            Sitecore.Context.Site = null;
            var templateId = ID.NewID;
            var itemId = new ID("031501A9C7F24596BD659276DA3A627A");

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbTemplate("TestTemplate", templateId),
                new Sitecore.FakeDb.DbItem("TestItem", itemId)
                {
                    Fields = { new FakeDbField(new ID("{B5E02AD9-D56F-4C41-A065-A133DB87BDEB}"), "DataMappersEmptyItem DisplayName")},
                    TemplateID = templateId,
                }
            })
            {
                Sitecore.Resources.Media.MediaProvider mediaProvider = Substitute.For<Sitecore.Resources.Media.MediaProvider>();
                mediaProvider
                      .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == itemId), Arg.Any<MediaUrlOptions>())
                      .Returns("/~/media/Test.ashx");

                using (new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider))
                {
                    var item = database.GetItem("/sitecore/content/TestItem");
                    Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                    var dataContext = new SitecoreDataMappingContext(null, item, null);

                    //Act
                    var value = mapper.MapToProperty(dataContext);

                    //Assert
                    Assert.AreEqual(expected, value);
                }
            }
        }

        [Test]
        [TestCase(SitecoreInfoMediaUrlOptions.Default)]
        [TestCase(SitecoreInfoMediaUrlOptions.RemoveExtension)]
        [TestCase(SitecoreInfoMediaUrlOptions.LowercaseUrls)]
        public void MapToProperty_MediaUrlWithFlag_ReturnsModifiedUrl(
            SitecoreInfoMediaUrlOptions option
            
            )
        {
            //Assign
            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = SitecoreInfoType.MediaUrl;
            config.MediaUrlOptions = option;
            mapper.Setup(new DataMapperResolverArgs(null, config));
            var itemId = new ID("031501A9C7F24596BD659276DA3A627A");
            string expected = "media url";

            Sitecore.Context.Site = null;

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem", itemId)
                {
                    Fields =
                    {
                        new FakeDbField(new ID("{B5E02AD9-D56F-4C41-A065-A133DB87BDEB}"),
                            "DataMappersEmptyItem DisplayName")
                    },
                }
            })
            {

                Func<MediaUrlOptions, bool> pred = x =>
                {
                    switch (option)
                    {
                        case SitecoreInfoMediaUrlOptions.Default:
                            return true;
                        case SitecoreInfoMediaUrlOptions.RemoveExtension:
                            return x.IncludeExtension == false;
                        case SitecoreInfoMediaUrlOptions.LowercaseUrls:
                            return x.LowercaseUrls == true;
                        default:
                            return false;
                    }
                };

                Sitecore.Resources.Media.MediaProvider mediaProvider =
                    Substitute.For<Sitecore.Resources.Media.MediaProvider>();
                mediaProvider
                    .GetMediaUrl(
                        Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == itemId),
                        Arg.Is<MediaUrlOptions>(x => pred(x))
                    )
                    .Returns(expected);

                using (new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider))
                {
                    var item = database.GetItem("/sitecore/content/TestItem");
                    Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                    var dataContext = new SitecoreDataMappingContext(null, item, null);

                    //Act
                    var value = mapper.MapToProperty(dataContext);

                    //Assert
                    Assert.AreEqual(expected, value);
                }
            }
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeNotSet_ThrowsException()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                SitecoreInfoType type = SitecoreInfoType.NotSet;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;

                var item = database.GetItem("/sitecore/Content/TestItem");

                Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                var dataContext = new SitecoreDataMappingContext(null, item, null);

                //Act
                Assert.Throws<MapperException>(() => mapper.Setup(new DataMapperResolverArgs(null, config)));

                //Assert
                //No asserts expect exception
            }
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeLanguage_ReturnsEnStringType()
        {

            //Assign

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var type = SitecoreInfoType.Language;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                config.PropertyInfo = new FakePropertyInfo(typeof(string), "StringField", typeof(Stub));
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var item = database.GetItem("/sitecore/Content/TestItem");

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
        public void MapToProperty_SitecoreInfoTypeItemuri_ReturnsFullItemUri()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var type = SitecoreInfoType.ItemUri;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                config.PropertyInfo = new FakePropertyInfo(typeof(string), "StringField", typeof(Stub));
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var item = database.GetItem("/sitecore/Content/TestItem");

                Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                var dataContext = new SitecoreDataMappingContext(null, item, null);

                //Act
                var value = mapper.MapToProperty(dataContext) as ItemUri;

                //Assert
                Assert.AreEqual(item.ID, value.ItemID);
                Assert.AreEqual(item.Language, value.Language);
                Assert.AreEqual(item.Database.Name, value.DatabaseName);
                Assert.AreEqual(item.Version, value.Version);
            }
        }


        [Test]
        public void MapToProperty_SitecoreInfoTypeLanguage_ReturnsEnLanguageType()
        {

            //Assign

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var type = SitecoreInfoType.Language;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var item = database.GetItem("/sitecore/Content/TestItem");

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

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var type = SitecoreInfoType.TemplateId;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var item = database.GetItem("/sitecore/Content/TestItem");
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

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var type = SitecoreInfoType.TemplateId;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                config.PropertyInfo = typeof(Stub).GetProperty("TemplateId");

                mapper.Setup(new DataMapperResolverArgs(null, config));

                var item = database.GetItem("/sitecore/Content/TestItem");
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
            ID templateId1 = ID.NewID;
            ID templateId2 = ID.NewID;

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbTemplate("TestTemplate1", templateId1),
                new Sitecore.FakeDb.DbTemplate("TestTemplate2", templateId2)
                {
                    new FakeDbField(new ID("{12C33F3F-86C5-43A5-AEB4-5598CEC45116}"), templateId1.ToString())
                },
                new Sitecore.FakeDb.DbItem("TestItem",ID.NewID, templateId2)
            })
            {
                var type = SitecoreInfoType.BaseTemplateIds;

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                config.PropertyInfo = typeof(Stub).GetProperty("BaseTemplateIds");

                mapper.Setup(new DataMapperResolverArgs(null, config));

                var item = database.GetItem("/sitecore/Content/TestItem");

                Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
                var dataContext = new SitecoreDataMappingContext(null, item, null);

                //Act
                IEnumerable<ID> results;
                using (new SecurityDisabler())
                {

                    results = mapper.MapToProperty(dataContext) as IEnumerable<ID>;
                }

                //Assert
                Assert.Greater(results.Count(), 1);
                Assert.IsTrue(results.All(x => x != item.TemplateID));
            }
        }

        #endregion

        #region Method - MapToCms

        [Test]
        public void MapToCms_SavingDisplayName_UpdatesTheDisplayNameField()
        {
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                //Assign
                var type = SitecoreInfoType.DisplayName;
                var expected = "new display name";

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var item = database.GetItem("/sitecore/Content/TestItem");

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

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var type = SitecoreInfoType.Name;
                var expected = "new  name";

                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = type;
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var item = database.GetItem("/sitecore/Content/TestItem");

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




