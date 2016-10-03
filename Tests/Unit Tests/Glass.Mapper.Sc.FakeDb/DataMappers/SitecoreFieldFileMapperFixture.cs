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
using Glass.Mapper.Sc.Fields;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreFieldFileMapperFixture 
    {

        #region Method - GetField

        [Test]
        public void GetViewValue_FieldPointsAtFile_ReturnFileObject()
        {
            //Assign
            var templateId = ID.NewID;
            var fieldId = ID.NewID;
            var targetId = ID.NewID;
            var expected = "/~/media/C10794CE624F4F72A2B914336F3FB582.ashx";
            var mediaId = new ID("{C10794CE-624F-4F72-A2B9-14336F3FB582}");
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {"Field", ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),
                new DbItem("Media", mediaId)
            })
            {

                Sitecore.Resources.Media.MediaProvider mediaProvider =
                    Substitute.For<Sitecore.Resources.Media.MediaProvider>();
                mediaProvider
                    .GetMediaUrl(
                        Arg.Is<Sitecore.Data.Items.MediaItem>(
                            i => i.ID == mediaId)
                    )
                    .Returns(expected);

                using (new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider))
                {


                    var fieldValue =
                        "<file mediaid=\"{C10794CE-624F-4F72-A2B9-14336F3FB582}\" src=\"~/media/C10794CE624F4F72A2B914336F3FB582.ashx\" />";

                    var item = database.GetItem("/sitecore/content/Target");
                    var field = item.Fields["Field"];
                    var mapper = new SitecoreFieldFileMapper();

                    using (new ItemEditing(item, true))
                    {
                        field.Value = fieldValue;
                    }

                    //Act
                    var result = mapper.GetField(field, null, null) as File;

                    //Assert
                    Assert.AreEqual(mediaId.Guid, result.Id);
                    Assert.AreEqual(expected, result.Src);
                }
            }
        }

        [Test]
        public void GetViewValue_FieldEmpty_ReturnEmptyValues()
        {
            //Assign
            var templateId = ID.NewID;
            var fieldId = ID.NewID;
            var targetId = ID.NewID;
            var mediaId = new ID("{C10794CE-624F-4F72-A2B9-14336F3FB582}");
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {"Field", ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),
                new DbItem("Media", mediaId)

            })
            {
                var fieldValue = string.Empty;


                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields["Field"];
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
        }

        #endregion

        #region Method - SetField

        [Test]
        public void SetField_FileObjectPass_FieldPopulated()
        {
            //Assign
            var templateId = ID.NewID;
            var fieldId = ID.NewID;
            var targetId = ID.NewID;
            var mediaId = new ID("{C10794CE-624F-4F72-A2B9-14336F3FB582}");
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {"Field", ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),
                new DbItem("Media", mediaId)

            })
            {

              


                    var expected =
                        "<file mediaid=\"{C10794CE-624F-4F72-A2B9-14336F3FB582}\" src=\"~/media/C10794CE624F4F72A2B914336F3FB582.ashx\" />";

                    var item = database.GetItem("/sitecore/content/Target");
                    var field = item.Fields["Field"];
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

                    Assert.AreEqual(expected, item["Field"]);
            }
        }

        [Test]
        public void SetField_FileNull_FileIsEmpty()
        {
            //Assign
            var templateId = ID.NewID;
            var fieldId = ID.NewID;
            var targetId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {"Field", ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var expected = string.Empty;

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields["Field"];
                var mapper = new SitecoreFieldFileMapper();
                var file = (File) null;


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

                Assert.AreEqual(expected, item["Field"]);
            }
        }

        [Test]
        public void SetField_FileEmptyGuid_FieldLinkRemoved()
        {
            //Assign
            var templateId = ID.NewID;
            var fieldId = ID.NewID;
            var targetId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {"Field", ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),
                new Sitecore.FakeDb.DbItem("File", new ID("{C10794CE-624F-4F72-A2B9-14336F3FB582}"), templateId),

            })
            {
                var fieldValue =
                    "<file mediaid=\"{C10794CE-624F-4F72-A2B9-14336F3FB582}\" src=\"~/media/C10794CE624F4F72A2B914336F3FB582.ashx\" />";

                var expected = string.Empty;

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields["Field"];
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

                Assert.AreEqual(expected, item["Field"]);
            }
        }

        [Test]
        public void SetField_FileContainsMissinfMedia_ExpectionThrown()
        {
            //Assign
            var templateId = ID.NewID;
            var fieldId = ID.NewID;
            var targetId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {"Field", ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var expected = string.Empty;

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields["Field"];
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
                    Assert.Throws<MapperException>(() =>
                    {
                        mapper.SetField(field, file, null, null);

                    });
                }
                //Assert
            }
        }
        #endregion
    }
}




