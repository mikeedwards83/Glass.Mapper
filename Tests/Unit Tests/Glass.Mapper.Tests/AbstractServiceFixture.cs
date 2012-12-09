using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;
using NUnit.Framework;
using NSubstitute;
using Glass.Mapper.Pipelines.TypeResolver;
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
            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();
            var resolver = Substitute.For<IDependencyResolver>();
            
            Context.ResolverFactory.GetResolver().Returns(resolver);
            var context = Context.Create(Substitute.For<IGlassConfiguration>());

            var typeTask = Substitute.For<ITypeResolverTask>();
            var configTask = Substitute.For<IConfigurationResolverTask>();
            var objTask = Substitute.For<IObjectConstructionTask>();

            resolver.ResolveAll<ITypeResolverTask>().Returns(new[] { typeTask });
            resolver.ResolveAll<IConfigurationResolverTask>().Returns(new[] { configTask });
            resolver.ResolveAll<IObjectConstructionTask>().Returns(new[] { objTask });

            typeTask.When(x=>x.Execute(Arg.Any<TypeResolverArgs>()))
                .Do(x=>x.Arg<TypeResolverArgs>().Result = typeof(StubClass));

            configTask.When(x => x.Execute(Arg.Any<ConfigurationResolverArgs>()))
                .Do(x => x.Arg<ConfigurationResolverArgs>().Result = Substitute.For<AbstractTypeConfiguration>());

            var expected = new object();

            objTask.When(x => x.Execute(Arg.Any<ObjectConstructionArgs>()))
                .Do(x => x.Arg<ObjectConstructionArgs>().Result = expected);

            var service = new StubAbstractService(context);

            //Act
            var result = service.InstantiateObject(Substitute.For<AbstractTypeCreationContext>());

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region Stub 

        public class StubClass
        {

        }

        public class StubAbstractService : AbstractService<StubAbstractDataMappingContext>
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
