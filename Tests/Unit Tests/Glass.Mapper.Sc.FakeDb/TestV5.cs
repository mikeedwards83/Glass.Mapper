using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc.FakeDb
{
    public class TestV5
    {


        [Test]
        public void GetItem_UsingItemId_ReturnsItem()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));


                var service = new SitecoreService(database.Database);

                //Act
                var result = (StubClass)service.GetItem<StubClass>(id);

                //Assert
                Assert.IsNotNull(result);
            }
        }


        [Test]
        public void GetItem_UsingItemId_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));


                var service = new SitecoreService(database.Database);

                //Act
                var result = service.GetItem<StubClass>(id);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Target", result.Name);
            }
        }


        [SitecoreType(TemplateId = TemplateId)]
        public class StubClass
        {

            public const string TemplateId = "{ABE81623-6250-46F3-914C-6926697B9A86}";

            public TimeSpan Param5 { get; set; }

            public DateTime Param3 { get; set; }
            public bool Param4 { get; set; }
            public string Param2 { get; set; }
            public int Param1 { get; set; }

            public StubClass()
            {
            }
            public StubClass(int param1)
            {
                Param1 = param1;
            }

            public StubClass(int param1, string param2)
                : this(param1)
            {
                Param2 = param2;
            }

            public StubClass(int param1, string param2, DateTime param3)
                : this(param1, param2)
            {
                Param3 = param3;
            }
            public StubClass(int param1, string param2, DateTime param3, bool param4)
                : this(param1, param2, param3)
            {
                Param4 = param4;
            }

            public StubClass(int param1, string param2, DateTime param3, bool param4, TimeSpan param5)
                : this(param1, param2, param3, param4)
            {
                Param5 = param5;
            }

            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreInfo(SitecoreInfoType.Language)]
            public virtual Language Language { get; set; }

            [SitecoreInfo(SitecoreInfoType.FullPath)]
            public virtual string Path { get; set; }

            [SitecoreInfo(SitecoreInfoType.Version)]
            public virtual int Version { get; set; }

            [SitecoreInfo(SitecoreInfoType.Name)]
            public virtual string Name { get; set; }

            [SitecoreField]
            public virtual string Field { get; set; }
        }
    }


}
