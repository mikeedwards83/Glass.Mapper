using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Tests.Configuration.Attributes
{
    [TestFixture]
    public class SitecoreAttributeConfigurationLoaderFixture
    {

        [Test]
        public void Load_StubClassConfigured_ReturnsStubClassAndProperties()
        {
            //Assign
            var loader = new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Tests");

            //Act
            var results = loader.Load();
            
            //Assert
            Assert.GreaterOrEqual(results.Count(), 0);

            var typeConfig = results.First(x => x.Type == typeof (StubClass));
            Assert.IsNotNull(typeConfig);

            var propertyNames = new[] {"Children", "Field", "Id", "Info", "Linked", "Node", "Parent", "Query"};

            foreach(var propertyName in propertyNames)
            {
                var propInfo = typeof (StubClass).GetProperty(propertyName);
                Assert.IsTrue(typeConfig.Properties.Any(x=>x.PropertyInfo == propInfo));
            }

        }


        #region Stubs

        [SitecoreType]
        public class StubClass
        {
            [SitecoreChildren]
            public string Children { get; set; }

            [SitecoreField]
            public string Field { get; set; }

            [SitecoreId]
            public ID Id { get; set; }

            [SitecoreInfo]
            public string Info { get; set; }

            [SitecoreLinked]
            public string Linked { get; set; }

            [SitecoreNode]
            public string Node { get; set; }

            [SitecoreParent]
            public string Parent { get; set; }

            [SitecoreQuery("")]
            public string Query { get; set; }

            
        }

        #endregion
    }
}
