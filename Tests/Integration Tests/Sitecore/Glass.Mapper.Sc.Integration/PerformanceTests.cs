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
            long count = 1000;

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
                var watch1 = new Stopwatch();

            for (int i = 0; i < count; i++)
            {

                watch1.Start();
                var rawItem = db.GetItem(new ID(id));
                var value = item["Field"];
                watch1.Stop();
                Assert.AreEqual(expected, value);
                rawTotal += watch1.ElapsedTicks;
            }

            long rawAverage = rawTotal / count;

            Console.WriteLine("Performance Test - 1000 - Raw - {0}", rawAverage);
            Console.WriteLine("Raw ElapsedTicks to sec:  {0}", rawAverage / (double)Stopwatch.Frequency);

            var glassTotal = (long)0;
                var watch2 = new Stopwatch();
                for (int i = 0; i < count; i++)
            {

                watch2.Start();
                var glassItem = service.GetItem<StubClass>(id);
                var value = glassItem.Field;
                watch2.Stop();
                Assert.AreEqual(expected, value);
                glassTotal += watch2.ElapsedTicks;
            }


                long glassAverage = glassTotal / count;

            Console.WriteLine("Performance Test - 1000 - Glass - {0}", glassAverage);
            Console.WriteLine("Glass ElapsedTicks to sec:  {0}", glassAverage / (double)Stopwatch.Frequency);
            Console.WriteLine("Raw/Glass {0}", (double) glassAverage/(double)rawAverage);


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
