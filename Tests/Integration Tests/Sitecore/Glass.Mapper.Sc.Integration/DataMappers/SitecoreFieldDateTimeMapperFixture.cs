using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreFieldDateTimeMapperFixture : AbstractMapperFixture
    {
        #region Method - GetFieldValue

        [Test]
        public void GetFieldValue_FieldContainsValidDate_ReturnsDateTime()
        {
            //Assign
            string fieldValue = "20120101T010101";
            DateTime expected = new DateTime(2012,01,01,01,01,01);
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDateTimeMapper/GetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDateTimeMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (DateTime) mapper.GetFieldValue(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region Method - SetFieldValue

        [Test]
        public void SetFieldValue_DateTimePassed_SetsFieldValue()
        {
            //Assign
            string expected = "20120101T010101";
            DateTime objectValue = new DateTime(2012, 01, 01, 01, 01, 01);
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDateTimeMapper/GetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDateTimeMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetFieldValue(field, objectValue, null, null);
            }
        

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetFieldValue_NonDateTimePassed_ExceptionThrown()
        {
            //Assign
            string expected = "20120101T010101";
            int objectValue = 4; 
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDateTimeMapper/GetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDateTimeMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetFieldValue(field, objectValue, null, null);
            }


            //Assert
        }

        #endregion
    }
}
