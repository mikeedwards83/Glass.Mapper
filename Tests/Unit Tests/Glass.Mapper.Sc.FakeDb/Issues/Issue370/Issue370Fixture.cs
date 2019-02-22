using Glass.Mapper.Sc.Configuration.Fluent;
using NUnit.Framework;
using Sitecore.FakeDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data;

namespace Glass.Mapper.Sc.FakeDb.Issues.Issue370
{
    /// <summary>
    /// https://github.com/mikeedwards83/Glass.Mapper/issues/370
    /// </summary>
    [TestFixture]   
    public class Issue370Fixture
    {
        [Test]
        public void Protected_MapsDataToProtectedFieldsIgnoresGetOnlyFields()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string expected1 = "V1";
            string expected2 = "V2";
            string expected3 = "V1V2";
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    {"Field1",expected1 },
                    {"Field2",expected2 },
                   
                }
            })
            {

                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database, context);

                //Act
                var result = service.GetItem<ProtectedModel>(id, x => { });

                //Arrange
                Assert.AreEqual(expected2, result.Field2);

                Assert.AreEqual(expected1, result.Field1Public);

                Assert.AreEqual(expected3, result.Field3);


            }

        }

        [Test]
        public void Privated_MapsDataToProtectedFieldsIgnoresGetOnlyFields()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string expected1 = "V1";
            string expected2 = "V2";
            string expected3 = "V1V2";
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    {"Field1",expected1 },
                    {"Field2",expected2 },

                }
            })
            {

                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database, context);

                //Act
                var result = service.GetItem<PrivateModel>(id, x => { });

                //Arrange
                Assert.AreEqual(expected2, result.Field2);

                Assert.AreEqual(expected1, result.Field1Public);

                Assert.AreEqual(expected3, result.Field3);


            }

        }

        [Test]
        public void Protected_MapsDataToProtectedFieldsGetOnlyFields()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string expected1 = "V1";
            string expected2 = "V2";
            string expected3 = "V1V2";
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    {"Field1",expected1 },
                    {"Field2",expected2 },
                     {"Field3",expected3 }
                }
            })
            {

                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database, context);

                //Act
                var result = service.GetItem<ProtectedModel>(id, x => { });

                //Arrange
                Assert.AreEqual(expected2, result.Field2);

                Assert.AreEqual(expected1, result.Field1Public);

                Assert.AreEqual(expected3, result.Field3);


            }

        }

        [Test]
        public void Protected_MapsDataToProtectedFieldsIgnoresGetOnlyFieldsUsingIgnoreAttribute()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string expected1 = "V1";
            string expected2 = "V2";
            string expected3 = "V1V2";
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    {"Field1",expected1 },
                    {"Field2",expected2 },

                }
            })
            {

                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database, context);

                //Act
                var result = service.GetItem<ProtectedModelWithIgnore>(id, x => { });

                //Arrange
                Assert.AreEqual(expected2, result.Field2);

                Assert.AreEqual(expected1, result.Field1Public);

                Assert.AreEqual(expected3, result.Field3);


            }

        }

        [SitecoreType (AutoMap =true)]
        public class ProtectedModel
        {
            protected virtual string Field1 { get; set; }
            public virtual string Field2 { get; protected set; }

            public virtual string Field3 => Field1 + Field2;

            public static SitecoreType<ProtectedModel> Load()
            {
                var config = new SitecoreType<ProtectedModel>();
                config.Field(x => x.Field1);
                config.Field(x => x.Field2);
                return config;
            }

            public string Field1Public => Field1;
        }

        [SitecoreType(AutoMap = true)]
        public class ProtectedModelWithIgnore
        {
            protected virtual string Field1 { get; set; }
            public virtual string Field2 { get; protected set; }

            [SitecoreIgnore]
            public virtual string Field3 => Field1 + Field2;

            public static SitecoreType<ProtectedModelWithIgnore> Load()
            {
                var config = new SitecoreType<ProtectedModelWithIgnore>();
                config.Field(x => x.Field1);
                config.Field(x => x.Field2);
                return config;
            }

            public string Field1Public => Field1;
        }


        [SitecoreType(AutoMap = true)]
        public class PrivateModel
        {
            private string Field1 { get; set; }
            public virtual string Field2 { get; private set; }

            public virtual string Field3 => Field1 + Field2;

            public static SitecoreType<PrivateModel> Load()
            {
                var config = new SitecoreType<PrivateModel>();
                config.Field(x => x.Field1);
                config.Field(x => x.Field2);
                return config;
            }

            public string Field1Public => Field1;
        }

    }

}
