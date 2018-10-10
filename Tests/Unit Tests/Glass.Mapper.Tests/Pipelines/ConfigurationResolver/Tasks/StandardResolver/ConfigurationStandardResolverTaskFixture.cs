
using System;
using System.Linq;
using Glass.Mapper.Configuration;
using Glass.Mapper.IoC;
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

            Substitute.For<IGlassConfiguration>();

            var type = typeof (StubClass);
            
            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;
            
            var loader = Substitute.For<IConfigurationLoader>();
            loader.Load().Returns(new [] {configuration});

            var context = Context.Create(Substitute.For<IDependencyResolver>());
            context.Config = new Config();

            context.Load(loader);

            var args = new ConfigurationResolverArgs(context, new StubAbstractTypeCreationContext()
                {
                    Options =  new TestGetOptions { Type = type}
                }, null);

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

        public class StubAbstractTypeCreationContext:AbstractTypeCreationContext
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
        #endregion
    }
}




