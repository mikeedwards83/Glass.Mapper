using System.Collections.Generic;
using System.IO;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    public class SitecoreFieldStreamMapperFixture 
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsData_StreamIsReturned()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var fieldValue = "";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];
                string expected = "hello world";

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(expected));
                var mapper = new SitecoreFieldStreamMapper();

                using (new ItemEditing(item, true))
                {
                    field.SetBlobStream(stream);
                }

                //Act
                var result = mapper.GetField(field, null, null) as Stream;

                //Assert
                var reader = new StreamReader(result);

                var resultStr = reader.ReadToEnd();
                Assert.AreEqual(expected, resultStr);
            }
        }


        //TODO: This requires a FakeDb fix
        [Test]
        public void GetField_FieldContainsDataTestConnectionLimit_StreamIsReturned()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var fieldValue = "";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];
                string expected = "hello world";

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(expected));
                var mapper = new SitecoreFieldStreamMapper();

                using (new ItemEditing(item, true))
                {
                    field.SetBlobStream(stream);
                }

                //Act
                var results = new List<Stream>();

                for (int i = 0; i < 1000; i++)
                {
                    var result = mapper.GetField(field, null, null) as Stream;
                    if(result == null)
                        continue;

                    results.Add(result);
                }

                //Assert
                Assert.AreEqual(1000, results.Count);
            }
        }


        #endregion


        #region Method - SetField

        [Test]
        public void SetField_StreamPassed_FieldContainsStream()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var fieldValue = "";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];
                string expected = "hello world";

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(expected));
                var mapper = new SitecoreFieldStreamMapper();

                using (new ItemEditing(item, true))
                {
                    field.SetBlobStream(new MemoryStream());
                }

                //Act
                using (new ItemEditing(item, true))
                {
                    mapper.SetField(field, stream, null, null);
                }

                //Assert
                var stream1 = field.GetBlobStream();
                stream1.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(stream1);
               
                var resultStr = reader.ReadToEnd();
                Assert.AreEqual(expected, resultStr);
            }
        }

        [Test]
        public void SetField_NullPassed_NoExceptionThrown()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var fieldValue = "";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];
                string expected = "hello world";

                Stream stream = null;
                var mapper = new SitecoreFieldStreamMapper();

                using (new ItemEditing(item, true))
                {
                    field.SetBlobStream(new MemoryStream());
                }

                //Act
                using (new ItemEditing(item, true))
                {
                    mapper.SetField(field, stream, null, null);
                }

                //Assert
                var outStream = field.GetBlobStream();
                Assert.AreEqual(0,outStream.Length);
            }
        }
        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_StreamType_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreFieldStreamMapper();
            var config = new SitecoreFieldConfiguration();

            config.PropertyInfo = typeof (StubClass).GetProperty("Stream");
            
            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public Stream Stream { get; set; }
        }
        #endregion
    }
}




