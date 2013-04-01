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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
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
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldNullableMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldNullableMapper<int, SitecoreFieldIntegerMapper>();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

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
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldNullableMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldNullableMapper<int, SitecoreFieldIntegerMapper>();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

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
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldNullableMapper/SetField");
            var field = item.Fields[FieldName];
            var value = (int?) null;
            var mapper = new SitecoreFieldNullableMapper<int, SitecoreFieldIntegerMapper>();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, value, null, null);
            }
            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_ValueIsSet_FieldContainsValue()
        {
            //Assign
            var expected = "4";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldNullableMapper/SetField");
            var field = item.Fields[FieldName];
            var value = (int?)4;
            var mapper = new SitecoreFieldNullableMapper<int, SitecoreFieldIntegerMapper>();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, value, null, null);
            }
            //Assert
            Assert.AreEqual(expected, field.Value);
        }



        #endregion

    }
}



