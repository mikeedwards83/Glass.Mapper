using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Integration.Configuation.Fluent
{
    [TestFixture]
    public class FluentGeneralFixture
    {

        [Test]
        public void General_RetrieveItemAndFieldsFromSitecore_ReturnPopulatedClass()
        {
            //Assign
            string fieldValue = "test field value";
            Guid id = new Guid("{A544AE18-BC21-457D-8852-438F53AAE7E1}");
            string name = "Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(new GlassConfig());

            var loader = new SitecoreFluentConfigurationLoader();

            var stubConfig = loader.Add<Stub>();
            stubConfig.Configure(x =>
                                     {
                                         x.Id(y => y.Id);
                                         x.Field(y => y.Field);
                                         x.Info(y => y.Name).InfoType(SitecoreInfoType.Name);
                                     });

            context.Load(loader);

            var item = db.GetItem(new ID(id));

            using (new ItemEditing(item, true))
            {
                item["Field"] = fieldValue;
            }

            var service = new SitecoreService(db, context);

            //Act
            var result = service.GetItem<Stub>(id);

            //Assert
            Assert.AreEqual(fieldValue, result.Field);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(name, result.Name);

        }


        #region Stub

        public class Stub
        {
            public virtual Guid Id { get; set; }
            public virtual string Field { get; set; }
            public virtual string Name { get; set; }

        }

        #endregion

    }
}
