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
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Tests.Configuration.Attributes
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
        [TestCase("IsLazy")]
        [TestCase("InferType")]
        public void Does_SitecoreNodeAttributee_Have_Properties(string propertyName)
        {
            var properties = typeof(SitecoreParentAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        [Test]
        public void Does_Constructor_Set_IsLazy_True()
        {
            Assert.IsTrue(new SitecoreParentAttribute().IsLazy);
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



