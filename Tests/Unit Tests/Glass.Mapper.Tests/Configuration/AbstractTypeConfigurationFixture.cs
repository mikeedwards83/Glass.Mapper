using System.Linq;
using Glass.Mapper.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Configuration
{
    [TestFixture]
    public class AbstractTypeConfigurationFixture
    {
        private AbstractTypeConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            _configuration = new StubAbstractTypeConfiguration();
        }

        #region AutoMapProperties

        [Test]
        public void AutoMapProperties_ClassNoPropertiesStaticallyMapped_AutoMapsAllProperties()
        {
            //Assign
            _configuration.Type = typeof(StubClass);
            Assert.AreEqual(0, _configuration.Properties.Count());
            _configuration.AutoMap = true;
            //Act
            _configuration.PerformAutoMap();

            //Assert
            Assert.AreEqual(4, _configuration.Properties.Count());
            Assert.AreEqual(1, _configuration.PrivateProperties.Count());
        }

        [Test]
        public void AutoMapProperties_ClassSomeStaticallyMapped_AutoMapsAllProperties()
        {
            //Assign
            _configuration.Type = typeof(StubClass);
            _configuration.AutoMap = true;

            Assert.AreEqual(0, _configuration.Properties.Count());

            AbstractPropertyConfiguration property1 = Substitute.For<AbstractPropertyConfiguration>();
            property1.PropertyInfo = _configuration.Type.GetProperty("Prop1");
            AbstractPropertyConfiguration property2 = Substitute.For<AbstractPropertyConfiguration>();
            property2.PropertyInfo = _configuration.Type.GetProperty("Prop2");

            _configuration.AddProperty(property1);
            _configuration.AddProperty(property2);

            //Act
            _configuration.PerformAutoMap();

            //Assert
            Assert.AreEqual(4, _configuration.Properties.Count());
            Assert.AreEqual(1, _configuration.PrivateProperties.Count());
        }

        [Test]
        public void AutoMapProperties_InterfaceNoPropertiesStaticallyMapped_AutoMapsAllProperties()
        {
            //Assign
            _configuration.Type = typeof(IStubInterface);
            _configuration.AutoMap = true;

            Assert.AreEqual(0, _configuration.Properties.Count());

            //Act
            _configuration.PerformAutoMap();

            //Assert
            Assert.AreEqual(5, _configuration.Properties.Count());
        }

        [Test]
        public void AutoMapProperties_InterfaceSomeStaticallyMapped_AutoMapsAllProperties()
        {
            //Assign
            _configuration.Type = typeof(IStubInterface);
            _configuration.AutoMap = true;

            Assert.AreEqual(0, _configuration.Properties.Count());

            AbstractPropertyConfiguration property1 = Substitute.For<AbstractPropertyConfiguration>();
            property1.PropertyInfo = _configuration.Type.GetProperty("Prop1");
            AbstractPropertyConfiguration property2 = Substitute.For<AbstractPropertyConfiguration>();
            property2.PropertyInfo = _configuration.Type.GetProperty("Prop2");

            _configuration.AddProperty(property1);
            _configuration.AddProperty(property2);

            //Act
            _configuration.PerformAutoMap();

            //Assert
            Assert.AreEqual(5, _configuration.Properties.Count());
        }
    

    #endregion


        #region Method - AddProperty

        [Test]
        public void AddProperty_PropertyAdded_PropertiesListContainsOneItem()
        {
            //Assign
            var property = Substitute.For<AbstractPropertyConfiguration>();
            property.PropertyInfo = typeof(StubClass).GetProperties().First();

            //Act
            _configuration.AddProperty(property);

            //Assert
            Assert.AreEqual(1, _configuration.Properties.Count());
            Assert.AreEqual(property, _configuration.Properties.First());
        }

        #endregion

        #region Stub

        public class StubAbstractTypeConfiguration : AbstractTypeConfiguration
        {

            protected override AbstractPropertyConfiguration AutoMapProperty(System.Reflection.PropertyInfo property)
            {
                var prop = Substitute.For<AbstractPropertyConfiguration>();
                prop.PropertyInfo = property;
                return prop;
            }
        }

        public class StubClass:StubClassBase
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
            private string Prop3 { get; set; }
        }
        public class StubClassBase 
        {
            protected string Prop4 { get; set; }
            public virtual string Prop5 { get; set; }
        }

        public interface IStubInterface : IStubInterfaceBase
        {
             string Prop1 { get; set; }
             int Prop2 { get; set; }
             string Prop3 { get; set; }
        }
        public interface IStubInterfaceBase
        {
             string Prop4 { get; set; }
             string Prop5 { get; set; }
        }

        #endregion

    }
}




