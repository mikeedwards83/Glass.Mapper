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
using System.IO;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NSubstitute;
using NUnit.Framework;
using Sitecore.FakeDb;
using Sitecore.Pipelines.RenderField;
using Sitecore.Sites;
using Sitecore.Xml.Xsl;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreFieldStringMapperFixture : AbstractMapperFixture
    {

        #region Method - GetField

        [Test]
        public void GetField_FieldContainsData_StringIsReturned()
        {
            //Assign
            var fieldValue = "hello world";

            string itemName = "SitecoreFieldStringMapperFixture";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(FieldName)
                    {
                        Value = fieldValue
                    }
                }
            })
            {
                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));
                var field = item.Fields[FieldName];

                var mapper = new SitecoreFieldStringMapper();
                var config = new SitecoreFieldConfiguration();


                //Act
                var result = mapper.GetField(field, config, null) as string;

                //Assert
                Assert.AreEqual(fieldValue, result);
            }
        }


        [Test]
        public void GetField_RichText_StringIsReturnedWithScapedUrl()
        {
            //Assign

            var fieldValue =
                  "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";

            string itemName = "SitecoreFieldStringMapperFixture";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(FieldName)
                    {
                        Type = "Rich Text",
                        Value = fieldValue
                    }
                }
            })
            {
                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));
              
                var expected =
                    "<p>Test with <a href=\"/en/Tests/DataMappers/SitecoreFieldStringMapper/GetField.aspx\">link</a></p>";

                db.PipelineWatcher.WhenCall("renderField").Then(x => (x as RenderFieldArgs).Result = new RenderFieldResult( expected));
             
                var field = item.Fields[FieldName];

                var mapper = new SitecoreFieldStringMapper();
                var config = new SitecoreFieldConfiguration();

                Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");
                Sitecore.Context.Site.SetDisplayMode(DisplayMode.Preview, DisplayModeDuration.Remember);
                
                //Act
                var result = mapper.GetField(field, config, null) as string;

                Sitecore.Context.Site = null;

                //Assert
                Assert.AreEqual(expected, result);
            }

        }

        [Test]
        public void GetField_RichTextSettingsIsRaw_StringIsReturnedWithoutEscaping()
        {
            //Assign

            string itemName = "SitecoreFieldStringMapperFixture";
            var fieldValue =
                   "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(FieldName)
                    {
                        Type = "Rich Text",
                        Value = fieldValue
                    }
                }
            })
            {

                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));
                
                var field = item.Fields[FieldName];

                var mapper = new SitecoreFieldStringMapper();
                var config = new SitecoreFieldConfiguration();
                config.Setting = SitecoreFieldSettings.RichTextRaw;

                Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");

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
        [ExpectedException(typeof(NotSupportedException))]
        public void SetField_RichText_ThrowsException()
        {
            //Assign

            string itemName = "SitecoreFieldStringMapperFixture";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(FieldName)
                    {
                        Type = "Rich Text"
                    }
                }
            })
            {
                var expected =
                    "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";
               
                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));
                
                var field = item.Fields[FieldName];

                var mapper = new SitecoreFieldStringMapper();
                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof (StubClass).GetProperty("String");


                Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");
                
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

        [Test]
        public void SetField_FieldRichTextRaw_ValueWrittenToField()
        {
            //Assign

            //Assign

            string itemName = "SitecoreFieldStringMapperFixture";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(FieldName)
                    {
                        Type = "Rich Text"
                    }
                }
            })
            {

                var expected =
                    "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";

                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                var field = item.Fields[FieldName];

                var mapper = new SitecoreFieldStringMapper();
                var config = new SitecoreFieldConfiguration();
                config.Setting = SitecoreFieldSettings.RichTextRaw;

                //Act
                using (new ItemEditing(item, true))
                {
                    mapper.SetField(field, expected, config, null);
                }

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




