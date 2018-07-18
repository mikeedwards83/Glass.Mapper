using System;
using Glass.Mapper.Configuration;
using Glass.Mapper.Diagnostics;
using Glass.Mapper.IoC;
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
        public void Contructor_ContextIsNull_ThrowsException()
        {
            //Assign

            //Act
            Assert.Throws<NullReferenceException>(() =>
            {
                var service = new StubAbstractService(null);
            });

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

            var configTask = Substitute.For<AbstractConfigurationResolverTask>();
            var objTask = Substitute.For<AbstractObjectConstructionTask>();

            resolver.ConfigurationResolverFactory.GetItems().Returns(new[] { configTask });
            resolver.ObjectConstructionFactory.GetItems().Returns(new []{objTask});

            configTask.When(x => x.Execute(Arg.Any<ConfigurationResolverArgs>()))
                .Do(x => x.Arg<ConfigurationResolverArgs>().Result = Substitute.For<AbstractTypeConfiguration>());

            var expected = new object();

            objTask.When(x => x.Execute(Arg.Any<ObjectConstructionArgs>()))
                .Do(x => x.Arg<ObjectConstructionArgs>().Result = expected);

            var service = new StubAbstractService(context);

            var typeContext = new TestTypeCreationContext();
            typeContext.Options = new TestGetOptions()
            {
                Type = typeof(object)
            };

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

          
        }

        public class StubAbstractTypeCreationContext : AbstractTypeCreationContext
        {
            public override bool CacheEnabled
            {
                get { return true; }
            }

            public override AbstractDataMappingContext CreateDataMappingContext(object obj)
            {
                throw new NotImplementedException();
            }
        }

        public class StubAbstractDataMappingContext : AbstractDataMappingContext
        {
            public StubAbstractDataMappingContext(object obj, GetOptions options) : base(obj, options)
            {

            }
        }

        #endregion
    }
}




