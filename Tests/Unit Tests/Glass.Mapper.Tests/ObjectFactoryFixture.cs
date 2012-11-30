using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.TypeResolver;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests
{
    [TestFixture]
    public class ObjectFactoryFixture
    {
        #region Constructors

        [Test]
        public void Constructor_CalledWithParameters_NewInstanceCreated()
        {
            //Assign
            var glassConfig = Substitute.For<IGlassConfiguration>();
            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();

            var context = Context.Create(glassConfig);
            
            var objConstructTasks = new IObjectConstructionTask[] {};
            var typeResolverTasks = new ITypeResolverTask[] {};
            var configResolverTasks = new IConfigurationResolverTask[] {};

            //Act
            var factory = new ObjectFactory(
                context,
                typeResolverTasks,
                configResolverTasks,
                objConstructTasks
                );

            //Act 
            Assert.IsNotNull(factory);
            Assert.AreEqual(context, factory.Context);
            Assert.AreEqual(objConstructTasks, factory.ObjectConstructionTasks);
            Assert.AreEqual(typeResolverTasks, factory.TypeResolverTasks);
            Assert.AreEqual(configResolverTasks, factory.ConfigurationResolverTasks);
        }

        #endregion

        #region Method - InstantiateObject

        [Test]
        public void InstantiateObject_CallsEachPipelineInTurn_ReturnsConcreteObject()
        {
            //Assign
            var glassConfig = Substitute.For<IGlassConfiguration>();
            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();

            var context = Context.Create(glassConfig);
            
            IDataContext dataContext = Substitute.For<IDataContext>();
            var configuration = Substitute.For<AbstractTypeConfiguration>();
            var type = typeof(StubClass);
            var expected = new StubClass();


            ITypeResolverTask typeTask = Substitute.For<ITypeResolverTask>();
            typeTask
                .When(x => x.Execute(Arg.Any<TypeResolverArgs>()))
                .Do(info =>
                {
                    var args = info.Args();
                    var arg = args[0] as TypeResolverArgs;
                    arg.Result = type;
                });

            IConfigurationResolverTask configTask = Substitute.For<IConfigurationResolverTask>();
            configTask
                .When(x => x.Execute(Arg.Any<ConfigurationResolverArgs>()))
                .Do(info =>
                {
                    var args = info.Args();
                    var arg = args[0] as ConfigurationResolverArgs;
                    if (arg.Type == type)
                        arg.Result = configuration;
                });

            IObjectConstructionTask objectTask = Substitute.For<IObjectConstructionTask>();
            objectTask
                .When(x => x.Execute(Arg.Any<ObjectConstructionArgs>()))
                .Do(info =>
                {
                    var args = info.Args();
                    var arg = args[0] as ObjectConstructionArgs;
                    if (arg.Configuration == configuration)
                        arg.Result = expected;
                });

            var factory = new ObjectFactory(
               context,
               typeTask.MakeEnumerable(),
               configTask.MakeEnumerable(),
               objectTask.MakeEnumerable()
               );


            //Act
            var result = factory.InstantiateObject(dataContext);

            //Assert
            Assert.AreEqual(expected, result);

        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void InstantiateObject_TypeResolverDoesntReturnResult_ExceptionThrown()
        {
            //Assign
            var glassConfig = Substitute.For<IGlassConfiguration>();
            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();

            var context = Context.Create(glassConfig);

            IDataContext dataContext = Substitute.For<IDataContext>();
            var configuration = Substitute.For<AbstractTypeConfiguration>();
            var type = typeof(StubClass);
            var expected = new StubClass();


            ITypeResolverTask typeTask = Substitute.For<ITypeResolverTask>();
            typeTask
                .When(x => x.Execute(Arg.Any<TypeResolverArgs>()))
                .Do(info =>
                {
                    var args = info.Args();
                    var arg = args[0] as TypeResolverArgs;
                    arg.Result = null;
                });


            var factory = new ObjectFactory(
               context,
               typeTask.MakeEnumerable(),
               new IConfigurationResolverTask[]{}, 
               new IObjectConstructionTask[] { }
               );


            //Act
            var result = factory.InstantiateObject(dataContext);

            //Assert
            Assert.AreEqual(expected, result);

        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void InstantiateObject_ConfigurationResolverDoesntReturnResult_ExceptionThrown()
        {
            //Assign
            var glassConfig = Substitute.For<IGlassConfiguration>();
            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();

            var context = Context.Create(glassConfig);

            IDataContext dataContext = Substitute.For<IDataContext>();
            var configuration = Substitute.For<AbstractTypeConfiguration>();
            var type = typeof(StubClass);
            var expected = new StubClass();


            ITypeResolverTask typeTask = Substitute.For<ITypeResolverTask>();
            typeTask
                .When(x => x.Execute(Arg.Any<TypeResolverArgs>()))
                .Do(info =>
                {
                    var args = info.Args();
                    var arg = args[0] as TypeResolverArgs;
                    arg.Result = type;
                });

           

            IConfigurationResolverTask configTask = Substitute.For<IConfigurationResolverTask>();
            configTask
                .When(x => x.Execute(Arg.Any<ConfigurationResolverArgs>()))
                .Do(info =>
                {
                    var args = info.Args();
                    var arg = args[0] as ConfigurationResolverArgs;
                    if (arg.Type == type)
                        arg.Result = null;
                });

            

            var factory = new ObjectFactory(
                context,
                typeTask.MakeEnumerable(),
                configTask.MakeEnumerable(),
                new IObjectConstructionTask[]{}
                );


            //Act
            var result = factory.InstantiateObject(dataContext);

            //Assert
            Assert.AreEqual(expected, result);

        }

        [Test]
        public void InstantiateObject_ObjectCreationReturnsNull_NullReturned()
        {
            //Assign
            var glassConfig = Substitute.For<IGlassConfiguration>();
            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();

            var context = Context.Create(glassConfig);

            IDataContext dataContext = Substitute.For<IDataContext>();
            var configuration = Substitute.For<AbstractTypeConfiguration>();
            var type = typeof(StubClass);
            var expected = (StubClass)null;

            ITypeResolverTask typeTask = Substitute.For<ITypeResolverTask>();
            typeTask
                .When(x => x.Execute(Arg.Any<TypeResolverArgs>()))
                .Do(info =>
                {
                    var args = info.Args();
                    var arg = args[0] as TypeResolverArgs;
                    arg.Result = type;
                });

            IConfigurationResolverTask configTask = Substitute.For<IConfigurationResolverTask>();
            configTask
                .When(x => x.Execute(Arg.Any<ConfigurationResolverArgs>()))
                .Do(info =>
                {
                    var args = info.Args();
                    var arg = args[0] as ConfigurationResolverArgs;
                    if (arg.Type == type)
                        arg.Result = configuration;
                });

            IObjectConstructionTask objectTask = Substitute.For<IObjectConstructionTask>();
            objectTask
                .When(x => x.Execute(Arg.Any<ObjectConstructionArgs>()))
                .Do(info =>
                {
                    var args = info.Args();
                    var arg = args[0] as ObjectConstructionArgs;
                    if (arg.Configuration == configuration)
                        arg.Result = expected;
                });


            var factory = new ObjectFactory(
                context,
                typeTask.MakeEnumerable(),
                configTask.MakeEnumerable(),
                objectTask.MakeEnumerable()
                );


            //Act
            var result = factory.InstantiateObject(dataContext);

            //Assert
            Assert.AreEqual(expected, result);

        }

        #endregion

        #region Stubs

        public class StubClass
        {
        }

        


        #endregion
    }
}
