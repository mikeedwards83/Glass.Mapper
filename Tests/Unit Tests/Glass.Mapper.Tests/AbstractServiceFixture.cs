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
        #region Method - InstantiateObject

        [Test]
        public void InstantiateObject_CallsEachPipelineInTurn_ReturnsConcreteObject()
        {
            //Assign
            Context context = Context.Load();
            IDataContext dataContext = Substitute.For<IDataContext>();
            var configuration = Substitute.For<AbstractTypeConfiguration>();
            var type = typeof (StubClass);
            var expected = new StubClass();


            ITypeResolverTask typeTask = Substitute.For<ITypeResolverTask>();
            typeTask
                .When(x=>x.Execute(Arg.Any<TypeResolverArgs>()))
                .Do(info =>
                        {
                            var args = info.Args();
                            var arg = args[0] as TypeResolverArgs;
                            arg.Result = type;
                        });

            context.TypeResolverTasks.Add(typeTask);

            IConfigurationResolverTask configTask = Substitute.For<IConfigurationResolverTask>();
            configTask
                .When(x=>x.Execute(Arg.Any<ConfigurationResolverArgs>()))
                .Do(info =>
                        {
                            var args = info.Args();
                            var arg = args[0] as ConfigurationResolverArgs;
                            if(arg.Type == type)
                                arg.Result = configuration;
                        });

            context.ConfigurationResolverTasks.Add(configTask);

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

            context.ObjectConstructionTasks.Add(objectTask);


            //Act
            var result = AbstractService<IDataContext>.InstantiateObject(context, dataContext);

            //Assert
            Assert.AreEqual(expected, result);

        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void InstantiateObject_TypeResolverDoesntReturnResult_ExceptionThrown()
        {
            //Assign
            Context context = Context.Load();
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

            context.TypeResolverTasks.Add(typeTask);
            
            //Act
            var result = AbstractService<IDataContext>.InstantiateObject(context, dataContext);

            //Assert
            Assert.AreEqual(expected, result);

        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void InstantiateObject_ConfigurationResolverDoesntReturnResult_ExceptionThrown()
        {
            //Assign
            Context context = Context.Load();
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

            context.TypeResolverTasks.Add(typeTask);

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

            context.ConfigurationResolverTasks.Add(configTask);

            //Act
            var result = AbstractService<IDataContext>.InstantiateObject(context, dataContext);

            //Assert
            Assert.AreEqual(expected, result);

        }

        [Test]
        public void InstantiateObject_ObjectCreationReturnsNull_NullReturned()
        {
            //Assign
            Context context = Context.Load();
            IDataContext dataContext = Substitute.For<IDataContext>();
            var configuration = Substitute.For<AbstractTypeConfiguration>();
            var type = typeof(StubClass);
            var expected = (StubClass) null;


            ITypeResolverTask typeTask = Substitute.For<ITypeResolverTask>();
            typeTask
                .When(x => x.Execute(Arg.Any<TypeResolverArgs>()))
                .Do(info =>
                {
                    var args = info.Args();
                    var arg = args[0] as TypeResolverArgs;
                    arg.Result = type;
                });

            context.TypeResolverTasks.Add(typeTask);

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

            context.ConfigurationResolverTasks.Add(configTask);

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

            context.ObjectConstructionTasks.Add(objectTask);


            //Act
            var result = AbstractService<IDataContext>.InstantiateObject(context, dataContext);

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
