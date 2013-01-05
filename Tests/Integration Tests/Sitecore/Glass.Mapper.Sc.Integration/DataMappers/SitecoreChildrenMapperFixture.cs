using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration.Attributes;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using Sitecore.Data;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreChildrenMapperFixture
    {
        #region Method - MapToProperty

        [Test]
        public void MapToProperty_ItemHasThreeChildren_ThreeObjectAreCreated()
        {
            //Assign
            var database = Sitecore.Configuration.Factory.GetDatabase("master");

            var item = database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreChildrenMapper/Parent");
            var mapper = new SitecoreChildrenMapper();
            
            var config = new SitecoreChildrenConfiguration();
            config.InferType = false;
            config.IsLazy = false;
            config.PropertyInfo = typeof (Stub).GetProperty("Children");

            var service = Substitute.For<ISitecoreService>();
            var predicate = Arg.Is<Item>(x => item.Children.Any(y => x.ID == y.ID));

            //ME - Although this looks correct I am not sure it is
            service.CreateClass(typeof(StubChild), predicate, false, false).ReturnsForAnyArgs(info => new StubChild()
                                                                                  {
                                                                                      Id =  info.Arg<Item>().ID
                                                                                  });

            var context = new SitecoreDataMappingContext(null, item, service);

            mapper.Setup(new DataMapperResolverArgs(null,config));
            
            //Act
            var result = mapper.MapToProperty(context) as IEnumerable<StubChild>;

            //Assert

            Assert.AreEqual(item.Children.Count, result.Count());

            foreach (Item child in item.Children)
            {
                Assert.IsTrue(result.Any(x=>x.Id == child.ID));
            }

        }

        [Test]
        public void MapToProperty_ItemHasNoChildren_NoObjectsCreated()
        {
            //Assign
            var database = Sitecore.Configuration.Factory.GetDatabase("master");

            var item = database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreChildrenMapper/Parent/Child1");
            var mapper = new SitecoreChildrenMapper();

            var config = new SitecoreChildrenConfiguration();
            config.InferType = false;
            config.IsLazy = false;
            config.PropertyInfo = typeof(Stub).GetProperty("Children");

            var service = Substitute.For<ISitecoreService>();
            var predicate = Arg.Is<Item>(x => item.Children.Any(y => x.ID == y.ID));

            //ME - Although this looks correct I am not sure it is
            service.CreateClass(typeof(StubChild), predicate, false, false).ReturnsForAnyArgs(info => new StubChild()
            {
                Id = info.Arg<Item>().ID
            });

            var context = new SitecoreDataMappingContext(null, item, service);

            mapper.Setup(new DataMapperResolverArgs(null,config));

            //Act
            var result = mapper.MapToProperty(context) as IEnumerable<StubChild>;

            //Assert

            Assert.AreEqual(0, result.Count());

          

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
