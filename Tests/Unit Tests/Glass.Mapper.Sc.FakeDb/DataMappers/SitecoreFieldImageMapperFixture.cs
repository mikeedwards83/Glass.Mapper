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
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.IoC;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Image = Glass.Mapper.Sc.Fields.Image;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreFieldImageMapperFixture 
    {

        protected const string FieldName = "Field";

        #region Method - GetField

        [Test]
        public void GetField_ImageInField_ReturnsImageObject()
        {
            //Assign
            var fieldValue =
                "<image mediaid=\"{D897833C-1F53-4FAE-B54B-BB5B11B8F851}\" mediapath=\"/Files/20121222_001405\" src=\"~/media/D897833C1F534FAEB54BBB5B11B8F851.ashx\" hspace=\"15\" vspace=\"20\" />";
            var mediaId = new ID("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}");

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
                {
                    new DbField(FieldName)
                    {
                        Value = fieldValue
                    }
                },
                new Sitecore.FakeDb.DbItem("MediaItem", mediaId)
                {
                    new DbField("alt") {Value = "test alt"},
                    new DbField("height") {Value = "480"},
                    new DbField("width") {Value = "640"},
                }
            })
            {

                Sitecore.Resources.Media.MediaProvider mediaProvider = Substitute.For<Sitecore.Resources.Media.MediaProvider>();
                mediaProvider
                      .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaId))
                      .Returns("/~/media/Test.ashx");

                using (new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider))
                {

                    var item = database.GetItem("/sitecore/content/TestItem");
                    var field = item.Fields[FieldName];
                    var mapper = new SitecoreFieldImageMapper();

                    //Act
                    var result = mapper.GetField(field, null, null) as Image;

                    //Assert
                    Assert.AreEqual("test alt", result.Alt);
                    // Assert.Equals(null, result.Border);
                    Assert.AreEqual(string.Empty, result.Class);
                    Assert.AreEqual(15, result.HSpace);
                    Assert.AreEqual(480, result.Height);
                    Assert.AreEqual(new Guid("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}"), result.MediaId);
                    Assert.IsTrue(result.Src.EndsWith("/~/media/Test.ashx"));
                    Assert.AreEqual(20, result.VSpace);
                    Assert.AreEqual(640, result.Width);
                }
            }
        }

        [Test]
        public void GetField_ImageFieldEmpty_ReturnsNull()
        {
            //Assign
            var fieldValue = string.Empty;

		    using (Db database = new Db
		    {
		        new Sitecore.FakeDb.DbItem("TestItem")
		        {
		            new DbField(FieldName)
		            {
		                Value = fieldValue
		            }
		        }
		    })
		    {
		        var item = database.GetItem("/sitecore/content/TestItem");
		        var field = item.Fields[FieldName];

		        var context = Context.Create(new DependencyResolver(new Config()));
		        var service = new SitecoreService(database.Database, context);

		        //Act
		        var result =
		            service.GetItem<StubImage>("/sitecore/content/TestItem");

		        //Assert
		        Assert.IsNull(result.Field);
		    }
        }

        [Test]
        public void GetField_FieldIsEmpty_ReturnsNullImageObject()
        {
            //Assign
            var fieldValue = string.Empty;

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
                {
                    new DbField(FieldName)
                    {
                        Value = fieldValue
                    }
                }
            })
            {
		        var item = database.GetItem("/sitecore/content/TestItem");
                var field = item.Fields[FieldName];
                var mapper = new SitecoreFieldImageMapper();
                var service = Substitute.For<ISitecoreService>();
                service.Config = new Config();

                var context = new SitecoreDataMappingContext(null, null, service);

                //Act
                var result = mapper.GetField(field, null, context) as Image;

                //Assert
                Assert.IsNull(result);
            }
        }

        [Test]
        public void GetField_FieldIsNull_ReturnsNullImageObject()
        {
            //Assign
            string fieldValue = null;

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
                {
                    new DbField(FieldName)
                    {
                        Value = fieldValue
                    }
                }
            })
            {
                var item = database.GetItem("/sitecore/content/TestItem");
                var field = item.Fields[FieldName];
                var mapper = new SitecoreFieldImageMapper();

                var service = Substitute.For<ISitecoreService>();
                service.Config = new Config();

                var context = new SitecoreDataMappingContext(null, null, service);


                //Act
                var result = mapper.GetField(field, null, context) as Image;

                //Assert
                Assert.IsNull(result);
            }
        }


        #endregion

        #region Method - SetField

        [Test]
        public void SetField_ImagePassed_ReturnsPopulatedField()
        {
            //Assign
            var expected =
                "<image mediaid=\"{D897833C-1F53-4FAE-B54B-BB5B11B8F851}\" width=\"640\" vspace=\"50\" height=\"480\" hspace=\"30\" alt=\"test alt\" />";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
                {
                    new DbField(FieldName)
                },
                new Sitecore.FakeDb.DbItem("MediaItem", new ID("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}"))
            })
            {
                var item = database.GetItem("/sitecore/content/TestItem");
                var field = item.Fields[FieldName];
                var mapper = new SitecoreFieldImageMapper();
                var image = new Image()
                {
                    Alt = "test alt",
                    HSpace = 30,
                    Height = 480,
                    MediaId = new Guid("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}"),
                    VSpace = 50,
                    Width = 640,
                    Border = String.Empty,
                    Class = String.Empty

                };

                using (new ItemEditing(item, true))
                {
                    field.Value = string.Empty;
                }

                //Act
                using (new ItemEditing(item, true))
                {
                    mapper.SetField(field, image, null, null);
                }
                //Assert
                AssertHtml.AreHtmlElementsEqual(expected, field.Value, "image");

            }
        }

        [Test]
        public void SetField_JustImageId_ReturnsPopulatedField()
        {
            //Assign
            var expected =
                "<image mediaid=\"{D897833C-1F53-4FAE-B54B-BB5B11B8F851}\" />";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
                {
                    new DbField(FieldName)
                },
                new Sitecore.FakeDb.DbItem("MediaItem", new ID("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}"))
            })
            {
                var item = database.GetItem("/sitecore/content/TestItem");
                var field = item.Fields[FieldName];
                var mapper = new SitecoreFieldImageMapper();
                var image = new Image()
                {
                    MediaId = new Guid("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}"),
                };

                using (new ItemEditing(item, true))
                {
                    field.Value = string.Empty;
                }

                //Act
                using (new ItemEditing(item, true))
                {
                    mapper.SetField(field, image, null, null);
                }
                //Assert
                AssertHtml.AreHtmlElementsEqual(expected, field.Value, "image");

            }
        }

        #endregion

        #region Stubs

        public class StubImage
        {
            public virtual Image Field { get; set; }
        }

        #endregion

    }
}




