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


using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
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

                var config = new SitecoreChildrenConfiguration();
                config.InferType = false;
                config.IsLazy = false;
                config.PropertyInfo = typeof(Stub).GetProperty("Children");

                var service = Substitute.For<ISitecoreService>();
                var predicate = Arg.Is<Item>(x => item.Children.Any(y => x.ID == y.ID));

                //ME - Although this looks correct I am not sure it is
                service.CreateType(typeof(StubChild), predicate, false, false, null)
                    .ReturnsForAnyArgs(info => new StubChild()
                    {
                        Id = info.Arg<Item>().ID
                    });
                service.Config.Returns(new Config());

                var context = new SitecoreDataMappingContext(null, item, service);

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

                var config = new SitecoreChildrenConfiguration();
                config.InferType = false;
                config.IsLazy = false;
                config.PropertyInfo = typeof(Stub).GetProperty("Children");

                var service = Substitute.For<ISitecoreService>();
                var predicate = Arg.Is<Item>(x => item.Children.Any(y => x.ID == y.ID));

                //ME - Although this looks correct I am not sure it is
                service.CreateType(typeof(StubChild), predicate, false, false, null)
                    .ReturnsForAnyArgs(info => new StubChild()
                    {
                        Id = info.Arg<Item>().ID
                    });
                service.Config = new Config();

                var context = new SitecoreDataMappingContext(null, item, service);

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




