
using System;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public  class SitecoreFieldFloatMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsValidFloat_ReturnsFloat()
        {
            //Assign
            string fieldValue = "3.141592";
            float expected = 3.141592F;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldFloatMapper();

            //Act
            var result = (float)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldContainsEmptyString_ReturnsFloatZero()
        {
            //Assign
            string fieldValue = string.Empty;
            float expected = 0f;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldFloatMapper();

            //Act
            var result = (float)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
   
        public void GetField_FieldContainsInvalidValidFloat_ReturnsFloat()
        {
            //Assign
            string fieldValue = "hello world";
            float expected = 3.141592f;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];
            var mapper = new SitecoreFieldFloatMapper();


            //Act

            Assert.Throws<MapperException>(() =>
            {
                var result = (float) mapper.GetField(field, null, null);
            });
            //Assert
           
        }

        #endregion


        #region Method - GetField

        [Test]
        public void SetField_ObjectisValidFloat_SetsFieldValue()
        {
            //Assign
            string expected = "3.141592";
            float objectValue = 3.141592f;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldFloatMapper();

            item.Editing.BeginEdit();

            //Act
            
            mapper.SetField(field, objectValue, null, null);
            
            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_ObjectIsInt_ThrowsException()
        {
            //Assign
            int objectValue = 3;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldFloatMapper();

            item.Editing.BeginEdit();

            //Act
            Assert.Throws<NotSupportedException>(() =>
            {
                mapper.SetField(field, objectValue, null, null);
            });
            //Assert
        }

        #endregion
    }
}




