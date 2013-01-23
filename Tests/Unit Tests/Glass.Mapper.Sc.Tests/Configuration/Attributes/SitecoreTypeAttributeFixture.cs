/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using NSubstitute;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Tests.Configuration.Attributes
{
    [TestFixture]
    public class SitecoreTypeAttributeFixture
    {
        [Test]
        public void Does_SitecoreTypeAttribute_Extend_AbstractClassAttribute()
        {
            Assert.IsTrue(typeof(AbstractTypeAttribute).IsAssignableFrom(typeof(SitecoreTypeAttribute)));
        }

        [Test]
        [TestCase("TemplateId")]
        [TestCase("BranchId")]
        public void Does_SitecoreTypeAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(SitecoreTypeAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        #region Method - Configure

        [Test]
        public void Configure_ConfigurationIsOfCorrectType_NoExceptionThrown()
        {
            //Assign
            var attr = new SitecoreTypeAttribute();
            var config = new SitecoreTypeConfiguration();
            var type = typeof (StubClass);

            var templateIdExpected = Guid.Empty;
            var branchIdExptected = Guid.Empty;

            //Act
            attr.Configure(type, config);

            //Assert
            Assert.AreEqual(type, config.Type);
            Assert.AreEqual(templateIdExpected, config.TemplateId);
            Assert.AreEqual(branchIdExptected, config.BranchId);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void Configure_ConfigurationIsIncorrectType_ExceptionThrown()
        {
            //Assign
            var attr = new SitecoreTypeAttribute();
            var config = Substitute.For<AbstractTypeConfiguration>();
            var type = typeof(StubClass);

            var templateIdExpected = Guid.Empty;
            var branchIdExptected = Guid.Empty;

            //Act
            attr.Configure(type, config);

            //Assert
        }

        [Test]
        public void Configure_AttributeHasTemplateId_TemplateIdSetOnConfig()
        {
            //Assign
            var attr = new SitecoreTypeAttribute();
            var config = new SitecoreTypeConfiguration();
            var type = typeof(StubClass);

            var templateIdExpected = new Guid();
            var branchIdExptected = Guid.Empty;

            attr.TemplateId = templateIdExpected.ToString();

            //Act
            attr.Configure(type, config);

            //Assert
            Assert.AreEqual(type, config.Type);
              Assert.AreEqual(templateIdExpected, config.TemplateId);
            Assert.AreEqual(branchIdExptected, config.BranchId);
        }

        [Test]
        public void Configure_AttributeHasBranchId_BranchIdSetOnConfig()
        {
            //Assign
            var attr = new SitecoreTypeAttribute();
            var config = new SitecoreTypeConfiguration();
            var type = typeof(StubClass);

            var templateIdExpected = Guid.Empty;
            var branchIdExptected = new Guid();

            attr.BranchId = branchIdExptected.ToString();

            //Act
            attr.Configure(type, config);

            //Assert
            Assert.AreEqual(type, config.Type);
            Assert.AreEqual(templateIdExpected, config.TemplateId);
            Assert.AreEqual(branchIdExptected, config.BranchId);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Configure_AttributeHasInvalidTemplateId_ExceptionThrown()
        {
            //Assign
            var attr = new SitecoreTypeAttribute();
            var config = new SitecoreTypeConfiguration();
            var type = typeof(StubClass);

            var templateIdExpected = Guid.Empty;
            var branchIdExptected = Guid.Empty;

            attr.TemplateId = "not a guid";

            //Act
            attr.Configure(type, config);

            //Assert
  
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Configure_AttributeHasInvalidBranchId_ExceptionThrown()
        {
            //Assign
            var attr = new SitecoreTypeAttribute();
            var config = new SitecoreTypeConfiguration();
            var type = typeof(StubClass);

            var templateIdExpected = Guid.Empty;
            var branchIdExptected = Guid.Empty;

            attr.BranchId = "not a guid";

            //Act
            attr.Configure(type, config);

            //Assert
          
        }

        #endregion

        #region Stubs

        public class StubClass
        {

        }

        #endregion
    }
}



