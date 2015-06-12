using System;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;

namespace Glass.Sandbox
{
    [TestFixture]
    public class ObjectCreationBase
    {
        protected void TestObject(Func<SourceItem, StubClass> func)
        {
            // Assign
            SourceItem item1 = new SourceItem { Property1 = "Item 1 - Property 1", Property2 = 1 };
            SourceItem item2 = new SourceItem {Property1 = "Item 2 - Property 1", Property2 = 2};

            // Act
            StubClass stubClass1 = func(item1);
            StubClass stubClass2 = func(item2);

            // Assert
            stubClass1.Property1.Should().Be("Item 1 - Property 1");
            stubClass1.Property2.Should().Be(1);

            stubClass2.Property2.Should().Be(2);
            stubClass2.Property1.Should().Be("Item 2 - Property 1");
        }

        protected void TestObjectLots(Func<SourceItem, StubClass> func)
        {
            // Assign
            SourceItem item1 = new SourceItem { Property1 = "Item 1 - Property 1", Property2 = 1 };

            // Act
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 20000; i++)
            {
                StubClass stubClass1 = func(item1);
            }
            sw.Stop();

            // Assert
            Console.WriteLine("Total elapsed ticks {0}", sw.ElapsedTicks);
            Console.WriteLine("Total elapsed ms {0}", sw.ElapsedMilliseconds);
        }

        public class SourceItem
        {
            public string Property1 { get; set; }
            public int Property2 { get; set; }
        }

        public class StubClass
        {
            public string Property1 { get; set; }

            public int Property2 { get; set; }
        }

    }
}
