using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Tests.DataMappers
{
    [TestFixture]
    public class SitecoreFieldLongMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsValidLong_ReturnsLong()
        {
            //Assign
            string fieldValue = "3";
            long expected = 3;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldLongMapper();
            
            //Act
            var result = (long)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldContainsEmptyString_ReturnsLongZero()
        {
            //Assign
            string fieldValue = string.Empty;
            long expected = 0;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldLongMapper();
            
            //Act
            var result = (long)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetField_FieldContainsInvalidValidLong_ReturnsLong()
        {
            //Assign
            string fieldValue = "hello world";
            long expected = 3;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldLongMapper();
            
            //Act
            var result = (long)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion


        #region Method - SetField

        [Test]
        public void SetField_ObjectisValidLong_SetsFieldValue()
        {
            //Assign
            string expected = "3";
            long objectValue = 3;

            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldLongMapper();


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

            var mapper = new SitecoreFieldLongMapper();

          
            item.Editing.BeginEdit();
            
            //Act
            mapper.SetField(field, objectValue, null, null);
           
            //Assert
        }

        #endregion
    }
}
