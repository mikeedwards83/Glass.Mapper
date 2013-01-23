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
using Sitecore.Data;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreFieldNameValueCollectionMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsThreeItems_ReturnsNameValueCollection()
        {
            //Assign
            var fieldValue = "Name1=Value1&Name2=Value2";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldNameValueCollectionMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldNameValueCollectionMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as NameValueCollection;

            //Assert
            Assert.AreEqual("Value1", result["Name1"]);
            Assert.AreEqual("Value2", result["Name2"]);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void GetField_FieldIsEmpty_ReturnsEmptyNameValueCollection()
        {
            //Assign
            var fieldValue = "";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldNameValueCollectionMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldNameValueCollectionMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as NameValueCollection;

            //Assert
            Assert.AreEqual(0, result.Count);
        }


        #endregion
        #region Method - SetField

        [Test]
        public void SetField_CollectionHas2Values_FieldContainsQueryString()
        {
            //Assign
            var expected = "Name1=Value1&Name2=Value2";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldNameValueCollectionMapper/SetField");
            var field = item.Fields[FieldName];
            var value = new NameValueCollection();
            value.Add("Name1", "Value1");
            value.Add("Name2", "Value2");
            var mapper = new SitecoreFieldNameValueCollectionMapper();

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
        public void SetField_CollectionHas0Values_FieldIsEmpty()
        {
            //Assign
            var expected = "";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldNameValueCollectionMapper/SetField");
            var field = item.Fields[FieldName];
            var value = new NameValueCollection();
            var mapper = new SitecoreFieldNameValueCollectionMapper();

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



