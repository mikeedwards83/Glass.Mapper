using System;
using System.Diagnostics;
using Glass.Mapper.Sc.ModelCache;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Mvc.Tests
{
    [TestFixture]
    public class ModelFinderTestFixture
    {
        [Test]
        public void GetModelFromInherits()
        {
            // Arrange
            ModelFinder modelFinder = new ModelFinder();

            // Act
            Type modelType = modelFinder.GetModelFromFile("Example1.cshtml");

            // Assert
            Assert.AreEqual(typeof(StubModel), modelType);
        }

        [Test]
        public void GetModelFromModelDeclarative()
        {
            // Arrange
            ModelFinder modelFinder = new ModelFinder();

            // Act
            Type modelType = modelFinder.GetModelFromFile("Example2.cshtml");

            // Assert
            Assert.AreEqual(typeof(StubModel), modelType);
        }

        [Ignore("Test for performance")]
        [Test]
        public void GetModelLots()
        {
            // Arrange
            ModelFinder modelFinder = new ModelFinder();

            // Act
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 1000; i++)
            {
                Type modelType = modelFinder.GetModelFromFile("Example2.cshtml");
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [Test]
        public void GetModelFromCompilation()
        {
            // Arrange
            ModelFinder modelFinder = new ModelFinder();

            // Act
            Type modelType = modelFinder.GetModelFromCompiled("Test", "Example2.cshtml");

            // Assert
            Assert.AreEqual(typeof(StubModel), modelType);
        }
    }
}
