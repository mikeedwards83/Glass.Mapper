using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateLazy;
using Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateLazy
{
    [TestFixture]
    public class CreateLazyTaskFixture
    {

        private CreateLazyTask _task;

        [SetUp]
        public void Setup()
        {
            _task = new CreateLazyTask();
        }

        [Test]
        public void Execute_LazyType_LazyTypeCreated()
        {
            //Assign

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType = typeof(CreateConcreteTaskFixture.StubClass);
            abstractTypeCreationContext.IsLazy = true;
            abstractTypeCreationContext.Service = new StubAbstractService(null, null);

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = typeof(StubClass);;

            ObjectConstructionArgs args = new ObjectConstructionArgs(null, abstractTypeCreationContext, configuration);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsTrue(args.IsAborted);
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is StubClass);
            Assert.IsFalse(args.Result.GetType() == typeof(StubClass));
        }

        #region Stubs

        public class StubClass
        {
            
        }

        public class StubAbstractService : AbstractService
        {
            public StubAbstractService(AbstractObjectFactory factory, Context context) : base(factory, context)
            {
            }
        }
        #endregion

    }
}
