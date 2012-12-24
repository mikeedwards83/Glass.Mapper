using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreFieldGuidMapperFixture : AbstractMapperFixture 
    {
        #region Method - GetField

        [Test]
        public void GetField_ContainsGuid_GuidObjectReturned()
        {
            //Assign
            var fieldValue = "{FC1D0AFD-71CC-47e2-84B3-7F1A2973248B}";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldGuidMapper/GetField");
            var field = item.Fields[FieldName];
            var expected = new Guid(fieldValue);

            var mapper = new SitecoreFieldGuidMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (Guid)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldEmpty_EmptyGuidReturned()
        {
            //Assign
            var fieldValue = string.Empty;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldGuidMapper/GetField");
            var field = item.Fields[FieldName];
            var expected = Guid.Empty;

            var mapper = new SitecoreFieldGuidMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (Guid)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region Method - SetField

        [Test]
        public void SetField_Guidpassed_ValueSetOnField()
        {
            //Assign
            var expected = "{FC1D0AFD-71CC-47E2-84B3-7F1A2973248B}";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldGuidMapper/SetField");
            var field = item.Fields[FieldName];
            var value = new Guid(expected);

            var mapper = new SitecoreFieldGuidMapper();

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
        public void SetField_EmptyGuidPassed_ValueSetOnField()
        {
            //Assign
            var expected = "{00000000-0000-0000-0000-000000000000}";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldGuidMapper/SetField");
            var field = item.Fields[FieldName];
            var value = new Guid(expected);

            var mapper = new SitecoreFieldGuidMapper();

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
        [ExpectedException(typeof(MapperException))]
        public void SetField_IntegerPassed_ValueSetOnField()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldGuidMapper/SetField");
            var field = item.Fields[FieldName];
            var value = 1;

            var mapper = new SitecoreFieldGuidMapper();

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
        }

        #endregion
    }

   
}
