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
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Tests.DataMappers
{
    [TestFixture]
    public class SitecoreFieldNullableMapperFixture : AbstractMapperFixture
    {

        #region Method - GetField

        [Test]
        public void GetField_EmptyField_ReturnsNull()
        {
            //Assign
            var fieldValue = "";
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldNullableMapper<int, SitecoreFieldIntegerMapper>();


            //Act
            var result = mapper.GetField(field, null, null) as int?;

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetField_FieldContainsNumber_ReturnsNumber()
        {
            //Assign
            var fieldValue = "4";
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldNullableMapper<int, SitecoreFieldIntegerMapper>();


            //Act
            var result = mapper.GetField(field, null, null) as int?;

            //Assert
            Assert.AreEqual(4, result);
        }

        #endregion
        #region Method - SetField

        [Test]
        public void SetField_NullValue_FieldIsEmpty()
        {
            //Assign
            var expected = "";
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var value = (int?) null;
            var mapper = new SitecoreFieldNullableMapper<int, SitecoreFieldIntegerMapper>();

           item.Editing.BeginEdit();

            //Act
           
                mapper.SetField(field, value, null, null);
            
            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_ValueIsSet_FieldContainsValue()
        {
            //Assign
            var expected = "4";
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var value = (int?)4;
            var mapper = new SitecoreFieldNullableMapper<int, SitecoreFieldIntegerMapper>();
            
            item.Editing.BeginEdit();

            //Act
            
                mapper.SetField(field, value, null, null);
            
            //Assert
            Assert.AreEqual(expected, field.Value);
        }



        #endregion

    }
}




