using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ConfigurationResolver.Tasks.StandardResolver
{
    [TestFixture]
    public class ConfigurationStandardResolverTaskFixture
    {
        #region Method - Execute

        [Test]
        public void Execute_FindsFirstTypeMatchedInConfigurationsList_ReturnsConfiguration()
        {
            //Assign

            var glassConfig = Substitute.For<IGlassConfiguration>();

            var type = typeof (StubClass);
            
            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;
            
            var loader = Substitute.For<IConfigurationLoader>();
            loader.Load().Returns(new [] {configuration});

            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();
            Context.ResolverFactory.GetResolver().Returns(Substitute.For<IDependencyResolver>());
            var context = Context.Create(glassConfig);
            
            context.Load(loader);

            var args = new ConfigurationResolverArgs(context, null, type);

            var task = new ConfigurationStandardResolverTask();

            //Act
            task.Execute(args);

            //Assert
            Assert.AreEqual(configuration, args.Result);


        }

        #endregion

        #region Stubs

        public class StubClass
        {
            
        }

        #endregion
    }
}
