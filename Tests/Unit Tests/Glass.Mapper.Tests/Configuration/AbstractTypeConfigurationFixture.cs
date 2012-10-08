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

        #region Method - Configure

        [Test]
        public void Configure_ConfigureMethodCalled_SetsStandardProperties()
        {
            //Assign
            var type = this.GetType();
            var attribute = Substitute.For<AbstractTypeAttribute>();

            //Act
            _configuration.Configure(attribute, type);

            //Assert
            Assert.AreEqual(type, _configuration.Type);

        }

        #endregion

        #region Stub

        public class StubAbstractTypeConfiguration : AbstractTypeConfiguration
        {
        }

        #endregion

    }
}
