using System;
using System.Security.Policy;
using NUnit.Framework;

namespace Glass.Mapper.Tests
{
    [TestFixture]
    public class GlassLazyTestFixture
    {
        [Test]
        public void GlassLazyString_implicitly_converts_to_string()
        {
            // Arrange
            GlassLazy<string> glassLazy = new GlassLazy<string>(() => "Test String");

            // Assert
            Assert.AreEqual("Test String", glassLazy.ToString());
            Assert.IsTrue(glassLazy.IsValueCreated);
            Assert.IsTrue(glassLazy == "Test String");
        }

        [Test]
        public void GlassLazyString_implicitly_converts_to_object()
        {
            // Arrange
            GlassLazy<LazyTestStub> glassLazy = new GlassLazy<LazyTestStub>(() => new LazyTestStub { Test = "Test String" });
            LazyTestStub lazyStub = glassLazy;

            // Assert
            Assert.IsTrue(glassLazy.IsValueCreated);
            Assert.AreEqual("Glass.Mapper.Tests.GlassLazyTestFixture+LazyTestStub", glassLazy.ToString());
            Assert.AreEqual(glassLazy.Value, lazyStub);
            Assert.IsTrue(lazyStub.Test == "Test String");
        }

        [Test]
      
        public void GlassLazyString_excepts_gracefully()
        {

            // Arrange
            GlassLazy<LazyTestStub> glassLazy = new GlassLazy<LazyTestStub>(
                () => { throw new Exception("Something Went Badly Wrong"); });
            Assert.Throws<InvalidOperationException>(() =>
            {
                LazyTestStub lazyStub = glassLazy;
            });

            // Assert
        }

        [Test]
        public void GlassLazyString_implicitly_converts_from_string()
        {
            // Arrange
            string target = "Test String";

            // Act
            GlassLazy<string> glassLazy = target;

            // Assert
            Assert.AreEqual("Test String", glassLazy.ToString());
            Assert.IsTrue(glassLazy == "Test String");
            Assert.IsTrue(glassLazy.IsValueCreated);
        }

        [Test]
        public void GlassLazyString_evaluates_when_null()
        {
            // Arrange
            GlassLazy<string> glassLazy = null;

            // Act
            string assigned = glassLazy;

            // Assert
            Assert.IsNull(assigned);
        }

        public class LazyTestStub
        {
            public string Test { get; set; }
        }
    }
}
