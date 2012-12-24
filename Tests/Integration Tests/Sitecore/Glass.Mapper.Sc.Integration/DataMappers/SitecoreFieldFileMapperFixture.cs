using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.Fields;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreFieldFileMapperFixture : AbstractMapperFixture
    {

        #region Method - GetField

        [Test]
        public void GetViewValue_FieldPointsAtFile_ReturnFileObject()
        {
            //Assign
            var fieldValue =
                "<file mediaid=\"{C10794CE-624F-4F72-A2B9-14336F3FB582}\" src=\"~/media/C10794CE624F4F72A2B914336F3FB582.ashx\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFileMapper/GetField");
            var field = item.Fields[FieldName];
            var mapper = new SitecoreFieldFileMapper();
            
            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as File;

            //Assert
            Assert.AreEqual(new Guid("{C10794CE-624F-4F72-A2B9-14336F3FB582}"), result.Id);
            Assert.AreEqual("/~/media/C10794CE624F4F72A2B914336F3FB582.ashx", result.Src);
        }
        [Test]
        public void GetViewValue_FieldEmpty_ReturnEmptyValues()
        {
            //Assign
            var fieldValue = string.Empty;

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFileMapper/GetField");
            var field = item.Fields[FieldName];
            var mapper = new SitecoreFieldFileMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as File;

            //Assert
            Assert.AreEqual(Guid.Empty, result.Id);
            Assert.AreEqual(null, result.Src);
        }

        #endregion

        #region Method - SetField

        [Test]
        public void SetField_FileObjectPass_FieldPopulated()
        {
            //Assign
              var expected =
                "<file mediaid=\"{C10794CE-624F-4F72-A2B9-14336F3FB582}\" src=\"~/media/C10794CE624F4F72A2B914336F3FB582.ashx\" />";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFileMapper/SetField");
            var field = item.Fields[FieldName];
            var mapper = new SitecoreFieldFileMapper();
            var file = new File()
                           {
                               Id = new Guid("{C10794CE-624F-4F72-A2B9-14336F3FB582}")
                           };


            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }


           //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, file, null, null);
            }
            //Assert

            Assert.AreEqual(expected, item[FieldName]);
        }

        [Test]
        public void SetField_FileNull_FileIsEmpty()
        {
            //Assign
            var expected = string.Empty;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFileMapper/SetField");
            var field = item.Fields[FieldName];
            var mapper = new SitecoreFieldFileMapper();
            var file = (File)null;


            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }


            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, file, null, null);
            }
            //Assert

            Assert.AreEqual(expected, item[FieldName]);
        }

        [Test]
        public void SetField_FileEmptyGuid_FieldLinkRemoved()
        {
            //Assign
            var fieldValue =
               "<file mediaid=\"{C10794CE-624F-4F72-A2B9-14336F3FB582}\" src=\"~/media/C10794CE624F4F72A2B914336F3FB582.ashx\" />";

            var expected = string.Empty;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFileMapper/SetField");
            var field = item.Fields[FieldName];
            var mapper = new SitecoreFieldFileMapper();
            var file = new File()
            {
                Id = Guid.Empty
            };


            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }


            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, file, null, null);
            }
            //Assert

            Assert.AreEqual(expected, item[FieldName]);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void SetField_FileContainsMissinfMedia_ExpectionThrown()
        {
            //Assign

            var expected = string.Empty;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFileMapper/SetField");
            var field = item.Fields[FieldName];
            var mapper = new SitecoreFieldFileMapper();
            var file = new File()
            {
                Id = Guid.NewGuid()
            };


            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }


            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, file, null, null);
            }
            //Assert

        }
        #endregion
    }
}
