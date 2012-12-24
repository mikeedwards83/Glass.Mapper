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
    public class SitecoreFieldImageMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_ImageInField_ReturnsImageObject()
        {
            //Assign
            var fieldValue =
                "<image mediaid=\"{D897833C-1F53-4FAE-B54B-BB5B11B8F851}\" mediapath=\"/Files/20121222_001405\" src=\"~/media/D897833C1F534FAEB54BBB5B11B8F851.ashx\" hspace=\"15\" vspace=\"20\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldImageMapper/GetField");
            var field = item.Fields[FieldName];
            var mapper = new SitecoreFieldImageMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as Image;

            //Assert
            Assert.AreEqual("test alt", result.Alt);
            // Assert.Equals(null, result.Border);
            Assert.AreEqual(string.Empty, result.Class);
            Assert.AreEqual(15, result.HSpace);
            Assert.AreEqual(480, result.Height);
            Assert.AreEqual(new Guid("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}"), result.MediaId);
            Assert.AreEqual("/~/media/D897833C1F534FAEB54BBB5B11B8F851.ashx", result.Src);
            Assert.AreEqual(20, result.VSpace);
            Assert.AreEqual(640, result.Width);
        }


        #endregion

        #region Method - SetField

        [Test]
        public void SetField_ImagePassed_ReturnsPopulatedField()
        {
            //Assign
            var expected =
                "<image mediaid=\"{D897833C-1F53-4FAE-B54B-BB5B11B8F851}\" width=\"640\" vspace=\"50\" height=\"480\" hspace=\"30\" border=\"\" class=\"\" alt=\"test alt\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldImageMapper/GetField");
            var field = item.Fields[FieldName];
            var mapper = new SitecoreFieldImageMapper();
            var image = new Image()
                            {
                                Alt = "test alt",
                                HSpace = 30,
                                Height = 480,
                                MediaId =  new Guid("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}"),
                                VSpace = 50,
                                Width = 640,
                                Border = String.Empty,
                                Class =  String.Empty

                            };

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, image, null, null);
            }
            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        #endregion
    }
}
