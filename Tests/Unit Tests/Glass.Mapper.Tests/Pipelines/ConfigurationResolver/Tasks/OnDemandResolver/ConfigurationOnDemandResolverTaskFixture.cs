using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glass.Mapper.Configuration;
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver
{
    [TestFixture]
    public class ConfigurationOnDemandResolverTaskFixture
    {
        #region Method - Execute

        [Test]
        public void Execute_OnDemandDisabled_ThrowsException()
        {
            //Arrange

            var resolver = Substitute.For<IDependencyResolver>();
            var context = Context.Create(resolver);
            context.Config = new Config();
            context.Config.OnDemandMappingEnabled = false;

            var typeContext = new TestTypeCreationContext();
            typeContext.Options = new TestGetOptions()
            {
                Type = typeof(StubClass)
            };

            var args = new ConfigurationResolverArgs(context, typeContext, null);


            var task = new ConfigurationOnDemandResolverTask<StubTypeConfiguration>();

            Assert.AreEqual(0, context.TypeConfigurations.Count);

            //Act
            Assert.Throws<MapperException>(
                () => { task.Execute(args); },
                Constants.Errors.OnDemandDisabled);
        }

        [Test]
        public void Execute_RunsLoader_TypeAddedToContext()
        {
            //Arrange

            var resolver = Substitute.For<IDependencyResolver>();
            var context = Context.Create(resolver);
            context.Config = new Config();
            var typeContext = new TestTypeCreationContext();
            typeContext.Options = new TestGetOptions()
            {
                Type = typeof(StubClass)
            };

            var args = new ConfigurationResolverArgs(context, typeContext,null);


            var task = new ConfigurationOnDemandResolverTask<StubTypeConfiguration>();

            Assert.AreEqual(0, context.TypeConfigurations.Count);

            //Act
            task.Execute(args);

            //Assert
            Assert.AreEqual(1, context.TypeConfigurations.Count);
            Assert.IsNotNull(context.TypeConfigurations[typeof(StubClass)]);
            Assert.AreEqual(typeof(StubClass), args.Result.Type);
        }

        [Test]
        public void Execute_ResultAlreadySet_DoesNotRunLoader()
        {
            //Arrange

            var resolver = Substitute.For<IDependencyResolver>();
            var context = Context.Create(resolver);
            var typeContext = new TestTypeCreationContext();
            typeContext.Options = new TestGetOptions()
            {
                Type = typeof(StubClass)
            };


            var args = new ConfigurationResolverArgs(context, typeContext, null);
            args.Result = new StubTypeConfiguration();

            var task = new ConfigurationOnDemandResolverTask<StubTypeConfiguration>();

            Assert.AreEqual(0, context.TypeConfigurations.Count);

            //Act
            task.Execute(args);

            //Assert
            Assert.AreEqual(0, context.TypeConfigurations.Count);
        }

        [Test]
        public void Execute_MultipleRequestsUnloadedType_AddsOnlyOnceToContext()
        {
            //Arrange
            var resolver = Substitute.For<IDependencyResolver>();
            var context = Context.Create(resolver);
            var typeContext = new TestTypeCreationContext();
            context.Config = new Config();




            var task = new ConfigurationOnDemandResolverTask<StubTypeConfiguration>();
            var argsList = new List<ConfigurationResolverArgs>();

            Action taskFunc = () =>
            {
                typeContext.Options = new TestGetOptions()
                {
                    Type = typeof(StubClass)
                };
                var args = new ConfigurationResolverArgs(context, typeContext, null);
                
                argsList.Add(args);
                
                task.Execute(args);
            };
    

            Assert.AreEqual(0, context.TypeConfigurations.Count);

            //Act
            var tasks = new []
            {
                new Task(taskFunc),
                new Task(taskFunc),
                new Task(taskFunc),
                new Task(taskFunc),
                new Task(taskFunc),
                new Task(taskFunc),
            };
            Parallel.ForEach(tasks, x => x.Start());
            Task.WaitAll(tasks);

            //Assert
            Assert.AreEqual(1, context.TypeConfigurations.Count);
            Assert.IsTrue(argsList.All(x => x.Result.Type == typeof(StubClass)));
        }
        #endregion

        #region Stubs

        public class StubClass { }

        public class StubTypeConfiguration : AbstractTypeConfiguration
        {
        }
        #endregion
    }
}
