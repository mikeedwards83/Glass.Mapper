using System.Linq;
using FluentAssertions;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Umb.Tests.Configuration.Attributes
{
    [TestFixture]
    public class UmbracoAttributeConfigurationLoaderFixture
    {

        [Test]
        public void Load_StubClassConfigured_ReturnsStubClassAndProperties()
        {
            //Assign
            var loader = new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Tests");

            //Act
            var results = loader.Load();
            
            //Assert
            results.Count().Should().BeGreaterOrEqualTo(0);

            var typeConfig = results.First(x => x.Type == typeof (StubClass));
            typeConfig.Should().NotBeNull();

            var propertyNames = new[] {"Children", "Property", "Id", /*"Info", "Linked", "Node",*/ "Parent"/*, "Query"*/};

            foreach(var propertyName in propertyNames)
            {
                var propInfo = typeof (StubClass).GetProperty(propertyName);
                typeConfig.Properties.Any(x=>x.PropertyInfo == propInfo).Should().BeTrue();
            }

        }

        #region Stubs

        [UmbracoType]
        public class StubClass
        {
            [UmbracoChildren]
            public string Children { get; set; }

            [UmbracoProperty]
            public string Property { get; set; }

            [UmbracoId]
            public int Id { get; set; }

            //[UmbracoInfo]
            //public string Info { get; set; }

            //[UmbracoLinked]
            //public string Linked { get; set; }

            //[UmbracoNode]
            //public string Node { get; set; }

            [UmbracoParent]
            public string Parent { get; set; }

            //[UmbracoQuery("")]
            //public string Query { get; set; }

            
        }

        #endregion
    }
}
