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



