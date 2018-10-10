

using System.Linq;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Sc.FakeDb.Configuation.Attributes
{
    [TestFixture]
    public class SitecoreParentAttributeFixture
    {
        [Test]
        public void Does_SitecoreParentAttribute_Extend_ParentAttribute()
        {
            Assert.IsTrue(typeof(ParentAttribute).IsAssignableFrom(typeof(SitecoreParentAttribute)));
        }

        [Test]
        [TestCase("InferType")]
        public void Does_SitecoreNodeAttributee_Have_Properties(string propertyName)
        {
            var properties = typeof(SitecoreParentAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

       

        [Test]
        public void Does_Constructor_Set_InferType_False()
        {
            Assert.IsFalse(new SitecoreParentAttribute().InferType);
        }

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_SitecoreInfoConfigurationReturned()
        {
            //Assign
            var attr = new SitecoreParentAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreParentConfiguration;

            //Assert
            Assert.IsNotNull(result);
           
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public string DummyProperty { get; set; }
        }

        #endregion
    }
}




