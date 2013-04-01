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

using System.Linq;
using FluentAssertions;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Umb.Tests.Configuration.Attributes
{
    [TestFixture]
    public class UmbracoAttributeConfigurationLoaderFixture
    {

        [Test]
        public void Load_StubClassConfigured_ReturnsStubClassAndProperties()
        {
            //Assign
            var loader = new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Tests");

            //Act
            var results = loader.Load();
            
            //Assert
            results.Count().Should().BeGreaterOrEqualTo(0);

            var typeConfig = results.First(x => x.Type == typeof (StubClass));
            typeConfig.Should().NotBeNull();

            var propertyNames = new[] {"Children", "Property", "Id", /*"Info", "Linked", "Node",*/ "Parent"/*, "Query"*/};

            foreach(var propertyName in propertyNames)
            {
                var propInfo = typeof (StubClass).GetProperty(propertyName);
                typeConfig.Properties.Any(x=>x.PropertyInfo == propInfo).Should().BeTrue();
            }

        }

        #region Stubs

        [UmbracoType]
        public class StubClass
        {
            [UmbracoChildren]
            public string Children { get; set; }

            [UmbracoProperty]
            public string Property { get; set; }

            [UmbracoId]
            public int Id { get; set; }

            //[UmbracoInfo]
            //public string Info { get; set; }

            //[UmbracoLinked]
            //public string Linked { get; set; }

            //[UmbracoNode]
            //public string Node { get; set; }

            [UmbracoParent]
            public string Parent { get; set; }

            //[UmbracoQuery("")]
            //public string Query { get; set; }

            
        }

        #endregion
    }
}



