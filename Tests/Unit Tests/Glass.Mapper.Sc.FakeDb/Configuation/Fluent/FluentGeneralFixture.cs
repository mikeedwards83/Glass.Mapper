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
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.IoC;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.Configuation.Fluent
{
    [TestFixture]
    public class FluentGeneralFixture
    {

        [Test]
        public void Issue254_LoadingChildrenWithFluent()
        {
            
            //Arrange
            var loader = new SitecoreFluentConfigurationLoader();


            //Act
            var stubChildren = loader.Add<StubWithChildren>();
            stubChildren.Children(x => x.BaseChildren);

            var stubInherit1 = loader.Add<StubInherit1>();
            stubInherit1.Import(stubChildren);
            stubInherit1.AutoMap();

            var stubInherit2 = loader.Add<StubInherit1>();
            stubInherit2.Import(stubInherit1);
            stubInherit2.AutoMap();

            //Assert
            Assert.IsTrue(stubChildren.Config.Properties.Any(x => x is SitecoreChildrenConfiguration));
            Assert.IsTrue(stubInherit1.Config.Properties.Any(x => x is SitecoreChildrenConfiguration));
            Assert.IsTrue(stubInherit2.Config.Properties.Any(x => x is SitecoreChildrenConfiguration));


        }


        [Test]
        public void General_RetrieveItemAndFieldsFromSitecore_ReturnPopulatedClass()
        {
            //Assign


            string fieldValue = "test field value";
            Guid id = new Guid("{A544AE18-BC21-457D-8852-438F53AAE7E1}");
            string name = "Target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Field")
                    {
                        Value =  fieldValue
                    }
                }
            })
            {
                var resolver = new DependencyResolver(new Config());
                resolver.Finalise();
                var context = Context.Create(resolver);

                var loader = new SitecoreFluentConfigurationLoader();

                var stubConfig = loader.Add<Stub>();
                stubConfig.Configure(x =>
                {
                    x.Id(y => y.Id);
                    x.Field(y => y.Field);
                    x.Info(y => y.Name).InfoType(SitecoreInfoType.Name);
                    x.Delegate(y => y.Delegated).GetValue(GetDelegatedValue);
                });

                context.Load(loader);

                var item = database.GetItem(new ID(id));

                using (new ItemEditing(item, true))
                {
                    item["Field"] = fieldValue;
                }

                var service = new SitecoreService(database.Database, context);

                //Act
                var result = service.GetItem<Stub>(id);

                //Assert
                Assert.AreEqual(fieldValue, result.Field);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(name, result.Name);
                Assert.AreEqual("happy", result.Delegated);
            }
        }

        private string GetDelegatedValue(SitecoreDataMappingContext arg)
        {
            return "happy";
        }

        #region Stub

        public class Stub
        {
            public virtual Guid Id { get; set; }
            public virtual string Field { get; set; }
            public virtual string Name { get; set; }
            public virtual string Delegated { get; set; }

        }


        public class StubWithChildren
        {
            public virtual IEnumerable<Stub> BaseChildren { get; set; }
        }

        public class StubInherit1 : StubWithChildren
        {
            
        }
        public class StubInherit2 : StubInherit1
        {

        }

        #endregion

    }
}




