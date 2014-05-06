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
using Glass.Mapper.Pipelines.ObjectConstruction;
using NUnit.Framework;
using NSubstitute;
using Glass.Mapper.Pipelines.ConfigurationResolver;

namespace Glass.Mapper.Tests
{
    [TestFixture]
    public class AbstractServiceFixture
    {
        #region Constructors

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void Contructor_ContextIsNull_ThrowsException()
        {
            //Assign

            //Act
            var service = new StubAbstractService(null);

            //Assert
        }

        #endregion

        #region Method - InstantiateObject

        [Test]
        public void InstantiageObject_AllRunnersSetup_ObjectReturned()
        {
            //Assign

            var resolver = Substitute.For<IDependencyResolver>();

            var context = Context.Create(resolver);

            var configTask = Substitute.For<IConfigurationResolverTask>();
            var objTask = Substitute.For<IObjectConstructionTask>();

            resolver.ResolveAll<IConfigurationResolverTask>().Returns(new[] { configTask });
            resolver.ResolveAll<IObjectConstructionTask>().Returns(new[] { objTask });

            configTask.When(x => x.Execute(Arg.Any<ConfigurationResolverArgs>()))
                .Do(x => x.Arg<ConfigurationResolverArgs>().Result = Substitute.For<AbstractTypeConfiguration>());

            var expected = new object();

            objTask.When(x => x.Execute(Arg.Any<ObjectConstructionArgs>()))
                .Do(x => x.Arg<ObjectConstructionArgs>().Result = expected);

            var service = new StubAbstractService(context);

            var typeContext = Substitute.For<AbstractTypeCreationContext>();
            typeContext.RequestedType = typeof(object);

            //Act
            var result = service.InstantiateObject(typeContext);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region Stub 

        public class StubClass
        {

        }

        public class StubAbstractService : AbstractService
        {
            public StubAbstractService(Context context) : base(context)
            {
            }

            public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext creationContext, object obj)
            {
                throw new NotImplementedException();
            }

            public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext)
            {
                throw new NotImplementedException();
            }
        }

        public class StubAbstractTypeCreationContext : AbstractTypeCreationContext
        {

        }

        public class StubAbstractDataMappingContext : AbstractDataMappingContext
        {
            public StubAbstractDataMappingContext(object obj) : base(obj)
            {

            }
        }

        #endregion
    }
}




