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
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.Tests.Configuration.Attributes
{
    [TestFixture]
    public class SitecoreLinkedAttributeFixture
    {
        [Test]
        public void Does_SitecoreIdAttribute_Extend_IdAttribute()
        {
            Assert.IsTrue(typeof(LinkedAttribute).IsAssignableFrom(typeof(SitecoreLinkedAttribute)));
        }

        [Test]
        [TestCase("InferType")]
        [TestCase("IsLazy")]
        [TestCase("Option")]
        public void Does_ChildrenAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(SitecoreLinkedAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        [Test]
        public void Default_Constructor_Set_Setting_To_Default()
        {
            var testSitecoreFieldAttribute = new SitecoreLinkedAttribute();
            Assert.AreEqual(testSitecoreFieldAttribute.Option, SitecoreLinkedOptions.All);
        }

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_SitecoreLinkedConfigurationReturned()
        {
            //Assign
            SitecoreLinkedAttribute attr = new SitecoreLinkedAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreLinkedConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SitecoreLinkedOptions.All, result.Option);
        }

        [Test]
        public void Configure_OptionSet_OptionSetOnConfiguration()
        {
            //Assign
            SitecoreLinkedAttribute attr = new SitecoreLinkedAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");
            attr.Option = SitecoreLinkedOptions.Referrers;
            

            //Act
            var result = attr.Configure(propertyInfo) as SitecoreLinkedConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SitecoreLinkedOptions.Referrers, result.Option);
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



