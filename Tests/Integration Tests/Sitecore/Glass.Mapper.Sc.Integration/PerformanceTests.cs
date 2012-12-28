using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Integration
{
    [TestFixture]
    public class PerformanceTests 
    {


        [Test]
        public void Get1000Items()
        {

            //Assign
            var expected = "hello world";
            var id = new Guid("{59784F74-F830-4BCD-B1F0-1A08616EF726}");

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);

            var item = db.GetItem(new ID(id));
            using (new ItemEditing(item, true))
            {
                item["Field"] = expected;
            }

            //Act

            //get Sitecore raw
            var rawTotal = (long)0;
            for (int i = 0; i < 1000; i++)
            {
                var watch = new Stopwatch();

                watch.Start();
                var rawItem = db.GetItem(new ID(id));
                var value = item["Field"];
                watch.Stop();
                Assert.AreEqual(expected, value);
                rawTotal += watch.ElapsedTicks;
            }

            var rawAverage = rawTotal/1000;

            Console.WriteLine("Performance Test - 1000 - Raw - {0}", rawAverage);

            var glassTotal = (long)0;
            for (int i = 0; i < 1000; i++)
            {
                var watch = new Stopwatch();

                watch.Start();
                var glassItem = service.GetItem<StubClass>(id);
                var value = glassItem.Field;
                watch.Stop();
                Assert.AreEqual(expected, value);
                glassTotal += watch.ElapsedTicks;
            }

            var glassAverage = glassTotal / 1000;

            Console.WriteLine("Performance Test - 1000 - Glass - {0}", glassAverage);



            //Assert
            //ME - at the moment I am allowing glass to take twice the time. I would hope to reduce this
            Assert.LessOrEqual(glassAverage, rawAverage*2);


        }


        #region Stubs

        [SitecoreType]
        public class StubClass
        {
            [SitecoreField]
            public virtual string Field { get; set; }

            [SitecoreId]
            public virtual Guid Id { get; set; }
        }

        #endregion
    }
}
