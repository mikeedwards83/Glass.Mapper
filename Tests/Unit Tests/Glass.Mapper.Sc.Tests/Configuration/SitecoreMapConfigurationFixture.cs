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
using System.Reflection;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.Maps;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Tests.Configuration
{
    [TestFixture]
    public class SitecoreMapConfigurationFixture
    {

        [Test]
        public void ConfigMapProperties_MapsSinglePropertyToConfiguration()
        {
            //Assign
            IDependencyResolver resolver = Substitute.For<IDependencyResolver>();
            StubMap1 stubMap = new StubMap1(resolver);
            SitecoreFluentConfigurationLoader loader = new SitecoreFluentConfigurationLoader();

            //Act
            stubMap.PerformMap(loader);

            //Assert
            Assert.AreEqual(1, stubMap.GlassType.Config.Properties.Count());
            Assert.IsNull(stubMap.GlassType.Config.IdConfig);            
        }

        [Test]
        public void ConfigMapProperties_MapsAllPropertiesToConfiguration()
        {
            //Assign
            IDependencyResolver resolver = Substitute.For<IDependencyResolver>();
            StubMap2 stubMap = new StubMap2(resolver);
            SitecoreFluentConfigurationLoader loader = new SitecoreFluentConfigurationLoader();

            //Act
            stubMap.PerformMap(loader);

            //Assert
            Assert.AreEqual(5, stubMap.GlassType.Config.Properties.Count());
            Assert.IsNotNull(stubMap.GlassType.Config.IdConfig);
        }

        #region Stubs


        public class StubMap1 : SitecoreGlassMap<StubClass>
        {
            public StubMap1(IDependencyResolver dependencyResolver) : base(dependencyResolver)
            {
            }

            public override void Configure()
            {
                Map(x => x.Field(y => y.Name).FieldName("Name"));
            }
        }

        public class StubMap2 : SitecoreGlassMap<StubClass>
        {
            public StubMap2(IDependencyResolver dependencyResolver)
                : base(dependencyResolver)
            {
            }

            public override void Configure()
            {
                Map(
                    x =>
                    {
                        x.Field(y => y.Name).FieldName("Name");
                        x.Parent(y => y.Parent);
                        x.Children(y => y.Children);
                        x.Field(y => y.FieldName).FieldName("Field Name");
                        x.Id(y => y.Id);
                    });
            }
        }

        public class StubSitecoreTypeConfiguration : SitecoreTypeConfiguration
        {
            
        }

        public class StubClass:StubClassBase
        {
            public string Name { get; set; }
            public string Parent { get; set; }
            public string Children { get; set; }
            public string FieldName { get; set; }
        }

        public class StubClassBase
        {
            public virtual string Id { get; set; }
            
        }

        #endregion

    }
}

