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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Integration
{
    [TestFixture]
    
    public class PerformanceTests
    {

        private string _expected;
        private Guid _id;
        private Context _context;
        private Database _db;
        private SitecoreService _service;
        private bool _hasRun = false;
        private Stopwatch _glassWatch;
        private Stopwatch _rawWatch;
        private double _glassTotal;
        private double _rawTotal;

        [SetUp]
        public void Setup()
        {
            if (_hasRun)
            {
                return;
            }
            else
                _hasRun = true;

            _glassWatch = new Stopwatch();
            _rawWatch= new Stopwatch();
            

            _expected = "hello world";
            _id = new Guid("{59784F74-F830-4BCD-B1F0-1A08616EF726}");

            _context = Context.Create(Utilities.CreateStandardResolver());


            _context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            _db = Factory.GetDatabase("master");

            //       service.Profiler = new SimpleProfiler();

            _service = new SitecoreService(_db);

            var item = _db.GetItem(new ID(_id));
            using (new ItemEditing(item, true))
            {
                item["Field"] = _expected;
            }
        }

        [Test]
        public void GetItemByIdvsItemByPath()
        {
            _glassWatch.Reset();
            // Warm up
            ID id = new ID(_id);

            var item1 = _db.GetItem(id);
            string path = item1.Paths.FullPath;
            Console.WriteLine(path);
            var item2 = _db.GetItem(path);

            string itemIdString = _id.ToString();
            var item3 = _db.GetItem(itemIdString);

            // Start
            _glassWatch.Start();
            for (var i = 0; i < 10000; i++)
            {
                _db.GetItem(id);
            }
            _glassWatch.Stop();
            Console.WriteLine("Item by Id: {0}", _glassWatch.ElapsedMilliseconds);

            _glassWatch.Reset();
            _glassWatch.Start();
            for (var i = 0; i < 10000; i++)
            {
                _db.GetItem(path);
            }
            _glassWatch.Stop();

            Console.WriteLine("Item by Path: {0}", _glassWatch.ElapsedMilliseconds);

            _glassWatch.Reset();
            _glassWatch.Start();

            for (var i = 0; i < 10000; i++)
            {
                _db.GetItem(itemIdString);
            }
            _glassWatch.Stop();

            Console.WriteLine("Item by Id String: {0}", _glassWatch.ElapsedMilliseconds);
        }

        [Test]
        [Timeout(120000)]
        [Repeat(10000)]
        public void GetItems(
            [Values(1, 1000, 10000, 50000)] int count
            )
        {
            _glassWatch.Reset();
            _rawWatch.Reset();

            for (int i = 0; i < count; i++)
            {
                _rawWatch.Start();
                var rawItem = _db.GetItem(new ID(_id));
                var value1 = rawItem["Field"];
                _rawWatch.Stop();
                _rawTotal = _rawWatch.ElapsedTicks;

                _glassWatch.Start();
                var glassItem = _service.GetItem<StubClass>(_id);
                var value2 = glassItem.Field;
                _glassWatch.Stop();
                _glassTotal = _glassWatch.ElapsedTicks;

            }

            double total = _glassTotal / _rawTotal;
            Console.WriteLine("Performance Test Count: {0} Ratio: {1} Average: {2}".Formatted(count, total, _glassTotal/count));
        }

        [Test]
        [Timeout(120000)]
        [Repeat(10000)]
        public void GetItems_LotsOfProperties(
            [Values(1000, 10000, 50000)] int count
            )
        {

            _glassWatch.Reset();
            _rawWatch.Reset();

            for (int i = 0; i < count; i++)
            {
                _rawWatch.Start();
                var rawItem = _db.GetItem(new ID(_id));
                var value1 = rawItem["Field"];
                _rawWatch.Stop();
                _rawTotal = _rawWatch.ElapsedTicks;

                _glassWatch.Start();
                var glassItem = _service.GetItem<StubClassWithLotsOfProperties>(_id);
                var value2 = glassItem.Field1;
                _glassWatch.Stop();
                _glassTotal = _glassWatch.ElapsedTicks;

            }

            double total = _glassTotal / _rawTotal;
            Console.WriteLine("Performance Test Count: {0} Ratio: {1} Average: {2}".Formatted(count, total, _glassTotal/count));
        }

        [Test]
        [Timeout(120000)]
        public void GetWholeDb()
        {
            List<Item> items = new List<Item>();
            var rawItem = _db.GetItem("/sitecore");
            _service.Cast<StubForWholeDb>(rawItem);

            foreach (Item child in rawItem.GetChildren())
            {
                AddChildren(child, items);
            }

            var count = 0;
            _glassWatch.Reset();
            _rawWatch.Reset();

            foreach (var item in items)
            {
                _rawWatch.Start();
                if (item.Versions.Count > 0)
                {
                    var value1 = rawItem["__DisplayName"];
                }
                _rawWatch.Stop();

                _glassWatch.Start();
                var glassItem = _service.Cast<StubForWholeDb>(item);
                if (glassItem != null)
                {
                    var value2 = glassItem.Field;
                }
                _glassWatch.Stop();

                count++;

            }

            _rawTotal += _rawWatch.ElapsedTicks;
            _glassTotal = _glassWatch.ElapsedTicks;


            double total = _glassTotal / _rawTotal;
            Console.WriteLine("Performance Test Count: {0} Ratio: {1} Average: {2}".Formatted(count, total, _glassTotal / count));
            Console.WriteLine("Total Items {0}", count);

        }

        private void AddChildren(Item parent, List<Item> items)
        {
            items.Add(parent);
            if (parent.HasChildren)
            {
                foreach (Item child in parent.GetChildren())
                {
                    AddChildren(child, items);
                }
            }
        }

        [Test]
        [Timeout(120000)]
        public void GetItems_InheritanceTest(
            [Values(100, 200, 300)] int count
            )
        {
            string path = "/sitecore/content/Tests/PerformanceTests/InheritanceTest";

            for (int i = 0; i < count; i++)
            {
                _glassWatch.Reset();
                _rawWatch.Reset();

                _rawWatch.Start();

                var glassItem1 = _service.GetItem<StubClassLevel5>(path);
                var value1 = glassItem1.Field;

                _rawWatch.Stop();
                _rawTotal = _rawWatch.ElapsedTicks;

                _glassWatch.Start();
                var glassItem2 = _service.GetItem<StubClassLevel1>(path);
                var value2 = glassItem2.Field;
                _glassWatch.Stop();
                _glassTotal = _glassWatch.ElapsedTicks;

            }

            double total = _glassTotal / _rawTotal;
            Console.WriteLine("Performance inheritance Test Count: {0},  Single: {1}, 5 Levels: {2}, Ratio: {3}".Formatted(count, _rawTotal, _glassTotal, total));
        }

        [Test]
        public void CreateService_Lots()
        {
            // Warm up
            const int count = 10000;
            Stopwatch sw = new Stopwatch();

            ISitecoreService service = new SitecoreService(_db);

            sw.Start();
            for (var i = 0; i < count; i++)
            {
                service = new SitecoreService(_db);
            }
            sw.Stop();

            Console.WriteLine("Elapsed: {0}", sw.ElapsedMilliseconds);


            Console.WriteLine("Elapsed: {0}", sw.ElapsedMilliseconds);
            Console.WriteLine("Object construction missed: {0}", _context.PipelineFactory.ObjectConstructionPipelinePool.Missed);
            Console.WriteLine("Configuration resolver missed: {0}", _context.PipelineFactory.ConfigurationResolverPipelinePool.Missed);
            Console.WriteLine("Object saving missed: {0}", _context.PipelineFactory.ObjectSavingPipelinePool.Missed);
            Console.WriteLine("Data mapper resolver missed: {0}", _context.PipelineFactory.DataMapperResolverPipelinePool.Missed);

        }

        [Test]
        public void CreateOldService_Lots()
        {
            // Warm up
            const int count = 10000;
            Stopwatch sw = new Stopwatch();

            // Warm up
            ISitecoreService service2 = new SitecoreService2(_db);

            sw.Start();
            for (var i = 0; i < count; i++)
            {
                //Console.WriteLine(i);
                service2 = new SitecoreService2(_db);
            }
            sw.Stop();


            Console.WriteLine("Elapsed: {0}", sw.ElapsedMilliseconds);
            Console.WriteLine("Object construction missed: {0}", _context.PipelineFactory.ObjectConstructionPipelinePool.Missed);
            Console.WriteLine("Configuration resolver missed: {0}", _context.PipelineFactory.ConfigurationResolverPipelinePool.Missed);
            Console.WriteLine("Object saving missed: {0}", _context.PipelineFactory.ObjectSavingPipelinePool.Missed);
            Console.WriteLine("Data mapper resolver missed: {0}", _context.PipelineFactory.DataMapperResolverPipelinePool.Missed);

        }

        [Test]
        public void ScaffoldService_Lots()
        {
            // Warm up
            ObjectConstruction objectConstruction = new ObjectConstruction(_context.DependencyResolver.ObjectConstructionFactory.GetItems());
            Stopwatch sw = Stopwatch.StartNew();
            for (var i = 0; i < 5000; i++)
            {
                objectConstruction = new ObjectConstruction(_context.DependencyResolver.ObjectConstructionFactory.GetItems());
            }
            Console.WriteLine("Original: {0}", sw.ElapsedMilliseconds);

            sw.Stop();

            PipelinePool<ObjectConstruction> pipelinePool = new PipelinePool<ObjectConstruction>(_context.DependencyResolver, 2000, x => new ObjectConstruction(x.ObjectConstructionFactory.GetItems()));

            sw.Restart();
            for (var i = 0; i < 5000; i++)
            {
                objectConstruction = pipelinePool.GetFromPool();
            }

            Console.WriteLine("Pipeline: {0}", sw.ElapsedMilliseconds);

            Console.WriteLine("Pipeline Missed: {0}", pipelinePool.Missed);

            objectConstruction = pipelinePool.GetFromPool();
        }


        #region Stubs


        [SitecoreType]
        public class StubClassWithLotsOfProperties
        {
            [SitecoreField("Field",Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field1 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field2 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field3 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field4 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field5 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field6 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field7 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field8 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field9 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field10 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field11 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field12 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field13 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field14 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field15 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field16 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field17 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field18 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field19 { get; set; }

            [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field20 { get; set; }


            [SitecoreId]
            public virtual Guid Id { get; set; }
        }

        [SitecoreType]
        public class StubForWholeDb
        {
            [SitecoreField("__Display Name")]
            public virtual string Field { get; set; }
        }

        [SitecoreType]
        public class StubClass
        {
            [SitecoreField(Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field { get; set; }
            
        }

        [SitecoreType]
        public class StubClassLevel1 : StubClassLevel2
        {
            
        }
        [SitecoreType]
        public class StubClassLevel2 : StubClassLevel3
        {

        }
        [SitecoreType]
        public class StubClassLevel3 : StubClassLevel4
        {

        }
        [SitecoreType]
        public class StubClassLevel4 : StubClassLevel5
        {

        }
        [SitecoreType]
        public class StubClassLevel5
        {
            [SitecoreField(Setting = SitecoreFieldSettings.RichTextRaw)]
            public virtual string Field { get; set; }

            [SitecoreId]
            public virtual Guid Id { get; set; }
        }

        #endregion




        //   [Test]
        //   [Timeout(120000)]
        //   public void GetItems()
        //   {

        //       //Assign
        //       int[] counts = new int[] {1, 100, 1000, 10000, 50000, 100000, 150000,200000};
        //       foreach (var count in counts)
        //       {
        //           GetItemsTest(count);
        //       }
        //   }
        //   private void GetItemsTest(int count){

        //   var expected = "hello world";
        //       var id = new Guid("{59784F74-F830-4BCD-B1F0-1A08616EF726}");

        //       var context = Context.Create(new SitecoreConfig());


        //       context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

        //       var db = Sitecore.Configuration.Factory.GetDatabase("master");
        //       var service = new SitecoreService(db);

        ////       service.Profiler = new SimpleProfiler();

        //       var item = db.GetItem(new ID(id));
        //       using (new ItemEditing(item, true))
        //       {
        //           item["Field"] = expected;
        //       }

        //       //Act

        //       //get Sitecore raw
        //       var rawTotal = (long)0;
        //           var watch1 = new Stopwatch();

        //       for (int i = 0; i < count; i++)
        //       {

        //           watch1.Start();
        //           var rawItem = db.GetItem(new ID(id));
        //           var value = rawItem["Field"];
        //           watch1.Stop();
        //         Assert.AreEqual(expected, value);
        //           rawTotal += watch1.ElapsedTicks;
        //       }

        //       long rawAverage = rawTotal / count;

        //       //Console.WriteLine("Performance Test - 1000 - Raw - {0}", rawAverage);
        //      // Console.WriteLine("Raw ElapsedTicks to sec:  {0}", rawAverage / (double)Stopwatch.Frequency);

        //       var glassTotal = (long)0;
        //           var watch2 = new Stopwatch();
        //           for (int i = 0; i < count; i++)
        //       {

        //           watch2.Start();
        //           var glassItem = service.GetItem<StubClass>(id);
        //          var value = glassItem.Field;
        //           watch2.Stop();
        //           Assert.AreEqual(expected, value);
        //           glassTotal += watch2.ElapsedTicks;
        //       }


        //           long glassAverage = glassTotal / count;

        //      // Console.WriteLine("Performance Test - 1000 - Glass - {0}", glassAverage);
        //       //Console.WriteLine("Glass ElapsedTicks to sec:  {0}", glassAverage / (double)Stopwatch.Frequency);
        //       Console.WriteLine("{1}: Raw/Glass {0}", (double) glassAverage/(double)rawAverage, count);


        //       //Assert
        //       //ME - at the moment I am allowing glass to take twice the time. I would hope to reduce this
        //       //Assert.LessOrEqual(glassAverage, rawAverage*2);


        //   }



    }
}




