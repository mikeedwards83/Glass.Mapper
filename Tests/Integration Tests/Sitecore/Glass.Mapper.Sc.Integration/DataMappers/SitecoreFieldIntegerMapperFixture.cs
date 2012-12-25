using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public  class SitecoreFieldIntegerMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsValidInteger_ReturnsInteger()
        {
            //Assign
            string fieldValue = "3";
            int expected = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldIntegerMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldIntegerMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (int)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        public void GetField_FieldContainsEmptyString_ReturnsIntegerZero()
        {
            //Assign
            string fieldValue = string.Empty;
            int expected = 0;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldIntegerMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldIntegerMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (int)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetField_FieldContainsInvalidValidInteger_ReturnsInteger()
        {
            //Assign
            string fieldValue = "hello world";
            int expected = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldIntegerMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldIntegerMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (int)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion


        #region Method - SetField

        [Test]
        public void SetField_ObjectisValidInteger_SetsFieldValue()
        {
            //Assign
            string expected = "3";
            int objectValue = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldIntegerMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldIntegerMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, objectValue, null, null);
            }


            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetField_ObjectIsDouble_ThrowsException()
        {
            //Assign
            double objectValue = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldIntegerMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldIntegerMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, objectValue, null, null);
            }


            //Assert
        }

        #endregion
    }
}
