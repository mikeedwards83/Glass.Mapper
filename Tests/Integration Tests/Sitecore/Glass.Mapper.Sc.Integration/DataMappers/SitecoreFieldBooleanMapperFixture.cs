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

        #region Method - GetField

        [Test]
        public void GetField_FieldValueZero_ReturnsFalse()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetField");
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
            var result = mapper.GetField(field, null, null);


            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void GetField_FieldValueStringEmpty_ReturnsFalse()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetField");
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
            var result = mapper.GetField(field, null, null);


            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void GetField_FieldValueOne_ReturnsTrue()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetField");
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
            var result = mapper.GetField(field, null, null);


            //Assert
            Assert.AreEqual(true, result);

        }

        [Test]
        public void GetField_FieldValueRandom_ReturnsFalse()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetField");
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
            var result = mapper.GetField(field, null, null);


            //Assert
            Assert.AreEqual(false, result);

        }
        #endregion

        #region Method - SetField

        [Test]
        public void SetField_ValueFalse_FieldSetToZero()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetField");
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
                mapper.SetField(field, value, null, null);
                item.Editing.EndEdit();
            }

            //Assert
            Assert.AreEqual(expected, field.Value);

        }

        [Test]
        public void SetField_ValueTrue_FieldSetToOne()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldBooleanMapper/GetField");
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
                mapper.SetField(field, value, null, null);
                item.Editing.EndEdit();
            }

            //Assert
            Assert.AreEqual(expected, field.Value);

        }
        #endregion

    }
}
