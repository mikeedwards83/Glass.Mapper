using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    public class SitecoreFieldStreamMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField
        
        [Test]
        public void GetField_FieldContainsData_StreamIsReturned()
        {
            //Assign
            var fieldValue = "";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStreamMapper/GetField");
            var field = item.Fields[FieldName];
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
        
        #endregion


        #region Method - SetField

        [Test]
        public void SetField_StreamPassed_FieldContainsStream()
        {
            //Assign
            var fieldValue = "";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStreamMapper/SetField");
            var field = item.Fields[FieldName];
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
            var reader = new StreamReader(field.GetBlobStream());

            var resultStr = reader.ReadToEnd();
            Assert.AreEqual(expected, resultStr);
        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_StreamType_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreFieldStreamMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(Stream));
            
            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion
    }
}
