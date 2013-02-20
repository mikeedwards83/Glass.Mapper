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
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
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
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFloatMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldFloatMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

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
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFloatMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldFloatMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (float)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetField_FieldContainsInvalidValidFloat_ReturnsFloat()
        {
            //Assign
            string fieldValue = "hello world";
            float expected = 3.141592f;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFloatMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldFloatMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (float)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion


        #region Method - GetField

        [Test]
        public void SetField_ObjectisValidFloat_SetsFieldValue()
        {
            //Assign
            string expected = "3.141592";
            float objectValue = 3.141592f;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFloatMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldFloatMapper();

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
        public void SetField_ObjectIsInt_ThrowsException()
        {
            //Assign
            int objectValue = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFloatMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldFloatMapper();

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



