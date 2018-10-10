using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.FakeDb.Infrastructure;
using Glass.Mapper.Sc.FakeDb.Infrastructure.Pipelines.RenderField;
using NUnit.Framework;
using Sitecore.FakeDb;
using Sitecore.Sites;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreFieldStringMapperFixture 
    {
        protected const string FieldName = "Field";


        #region Method - GetField

        [Test]
        public void GetField_FieldContainsData_StringIsReturned()
        {
            //Assign
            var fieldValue = "<p>hello world</p>";


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

                var mapper = new SitecoreFieldStringMapper();
                var config = new SitecoreFieldConfiguration();

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                //Act
                var result = mapper.GetField(field, config, null) as string;

                //Assert
                Assert.AreEqual(fieldValue, result);
            }
        }

        [Test]
        public void GetField_ForceRenderFieldPipeline_StringIsReturned()
        {
            //Assign

            var fieldValue = SimpleRenderField.ReplacementKey + "<p>hello world</p>";
            var expected = SimpleRenderField.ReplacementValue + "<p>hello world</p>";

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
                using (new FakeSite())
                {

                    var item = database.GetItem("/sitecore/content/TestItem");
                    var field = item.Fields[FieldName];

                    var mapper = new SitecoreFieldStringMapper();
                    var config = new SitecoreFieldConfiguration();
                    config.Setting = SitecoreFieldSettings.ForceRenderField;

                    using (new ItemEditing(item, true))
                    {
                        field.Value = fieldValue;
                    }

                    Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");
                    Sitecore.Context.Site.SetDisplayMode(DisplayMode.Preview, DisplayModeDuration.Remember);


                    //Act
                    var result = mapper.GetField(field, config, null) as string;

                    //Assert

                    Sitecore.Context.Site = null;

                    Assert.AreEqual(expected, result);
                }
            }

        }
        [Test]
        public void SetField_ForceRenderFieldPipeline_ThrowsException()
        {
            //Assign

            var fieldValue = "<p>hello world</p>";
            var expected = "&lt;p&gt;hello world&lt;/p&gt;";

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

                var mapper = new SitecoreFieldStringMapper();
                var config = new SitecoreFieldConfiguration();
                config.Setting = SitecoreFieldSettings.ForceRenderField;
                config.PropertyInfo = new FakePropertyInfo(typeof(string), "String", typeof(StubClass));
                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }



                //Act
                Assert.Throws<NotSupportedException>(()=> mapper.SetField(field, fieldValue, config, null));

                //Assert
            }

        }

        [Test]
        public void GetField_RichText_ValueGoesByRenderFieldPipeline()
        {
            //Assign
            var fieldBase =
                "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";
            var fieldValue = SimpleRenderField.ReplacementKey + fieldBase;
               ;
            var expected = SimpleRenderField.ReplacementValue + fieldBase;

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
                {
                    new DbField(FieldName)
                    {
                        Value = fieldValue,
                        Type = "Rich Text"
                    }
                }
            })
            {
                using (new FakeSite())
                {
                    var item =
                        database.GetItem("/sitecore/content/TestItem");
                    var field = item.Fields[FieldName];

                    var mapper = new SitecoreFieldStringMapper();
                    var config = new SitecoreFieldConfiguration();

                    Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");
                    Sitecore.Context.Site.SetDisplayMode(DisplayMode.Preview, DisplayModeDuration.Remember);

                    using (new ItemEditing(item, true))
                    {
                        field.Value = fieldValue;
                    }



                    //Act
                    var result = mapper.GetField(field, config, null) as string;

                    Sitecore.Context.Site = null;

                    //Assert
                    Assert.AreEqual(expected, result);
                }
            }
        }

        [Test]
        public void GetField_RichTextSettingsIsRaw_StringIsReturnedWithoutEscaping()
        {
            //Assign
            var fieldValue = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";

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

                var mapper = new SitecoreFieldStringMapper();
                var config = new SitecoreFieldConfiguration();
                config.Setting = SitecoreFieldSettings.RichTextRaw;

                Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }



                //Act
                var result = mapper.GetField(field, config, null) as string;

                Sitecore.Context.Site = null;

                //Assert
                Assert.AreEqual(fieldValue, result);
            }
        }

        #endregion

        #region Method - SetField

        [Test]
        public void SetField_RichText_ThrowsException()
        {
            //Assign
            var expected = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
                {
                    new DbField(FieldName)
                    {
                        Type = "Rich Text"
                    }
                }
            })
            {
                using (new FakeSite())
                {

                    var item =
                        database.GetItem("/sitecore/content/TestItem");
                    var field = item.Fields[FieldName];

                    var mapper = new SitecoreFieldStringMapper();
                    var config = new SitecoreFieldConfiguration();
                    config.PropertyInfo = typeof(StubClass).GetProperty("String");


                    Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");

                    using (new ItemEditing(item, true))
                    {
                        field.Value = string.Empty;
                    }

                    //Act
                    using (new ItemEditing(item, true))
                    {
                        //Rich text not raw throws exception
                        Assert.Throws<NotSupportedException>(() => mapper.SetField(field, expected, config, null));
                    }

                    Sitecore.Context.Site = null;

                    //Assert
                    Assert.AreEqual(string.Empty, field.Value);
                }
            }
        }

        [Test]
        public void SetField_FielNonRichText_ValueWrittenToField()
        {
            //Assign
            var expected = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
                {
                    new DbField(FieldName)
                    {
                    }
                }
            })
            {
                var item = database.GetItem("/sitecore/content/TestItem");
                var field = item.Fields[FieldName];

                var mapper = new SitecoreFieldStringMapper();
                var config = new SitecoreFieldConfiguration();
                config.Setting = SitecoreFieldSettings.RichTextRaw;

                Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");

                using (new ItemEditing(item, true))
                {
                    field.Value = string.Empty;
                }

                //Act
                using (new ItemEditing(item, true))
                {
                    mapper.SetField(field, expected, config, null);
                }

                Sitecore.Context.Site = null;

                //Assert
                Assert.AreEqual(expected, field.Value);
            }
        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_StreamType_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreFieldStringMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof (StubClass).GetProperty("String");

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Stub

        public class StubClass
        {
            public string String { get; set; }
        }
        #endregion
    }
}




