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

using FluentAssertions;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Tests.Configuration.Attributes
{
    [TestFixture]
    public class UmbracoIdAttributeFixture
    {
        [Test]
        public void Does_UmbracoIdAttribute_Extend_IdAttribute()
        {
            typeof(IdAttribute).IsAssignableFrom(typeof(UmbracoIdAttribute)).Should().BeTrue();
        }

       

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_UmbracoIdConfigurationReturned()
        {
            //Assign
            UmbracoIdAttribute attr = new UmbracoIdAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as UmbracoIdConfiguration;

            //Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public int DummyProperty { get; set; }
        }

        #endregion
    }
}



