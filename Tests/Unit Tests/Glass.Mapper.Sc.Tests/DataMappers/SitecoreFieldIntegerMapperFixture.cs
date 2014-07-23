using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Tests.DataMappers
{
    [TestFixture]
    public class SitecoreFieldIntegerMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsValidInteger_ReturnsInteger()
        {
            //Assign
            string fieldValue = "3";
            int expected = 3;
            var mapper = new SitecoreFieldIntegerMapper();
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            //Act
            var result = (int)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldContainsEmptyString_ReturnsIntegerZero()
        {
            //Assign
            string fieldValue = string.Empty;
            int expected = 0;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldIntegerMapper();

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
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldIntegerMapper();

            //Act
            var result = (int)mapper.GetField(field, null, null);

            //Assert 
        }

        #endregion


        #region Method - SetField

        [Test]
        public void SetField_ObjectisValidInteger_SetsFieldValue()
        {
            //Assign
            string expected = "3";
            int objectValue = 3;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldIntegerMapper();

            item.Editing.BeginEdit();

            //Act
            mapper.SetField(field, objectValue, null, null);

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetField_ObjectIsDouble_ThrowsException()
        {
            //Assign
            double objectValue = 3;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldIntegerMapper();

            item.Editing.BeginEdit();
            //Act
            mapper.SetField(field, objectValue, null, null);


            //Assert
        }

        #endregion
    }
}
