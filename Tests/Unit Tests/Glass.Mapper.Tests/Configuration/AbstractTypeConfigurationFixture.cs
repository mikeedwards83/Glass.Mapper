using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;
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

        


        #region Method - AddProperty

        [Test]
        public void AddProperty_PropertyAdded_PropertiesListContainsOneItem()
        {
            //Assign
            var property = Substitute.For<AbstractPropertyConfiguration>();

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
        }

        #endregion

    }
}
