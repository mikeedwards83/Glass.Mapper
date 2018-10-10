using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Sc.FakeDb.Configuation.Attributes
{
    [TestFixture]
    public class SitecoreQueryAttributeFixture
    {

        

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_SitecoreQueryConfigurationReturned()
        {
            //Assign
            SitecoreQueryAttribute attr = new SitecoreQueryAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("QueryContextProperty");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreQueryConfiguration;

            //Assert
            Assert.IsNotNull(result);
        }

       
        #endregion

        #region Stubs

        public class StubClass
        {
            public string QueryContextProperty { get; set; }
        }

        #endregion
    }
}




