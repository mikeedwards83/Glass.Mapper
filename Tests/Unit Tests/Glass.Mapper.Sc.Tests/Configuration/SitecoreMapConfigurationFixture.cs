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
using Glass.Mapper.Configuration;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.DataMappers;
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
            StubBaseMap stubMap = new StubBaseMap();
            SitecoreFluentConfigurationLoader loader = new SitecoreFluentConfigurationLoader();
            ConfigurationMap configMap = new ConfigurationMap(new IGlassMap[] { stubMap });
            configMap.Load(loader);

            //Act
            stubMap.PerformMap(loader);

            //Assert
            Assert.AreEqual(1, stubMap.GlassType.Config.Properties.Count());
            Assert.IsNull(stubMap.GlassType.Config.ItemConfig);            
        }

        [Test]
        public void ConfigMapProperties_MapsAllPropertiesToConfiguration()
        {
            //Assign
            FinalStubMap finalStubMap = new FinalStubMap();
            PartialStub1Map partialStub1Map = new PartialStub1Map();
            StubBaseMap stubBaseMap = new StubBaseMap();
            PartialStub2Map partialStub2Map = new PartialStub2Map();

            SitecoreFluentConfigurationLoader loader = new SitecoreFluentConfigurationLoader();
            ConfigurationMap configMap = new ConfigurationMap(new IGlassMap[] { partialStub1Map, stubBaseMap, partialStub2Map, finalStubMap });
            configMap.Load(loader);

            //Act
            finalStubMap.PerformMap(loader);
            partialStub1Map.PerformMap(loader);
            stubBaseMap.PerformMap(loader);
            partialStub2Map.PerformMap(loader);

            //Assert
            Assert.AreEqual(5, finalStubMap.GlassType.Config.Properties.Count());
            SitecoreFieldConfiguration fieldNameProperty = finalStubMap.GlassType.Config.Properties.FirstOrDefault(x => x.PropertyInfo.Name == "FieldName") as SitecoreFieldConfiguration;
            Assert.AreEqual("Field Name", fieldNameProperty.FieldName);

            SitecoreInfoConfiguration qwertyProperty = finalStubMap.GlassType.Config.Properties.FirstOrDefault(x => x.PropertyInfo.Name == "Qwerty") as SitecoreInfoConfiguration;
            Assert.AreEqual(SitecoreInfoType.Name, qwertyProperty.Type);

            Assert.IsNotNull(finalStubMap.GlassType.Config.IdConfig);
        }

        [Test]
        public void ConfigMapProperties_ImportMap_CanOverrideExistingPropertyConfig()
        {
            //Assign
            FinalStubMap finalStubMap = new FinalStubMap();
            FinalStubSubClassMap finalStubSubClassMap = new FinalStubSubClassMap();
            PartialStub1Map partialStub1Map = new PartialStub1Map();
            PartialStub2Map partialStub2Map = new PartialStub2Map();
            StubBaseMap stubBaseMap = new StubBaseMap();

            SitecoreFluentConfigurationLoader loader = new SitecoreFluentConfigurationLoader();
            ConfigurationMap configMap = new ConfigurationMap(new IGlassMap[] { partialStub1Map, stubBaseMap, partialStub2Map, finalStubMap, finalStubSubClassMap });
            configMap.Load(loader);

            //Act
            finalStubMap.PerformMap(loader);
            finalStubSubClassMap.PerformMap(loader);

            //Assert
            Assert.AreEqual(5, finalStubSubClassMap.GlassType.Config.Properties.Count());
            SitecoreFieldConfiguration fieldNameProperty = finalStubSubClassMap.GlassType.Config.Properties.FirstOrDefault(x => x.PropertyInfo.Name == "FieldName") as SitecoreFieldConfiguration;
            Assert.AreEqual("Field Other Name", fieldNameProperty.FieldName);

            SitecoreInfoConfiguration qwertyProperty = finalStubSubClassMap.GlassType.Config.Properties.FirstOrDefault(x => x.PropertyInfo.Name == "Qwerty") as SitecoreInfoConfiguration;
            Assert.AreEqual(SitecoreInfoType.Name, qwertyProperty.Type);

            Assert.IsNotNull(finalStubSubClassMap.GlassType.Config.IdConfig);
        }

        #region Stubs

        public class StubBaseMap : SitecoreGlassMap<IStubBase>
        {
            public override void Configure()
            {
                Map(x => x.Id(y => y.Id));
            }
        }

        public class PartialStub1Map : SitecoreGlassMap<IPartialStub1>
        {
            public override void Configure()
            {
                Map(x =>
                {
                    ImportType<IStubBase>();
                    x.Info(y => y.Qwerty).InfoType(SitecoreInfoType.Name);
                });
            }
        }

        public class PartialStub2Map : SitecoreGlassMap<IPartialStub2>
        {
            public override void Configure()
            {
                Map(x =>
                {
                    ImportType<IStubBase>();
                    x.Parent(y => y.Parent);
                });
            }
        }

        public class FinalStubMap : SitecoreGlassMap<IFinalStub>
        {
            public override void Configure()
            {
                Map(
                    x =>
                    {
                        ImportType<IPartialStub1>();
                        ImportType<IPartialStub2>();
                        x.Children(y => y.Children);
                        x.Field(y => y.FieldName).FieldName("Field Name");
                        x.Id(y => y.Id);
                    });
            }
        }

        public class FinalStubSubClassMap : SitecoreGlassMap<IFinalSubClassStub>
        {
            public override void Configure()
            {
                Map(
                    x =>
                    {
                        ImportMap<IFinalStub>();
                        x.Field(y => y.FieldName).FieldName("Field Other Name");
                    });
            }
        }

        public interface IStubBase
        {
            string Id { get; set; }
        }


        public interface IFinalStub : IPartialStub1, IPartialStub2
        {
            string Children { get; set; }
            string FieldName { get; set; }
        }

        public interface IFinalSubClassStub : IPartialStub1, IPartialStub2
        {
            string Children { get; set; }
            string FieldName { get; set; }
        }

        public interface IPartialStub1 : IStubBase
        {
            string Qwerty { get; set; } 
        }

        public interface IPartialStub2 : IStubBase
        {
            string Parent { get; set; }
        }

        #endregion

    }
}

