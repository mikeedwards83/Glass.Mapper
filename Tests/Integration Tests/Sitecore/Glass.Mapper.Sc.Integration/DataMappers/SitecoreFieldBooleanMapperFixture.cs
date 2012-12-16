using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreFieldBooleanMapperFixture : AbstractMapperFixture
    {

        #region Method - GetFieldValue

        [Test]
        public void GetFieldValue_FieldValueZero_ReturnsFalse()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetFieldValue");
            var fieldName = "Field";
            var value = "0";
            var mapper = new SitecoreFieldBooleanMapper();


            var field = item.Fields[fieldName];
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                field.Value = value;
                item.Editing.EndEdit();
            }

            //Act
            var result = mapper.GetFieldValue(field, null, null);


            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void GetFieldValue_FieldValueStringEmpty_ReturnsFalse()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetFieldValue");
            var fieldName = "Field";
            var value = string.Empty;
            var mapper = new SitecoreFieldBooleanMapper();


            var field = item.Fields[fieldName];
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                field.Value = value;
                item.Editing.EndEdit();
            }

            //Act
            var result = mapper.GetFieldValue(field, null, null);


            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void GetFieldValue_FieldValueOne_ReturnsTrue()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetFieldValue");
            var fieldName = "Field";
            var value = "1";
            var mapper = new SitecoreFieldBooleanMapper();


            var field = item.Fields[fieldName];
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                field.Value = value;
                item.Editing.EndEdit();
            }

            //Act
            var result = mapper.GetFieldValue(field, null, null);


            //Assert
            Assert.AreEqual(true, result);

        }

        [Test]
        public void GetFieldValue_FieldValueRandom_ReturnsFalse()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetFieldValue");
            var fieldName = "Field";
            var value = "afaegaeg";
            var mapper = new SitecoreFieldBooleanMapper();


            var field = item.Fields[fieldName];
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                field.Value = value;
                item.Editing.EndEdit();
            }

            //Act
            var result = mapper.GetFieldValue(field, null, null);


            //Assert
            Assert.AreEqual(false, result);

        }
        #endregion

        #region Method - SetFieldValue

        [Test]
        public void SetFieldValue_ValueFalse_FieldSetToZero()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetFieldValue");
            var fieldName = "Field";
            var expected = "0";
            var mapper = new SitecoreFieldBooleanMapper();
            var value = false;

            var field = item.Fields[fieldName];
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                field.Value = string.Empty;
                item.Editing.EndEdit();
            }

            //Act
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                mapper.SetFieldValue(field, value, null, null);
                item.Editing.EndEdit();
            }

            //Assert
            Assert.AreEqual(expected, field.Value);

        }

        [Test]
        public void SetFieldValue_ValueTrue_FieldSetToOne()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetFieldValue");
            var fieldName = "Field";
            var expected = "1";
            var mapper = new SitecoreFieldBooleanMapper();
            var value = true;

            var field = item.Fields[fieldName];
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                field.Value = string.Empty;
                item.Editing.EndEdit();
            }

            //Act
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                mapper.SetFieldValue(field, value, null, null);
                item.Editing.EndEdit();
            }

            //Assert
            Assert.AreEqual(expected, field.Value);

        }
        #endregion

    }
}
