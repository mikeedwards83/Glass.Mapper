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
using Glass.Mapper.Pipelines.DataMapperResolver;
using NUnit.Framework;
using Glass.Mapper.Configuration;
using NSubstitute;

namespace Glass.Mapper.Tests
{
    [TestFixture]
    public class ContextFixture
    {
        [TearDown]
        public void TearDown()
        {
            Context.Clear();
        }

        [SetUp]
        public void Setup()
        {
            
         
        }

        #region Create

        [Test]
        public void Create_CreateContext_CanGetContextFromContextDictionary()
        {
            //Assign
            string contextName = "testContext";
            bool isDefault = false;
            Context.Clear();

            //Act
            Context.Create(Substitute.For<IDependencyResolver>(), contextName, isDefault);

            //Assert
            Assert.IsTrue(Context.Contexts.ContainsKey(contextName));
            Assert.IsNotNull(Context.Contexts[contextName]);
            Assert.IsNull(Context.Default);
        }

        [Test]
        public void Create_LoadContextAsDefault_CanGetContextFromContextDictionary()
        {
            //Assign
            string contextName = "testContext";
            bool isDefault = true;

            //Act
            Context.Create(Substitute.For<IDependencyResolver>(), contextName, isDefault);

            //Assert
            Assert.IsTrue(Context.Contexts.ContainsKey(contextName));
            Assert.IsNotNull(Context.Contexts[contextName]);
            Assert.IsNotNull(Context.Default);
            Assert.AreEqual(Context.Contexts[contextName], Context.Default);
        }

        [Test]
        public void Create_LoadContextAsDefault_CanGetContextFromContextDictionaryUsingDefault()
        {
            //Assign

            //Act
            Context.Create(Substitute.For<IDependencyResolver>());
            
            //Assert
            Assert.IsNotNull(Context.Default);
            Assert.AreEqual(Context.Contexts[Context.DefaultContextName], Context.Default);
        }
        
        #endregion

        #region Load

        [Test]
        public void Load_LoadContextWithTypeConfigs_CanGetTypeConfigsFromContext()
        {
            //Assign
            var loader1 = Substitute.For<IConfigurationLoader>();
            var config1 = Substitute.For<AbstractTypeConfiguration>();
            config1.Type = typeof (StubClass1);
            loader1.Load().Returns(new[]{config1});

            var loader2 = Substitute.For<IConfigurationLoader>();
            var config2 = Substitute.For<AbstractTypeConfiguration>();
            config2.Type = typeof(StubClass2);
            loader2.Load().Returns(new[] { config2 });

            //Act
            var context = Context.Create(Substitute.For<IDependencyResolver>());
            context.Load(loader1, loader2);

            //Assert
            Assert.IsNotNull(Context.Default);
            Assert.AreEqual(Context.Contexts[Context.DefaultContextName], Context.Default);
            Assert.AreEqual(config1, Context.Default.TypeConfigurations[config1.Type]);
            Assert.AreEqual(config2, Context.Default.TypeConfigurations[config2.Type]);
        }

        

        #endregion

        #region Indexers

        [Test]
        public void Indexer_UseTypeIndexerWithValidConfig_ReturnsTypeConfiguration()
        {
            //Assign
            var loader1 = Substitute.For<IConfigurationLoader>();
            var config1 = Substitute.For<AbstractTypeConfiguration>();
            config1.Type = typeof(StubClass1);
            loader1.Load().Returns(new[] { config1 });



            //Act
            var context = Context.Create(Substitute.For<IDependencyResolver>());
            context.Load(loader1);

            //Assert
            Assert.IsNotNull(Context.Default);
            Assert.AreEqual(Context.Contexts[Context.DefaultContextName], Context.Default);
            Assert.AreEqual(config1, Context.Default[config1.Type]);
        }

        [Test] public void Indexer_UseTypeIndexerWithInvalidConfig_ReturnsNull()
        {
            //Assign
            var loader1 = Substitute.For<IConfigurationLoader>();

            //Act
            var context = Context.Create(Substitute.For<IDependencyResolver>());
            context.Load(loader1);

            //Assert
            Assert.IsNotNull(Context.Default);
            Assert.AreEqual(Context.Contexts[Context.DefaultContextName], Context.Default);
            Assert.IsNull( Context.Default[typeof(StubClass1)]);
        }

        #endregion

        #region GetTypeConfiguration

        [Test]
        public void GetTypeConfiguration_BaseTypeNullForInterface_AutoLoadsConfig()
        {
            //Arrange
            string contextName = "testContext";
            bool isDefault = true;
            var type = typeof (IStubInterface1);
            Context.Create(Substitute.For<IDependencyResolver>(), contextName, isDefault);
            var context = Context.Contexts[contextName];

            //Act
            var config = context.GetTypeConfiguration<StubAbstractTypeConfiguration>(type);

            //Assert
            Assert.IsNotNull(config);

        }
        #endregion

        #region Stubs

        public interface IStubInterface1
        {
            
        }

        public class StubClass1
        {

        }

        public class StubClass2
        {

        }

        public class StubAbstractDataMapper : AbstractDataMapper
        {
            public Func<AbstractPropertyConfiguration, bool> CanHandleFunction { get; set; }
            public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
            {
                return CanHandleFunction(configuration);
            }



            public override void MapToCms(AbstractDataMappingContext mappingContext)
            {
                throw new NotImplementedException();
            }

            public override object MapToProperty(AbstractDataMappingContext mappingContext)
            {
                throw new NotImplementedException();
            }

            public override void Setup(DataMapperResolverArgs args)
            {
                throw new NotImplementedException();
            }
        }

        public class StubAbstractTypeConfiguration : AbstractTypeConfiguration
        {
            
        }

        #endregion
    }
}




