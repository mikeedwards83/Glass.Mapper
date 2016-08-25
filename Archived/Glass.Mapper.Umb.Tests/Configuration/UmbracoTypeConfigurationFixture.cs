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
using FluentAssertions;
using Glass.Mapper.Configuration;
using Glass.Mapper.Umb.Configuration;
using NUnit.Framework;

namespace Glass.Mapper.Umb.Tests.Configuration
{
    [TestFixture]
    public class UmbracoTypeConfigurationFixture
    {
        #region AutoMapProperty

        [Test]
        public void AutoMapProperty_MappingUmbracoInfo_ReturnsInfoConfig()
        {
            //Assign
            var typeConfig = new StubUmbracoTypeConfiguration();
            typeConfig.Type = typeof (StubClass);

            var prop = typeConfig.Type.GetProperty("Name");

            //Act
            var propConfig = typeConfig.StubAutoMapProperty(prop);

            //Assert
            (propConfig is UmbracoInfoConfiguration).Should().BeTrue();
            propConfig.PropertyInfo.ShouldBeEquivalentTo(prop);
        }

        [Test]
        public void AutoMapProperty_MappingUmbracoChildren_ReturnsChildrenConfig()
        {
            //Assign
            var typeConfig = new StubUmbracoTypeConfiguration();
            typeConfig.Type = typeof (StubClass);

            var prop = typeConfig.Type.GetProperty("Children");

            //Act
            var propConfig = typeConfig.StubAutoMapProperty(prop);

            //Assert
            (propConfig is UmbracoChildrenConfiguration).Should().BeTrue();
            propConfig.PropertyInfo.ShouldBeEquivalentTo(prop);
        }

        [Test]
        public void AutoMapProperty_MappingUmbracoParent_ReturnsParentConfig()
        {
            //Assign
            var typeConfig = new StubUmbracoTypeConfiguration();
            typeConfig.Type = typeof (StubClass);

            var prop = typeConfig.Type.GetProperty("Parent");

            //Act
            var propConfig = typeConfig.StubAutoMapProperty(prop);

            //Assert
            (propConfig is UmbracoParentConfiguration).Should().BeTrue();
            propConfig.PropertyInfo.ShouldBeEquivalentTo(prop);
        }

        [Test]
        public void AutoMapProperty_MappingUmbracoProperty_ReturnsPropertyConfig()
        {
            //Assign
            var typeConfig = new StubUmbracoTypeConfiguration();
            typeConfig.Type = typeof (StubClass);

            var prop = typeConfig.Type.GetProperty("PropertyAlias");

            //Act
            var propConfig = typeConfig.StubAutoMapProperty(prop);

            //Assert
            (propConfig is UmbracoPropertyConfiguration).Should().BeTrue();
            propConfig.PropertyInfo.ShouldBeEquivalentTo(prop);
            propConfig.CastTo<UmbracoPropertyConfiguration>().PropertyAlias.ShouldBeEquivalentTo("PropertyAlias");
        }

        [Test]
        public void AutoMapProperty_MappingUmbracoId_ReturnsIdConfig()
        {
            //Assign
            var typeConfig = new StubUmbracoTypeConfiguration();
            typeConfig.Type = typeof (StubClass);

            var prop = typeConfig.Type.GetProperty("Id");

            //Act
            var propConfig = typeConfig.StubAutoMapProperty(prop);

            //Assert
            (propConfig is UmbracoIdConfiguration).Should().BeTrue();
            propConfig.PropertyInfo.ShouldBeEquivalentTo(prop);
        }

        #endregion

        #region AutoMapProperties

        [Test]
        public void AutoMapProperties_MapsAllPropertiesToConfiguration()
        {
            //Assign
            var typeConfig = new StubUmbracoTypeConfiguration();
            typeConfig.Type = typeof(StubClass);
            typeConfig.AutoMap = true;

            //Act
            typeConfig.PerformAutoMap();

            //Assert
            typeConfig.Properties.Count().ShouldBeEquivalentTo(5);
            typeConfig.IdConfig.Should().NotBeNull();
        }

        #endregion

        #region Stubs
        
        public class StubUmbracoTypeConfiguration : UmbracoTypeConfiguration
        {
            public AbstractPropertyConfiguration StubAutoMapProperty(PropertyInfo property)
            {
               return base.AutoMapProperty(property);
            }
        }

        public class StubClass:StubClassBase
        {
            public string Name { get; set; }
            public string Parent { get; set; }
            public string Children { get; set; }
            public string PropertyAlias { get; set; }
        }
        public class StubClassBase
        {
            public virtual string Id { get; set; }
        }

        #endregion
    }
}

