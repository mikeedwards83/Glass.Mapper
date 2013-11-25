using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Tests.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver
{
    [TestFixture]
    public class ConfigurationOnDemandResolverTaskFixture
    {
        #region Method - Execute

        [Test]
        public void Execute_RunsLoader_TypeAddedToContext()
        {
            //Arrange

            var resolver = NSubstitute.Substitute.For<IDependencyResolver>();
            var context = Context.Create(resolver);
            var typeContext = NSubstitute.Substitute.For<AbstractTypeCreationContext>();
            typeContext.RequestedType = typeof (StubClass);
            var args = new ConfigurationResolverArgs(context, typeContext, typeContext.RequestedType, null);


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

            var resolver = NSubstitute.Substitute.For<IDependencyResolver>();
            var context = Context.Create(resolver);
            var typeContext = NSubstitute.Substitute.For<AbstractTypeCreationContext>();
            typeContext.RequestedType = typeof (StubClass);
            var args = new ConfigurationResolverArgs(context, typeContext, typeContext.RequestedType, null);
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
            var resolver = NSubstitute.Substitute.For<IDependencyResolver>();
            var context = Context.Create(resolver);
            var typeContext = NSubstitute.Substitute.For<AbstractTypeCreationContext>();
            
            var task = new ConfigurationOnDemandResolverTask<StubTypeConfiguration>();
            var argsList = new List<ConfigurationResolverArgs>();

            Action taskFunc = () =>
            {
                typeContext.RequestedType = typeof(StubClass);
                var args = new ConfigurationResolverArgs(context, typeContext, typeContext.RequestedType, null);
                
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
