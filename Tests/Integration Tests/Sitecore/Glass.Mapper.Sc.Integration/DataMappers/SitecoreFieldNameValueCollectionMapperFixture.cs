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
