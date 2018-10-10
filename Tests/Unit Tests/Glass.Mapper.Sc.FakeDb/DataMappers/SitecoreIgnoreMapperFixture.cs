using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreIgnoreMapperFixture 
    {

        #region Method - CanHandle

        [Test]
        public void CanHandle_IgnoreCOnfigurtion_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreIgnoreMapper();
            var config = new SitecoreIgnoreConfiguration();

            //ACt
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Method - MapCmsToProperty

        [Test]
        public void MapCmsToProperty_DoesNotAlterObject()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"Field", ""}
                }
            })
            {
                var fieldValue = "hello world";
                var propertyValue = "goodbye world";

                var item = database.GetItem(targetPath);
                var field = item.Fields["Field"];

                var mapper = new SitecoreIgnoreMapper();
                var config = new SitecoreIgnoreMapper();
                var options= new GetItemOptionsParams();


                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                var stub = new StubClass();
                stub.Field = propertyValue;
                var context = new SitecoreDataMappingContext(stub, item, null, options);

                //Act
                mapper.MapCmsToProperty(context);


                //Assert
                Assert.AreEqual(stub.Field, propertyValue);
            }
        }

        [Test]
        public void MapCmsToProperty_AutoMap()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"Field", ""}
                }
            })
            {

                var fieldValue = "hello world";
                var propertyValue = "goodbye world";


                var item = database.GetItem(targetPath);
                var field = item.Fields["Field"];

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClassNotIgnored)));
                var service = new SitecoreService(item.Database, context);


                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }
                //Act
                var notIgnored = service.GetItem<StubClassNotIgnored>(targetPath);
                var ignored = service.GetItem<StubClassIgnored>(targetPath);

                //Assert
                Assert.AreEqual(fieldValue, notIgnored.Field);
                Assert.AreEqual(null, ignored.Field);
            }
        }

        #endregion

        #region Method - MapPropertyToCms

        [Test]
        public void MapPropertyToCms_DoesNotAlterObject()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"Field", ""}
                }
            }) { 
                var fieldValue = "hello world";
                var propertyValue = "goodbye world";

                var item = database.GetItem(targetPath);
                var field = item.Fields["Field"];

                var mapper = new SitecoreIgnoreMapper();
                var config = new SitecoreIgnoreMapper();
                var options = new GetItemOptionsParams();

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                var stub = new StubClass();
                stub.Field = propertyValue;
                var context = new SitecoreDataMappingContext(stub, item, null, options);

                //Act
                mapper.MapPropertyToCms(context);


                //Assert
                Assert.AreEqual(fieldValue, item.Fields["Field"].Value);
            }
        }

        #endregion


        #region Stub

        public class StubClass
        {
            public string Field { get; set; }
        }

        [SitecoreType(AutoMap = true)]
        public class StubClassNotIgnored
        {
            public virtual string Field { get; set; }
        }

        [SitecoreType(AutoMap = true)]
        public class StubClassIgnored
        {
            [SitecoreIgnore]
            public string Field { get; set; }
        }


        #endregion
    }
}

