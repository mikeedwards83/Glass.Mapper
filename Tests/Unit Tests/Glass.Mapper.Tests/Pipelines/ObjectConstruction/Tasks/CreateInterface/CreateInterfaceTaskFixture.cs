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
using Glass.Mapper.Diagnostics;
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateInterface
{
    [TestFixture]
    public class CreateInterfaceTaskFixture
    {
        private CreateInterfaceTask _task;

        [SetUp]
        public void Setup()
        {
            _task = new CreateInterfaceTask();
        }

        #region Method - Execute

        [Test]
        public void Execute_ConcreteClass_ObjectNotCreated()
        {
            //Assign
            Type type = typeof(StubClass);
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(Substitute.For<IDependencyResolver>());

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType = type;

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service, new ModelCounter());

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNull(args.Result);

        }

        [Test]
        public void Execute_ProxyInterface_ProxyGetsCreated()
        {
            //Assign
            Type type = typeof(IStubInterface);
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(Substitute.For<IDependencyResolver>());

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType = typeof (IStubInterface);

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service, new ModelCounter());

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is IStubInterface);
            Assert.IsFalse(args.Result.GetType() == typeof(IStubInterface));
        }

        [Test]
        public void Execute_TwoClassesWithTheSameName_ProxyGetsCreated()
        {
            //Assign
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(Substitute.For<IDependencyResolver>());

            AbstractTypeCreationContext abstractTypeCreationContext1 = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext1.RequestedType = typeof(NS1.ProxyTest1);

            var configuration1 = Substitute.For<AbstractTypeConfiguration>();
            configuration1.Type = typeof(NS1.ProxyTest1);

            ObjectConstructionArgs args1 = new ObjectConstructionArgs(context, abstractTypeCreationContext1, configuration1, service, new ModelCounter());

            AbstractTypeCreationContext abstractTypeCreationContext2 = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext2.RequestedType = typeof(NS2.ProxyTest1);

            var configuration2 = Substitute.For<AbstractTypeConfiguration>();
            configuration2.Type = typeof(NS2.ProxyTest1); ;

            ObjectConstructionArgs args2 = new ObjectConstructionArgs(context, abstractTypeCreationContext2, configuration2, service, new ModelCounter());

            //Act
            _task.Execute(args1);
            _task.Execute(args2);

            //Assert
            Assert.IsNotNull(args1.Result);
            Assert.IsTrue(args1.Result is NS1.ProxyTest1);
            Assert.IsFalse(args1.Result.GetType() == typeof(NS1.ProxyTest1));

            Assert.IsNotNull(args2.Result);
            Assert.IsTrue(args2.Result is NS2.ProxyTest1);
            Assert.IsFalse(args2.Result.GetType() == typeof(NS2.ProxyTest1));
        }

        
        [Test]
        public void Execute_ResultAlreadySet_DoesNoWork()
        {
            //Assign
            Type type = typeof(IStubInterface);
            var resolver = Substitute.For<IDependencyResolver>();
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(resolver);

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType= typeof(IStubInterface);

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service, new ModelCounter());
            args.Result = string.Empty;

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is string);
        }



        #endregion

        #region Stubs

        public class StubClass
        {

        }

        public interface IStubInterface
        {

        }

        #endregion
    }

    namespace NS1
    {
        public interface ProxyTest1 { }
    }
    namespace NS2
    {
        public interface ProxyTest1 { }
    }
}




