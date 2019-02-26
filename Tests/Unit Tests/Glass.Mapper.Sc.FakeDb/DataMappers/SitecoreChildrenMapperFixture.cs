using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreChildrenMapperFixture
    {
        #region Method - MapToProperty

        [Test]
        public void MapToProperty_ItemHasThreeChildren_ThreeObjectAreCreated()
        {
            //Assign
            using(new EventDisabler())
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
                {
                    new Sitecore.FakeDb.DbItem("Child1"),
                    new Sitecore.FakeDb.DbItem("Child2"),
                    new Sitecore.FakeDb.DbItem("Child3")

                }
            })
            {

                var item = database.GetItem("/sitecore/content/TestItem");
                var mapper = new SitecoreChildrenMapper();
                var options = new GetItemOptionsParams();

                var config = new SitecoreChildrenConfiguration();
                config.InferType = false;
                config.PropertyInfo = typeof(Stub).GetProperty("Children");

                var service = Substitute.For<ISitecoreService>();

                service.GetItems(Arg.Any<GetItemsByFuncOptions>()).Returns(info => ((GetItemsByFuncOptions)info.Args()[0]).ItemsFunc(item.Database).Select(child => new StubChild { Id = child.ID }));

                service.Config.Returns(new Config());

                var context = new SitecoreDataMappingContext(null, item, service, options);

                mapper.Setup(new DataMapperResolverArgs(null, config));

                //Act
                var result = mapper.MapToProperty(context) as IEnumerable<StubChild>;

                //Assert

                Assert.AreEqual(item.Children.Count, result.Count());

                foreach (Item child in item.Children)
                {
                    Assert.IsTrue(result.Any(x => x.Id == child.ID));
                }
            }

        }


        [Test]
        public void MapToProperty_EnforceTemplate_OneObjectAreCreated()
        {
            //Assign


            ID templateId = ID.NewID;
            ID childId = ID.NewID;

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    
                },
                new Sitecore.FakeDb.DbItem("TestItem")
                {
                    new Sitecore.FakeDb.DbItem("Child1", childId, templateId),
                    new Sitecore.FakeDb.DbItem("Child2"),
                    new Sitecore.FakeDb.DbItem("Child3")

                }
            })
            {

                var item = database.GetItem("/sitecore/content/TestItem");
                var mapper = new SitecoreChildrenMapper();
                var options = new GetItemOptionsParams();

                var config = new SitecoreChildrenConfiguration();
                config.InferType = false;
                config.PropertyInfo = typeof(Stub).GetProperty("Children");
                config.TemplateId = templateId;
                config.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;

                var scContext = Context.Create(Utilities.CreateStandardResolver());

                var service = new SitecoreService(database.Database, scContext);


                var context = new SitecoreDataMappingContext(null, item, service, options);

                mapper.Setup(new DataMapperResolverArgs(null, config));

                //Act
                var result = mapper.MapToProperty(context) as IEnumerable<StubChild>;

                //Assert

                Assert.AreEqual(1, result.Count());

                Assert.AreEqual(childId, result.First().Id);

                
            }

        }



        [Test]
        public void MapToProperty_ItemHasNoChildren_NoObjectsCreated()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
                {

                }
            })
            {

                var item = database.GetItem("/sitecore/content/TestItem");
                var mapper = new SitecoreChildrenMapper();
                var options = new GetItemOptionsParams();

                var config = new SitecoreChildrenConfiguration();
                config.InferType = false;
                config.PropertyInfo = typeof(Stub).GetProperty("Children");

                var scContext = Context.Create(Utilities.CreateStandardResolver());

                var service = new SitecoreService(database.Database, scContext);

                service.Config = new Config();

                var context = new SitecoreDataMappingContext(null, item, service, options);

                mapper.Setup(new DataMapperResolverArgs(null, config));

                //Act
                var result = mapper.MapToProperty(context) as IEnumerable<StubChild>;

                //Assert

                Assert.AreEqual(0, result.Count());


            }
        }

        #endregion

        #region Property - ReadOnly

        [Test]
        public void ReadOnly_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreChildrenMapper();

            //Act
            var result = mapper.ReadOnly;

            //Assert
            Assert.IsTrue(result);
        }

        #endregion
  
        #region CanHandle

        [Test]
        public void CanHandle_ConfigIsChildren_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreChildrenMapper();
            var config = new SitecoreChildrenConfiguration();

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }


        #endregion
        #region Stubs

        [SitecoreType]
        public class StubChild
        {
            [SitecoreId]
            public virtual ID Id { get; set; }
        }

        public class Stub
        {
            public IEnumerable<StubChild> Children { get; set; } 
        }


        #endregion

     
    }
}




