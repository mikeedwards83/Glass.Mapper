


using System;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.FakeDb.Infrastructure;
using Glass.Mapper.Sc.IoC;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.Resources.Media;
using Image = Glass.Mapper.Sc.Fields.Image;

#if SC90 || SC91 || SC92  || SC93 || SC100 || SC1001
using Sitecore.Abstractions;
using Sitecore.DependencyInjection;
#endif

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


#if SC90 || SC91 || SC92  || SC93 || SC100 || SC1001

                var mediaUrlProvider = Substitute.For<BaseMediaManager>();

                SitecoreVersionAbstractions.MediaManager = new LazyResetable<BaseMediaManager>(() => mediaUrlProvider);

                mediaUrlProvider
                    .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaId), Arg.Any<MediaUrlOptions>())
                    .Returns("/~/media/Test.ashx");

#else
                Sitecore.Resources.Media.MediaProvider mediaProvider =
                    Substitute.For<Sitecore.Resources.Media.MediaProvider>();
                mediaProvider
                     .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaId), Arg.Any<MediaUrlOptions>())
                      .Returns("/~/media/Test.ashx");

                new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider);
#endif




                var item = database.GetItem("/sitecore/content/TestItem");
                var field = item.Fields[FieldName];
                var mapper = new SitecoreFieldImageMapper();

                //Act
                var result = mapper.GetField(field, new SitecoreFieldConfiguration(), null) as Image;

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
                Assert.AreEqual(true, result.MediaExists);
            }
        }

        [Test]
        public void GetField_ImageInField_MissingMediaItem_ReturnsImageObjectWithSrc()
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
                }

            })
            {

#if SC90 || SC91 || SC92  || SC93 || SC100 || SC1001

                var mediaUrlProvider = Substitute.For<BaseMediaManager>();

                SitecoreVersionAbstractions.MediaManager = new LazyResetable<BaseMediaManager>(() => mediaUrlProvider);

                mediaUrlProvider
                    .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaId), Arg.Any<MediaUrlOptions>())
                    .Returns("/~/media/Test.ashx");

#else
                Sitecore.Resources.Media.MediaProvider mediaProvider =
                    Substitute.For<Sitecore.Resources.Media.MediaProvider>();
                mediaProvider
                     .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaId), Arg.Any<MediaUrlOptions>())
                      .Returns("/~/media/Test.ashx");

                new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider);
#endif


             

                    var item = database.GetItem("/sitecore/content/TestItem");
                    var field = item.Fields[FieldName];
                    var mapper = new SitecoreFieldImageMapper();

                    //Act
                    var result = mapper.GetField(field, new SitecoreFieldConfiguration(), null) as Image;

                    //Assert
                    Assert.IsEmpty(result.Alt);
                    // Assert.Equals(null, result.Border);
                    Assert.AreEqual(string.Empty, result.Class);
                    Assert.AreEqual(15, result.HSpace);
                    Assert.AreEqual(0, result.Height);
                    Assert.AreEqual(new Guid("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}"), result.MediaId);
                    Assert.IsTrue(string.IsNullOrEmpty(result.Src));
                    Assert.AreEqual(20, result.VSpace);
                    Assert.AreEqual(0, result.Width);
                    Assert.AreEqual(false, result.MediaExists);
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
                var options = new GetItemOptionsParams();

                service.Config = new Config();

                var context = new SitecoreDataMappingContext(null, null, service, options);

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
                var options = new GetItemOptionsParams();

                var service = Substitute.For<ISitecoreService>();
                service.Config = new Config();

                var context = new SitecoreDataMappingContext(null, null, service, options);


                //Act
                var result = mapper.GetField(field, null, context) as Image;

                //Assert
                Assert.IsNull(result);
            }
        }
        [Test]
        [TestCase(SitecoreMediaUrlOptions.Default)]
        [TestCase(SitecoreMediaUrlOptions.RemoveExtension)]
        [TestCase(SitecoreMediaUrlOptions.LowercaseUrls)]
        public void GetField_FieldNotNull_ReturnsNullImageObject(
            SitecoreMediaUrlOptions option

        )
        {
            //Assign
            var config = new SitecoreFieldConfiguration();
            config.MediaUrlOptions = option;
            string expected = "/~/media/Test.ashx";
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


                Func<MediaUrlOptions, bool> pred = x =>
                {
                    switch (option)
                    {
                        case SitecoreMediaUrlOptions.Default:
                            return true;
                        case SitecoreMediaUrlOptions.RemoveExtension:
                            return x.IncludeExtension == false;
                        case SitecoreMediaUrlOptions.LowercaseUrls:
                            return x.LowercaseUrls == true;
                        default:
                            return false;
                    }
                };






#if SC90 || SC91 || SC92  || SC93 || SC100 || SC1001

                var mediaUrlProvider = Substitute.For<BaseMediaManager>();

                SitecoreVersionAbstractions.MediaManager = new LazyResetable<BaseMediaManager>(() => mediaUrlProvider);

                mediaUrlProvider
                    .GetMediaUrl(
                        Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaId),
                        Arg.Is<MediaUrlOptions>(x => pred(x))
                    )
                    .Returns(expected);

#else
                Sitecore.Resources.Media.MediaProvider mediaProvider =
                    Substitute.For<Sitecore.Resources.Media.MediaProvider>();
                mediaProvider
                    .GetMediaUrl(
                        Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaId),
                        Arg.Is<MediaUrlOptions>(x => pred(x))
                    )
                    .Returns(expected);

                new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider);
#endif


                    var item = database.GetItem("/sitecore/content/TestItem");
                    var field = item.Fields[FieldName];
                    var mapper = new SitecoreFieldImageMapper();
                    mapper.Setup(new DataMapperResolverArgs(null, config));
                    //Act
                    var result = mapper.GetField(field, config, null) as Image;

                    //Assert
                    Assert.AreEqual("test alt", result.Alt);
                    // Assert.Equals(null, result.Border);
                    Assert.AreEqual(string.Empty, result.Class);
                    Assert.AreEqual(15, result.HSpace);
                    Assert.AreEqual(480, result.Height);
                    Assert.AreEqual(new Guid("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}"), result.MediaId);
                    Assert.IsTrue(result.Src.EndsWith(expected));
                    Assert.AreEqual(20, result.VSpace);
                    Assert.AreEqual(640, result.Width);
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




