using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Tests.Configuration.Attributes
{
    [TestFixture]
    public class QueryAttributeFixture
    {
        [Test]
        public void Does_QueryAttribute_Extend_AbstractPropertyAttribute()
        {
            Assert.IsTrue(typeof(AbstractPropertyAttribute).IsAssignableFrom(typeof(QueryAttribute)));
        }

        [Test]
        [TestCase("Query")]
        [TestCase("IsLazy")]
        [TestCase("IsRelative")]
        [TestCase("InferType")]
        [TestCase("UseQueryContext")]
        public void Does_QueryAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(QueryAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        [Test]
        public void Does_Constructor_Set_IsLazy_True()
        {
            Assert.IsTrue(new TestQueryAttribute().IsLazy);
        }

        [Test]
        public void Does_Constructor_Set_UseQueryContext_False()
        {
            Assert.IsFalse(new TestQueryAttribute().UseQueryContext);
        }

        [Test]
        public void Does_Constructor_Set_IsRelative_False()
        {
            Assert.IsFalse(new TestQueryAttribute().IsRelative);
        }

        [Test]
        public void Does_Constructor_Set_InferType_False()
        {
            Assert.IsFalse(new TestQueryAttribute().InferType);
        }

        [Test]
        public void Does_Constructor_Set_Query()
        {
            var query = "This is a query";
            Assert.AreEqual(query, new TestQueryAttribute(query).Query);
        }

        private class TestQueryAttribute : QueryAttribute
        {
            public TestQueryAttribute() : base("This is a query") { }
            public TestQueryAttribute(string query) : base(query) { }

            public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
            {
                throw new NotImplementedException();
            }
        }
    }
}
