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
using Castle.MicroKernel.Registration;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Umb.CastleWindsor.Pipelines.ObjectConstruction;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Umb.CastleWindsor.Tests.Pipelines.ObjectConstruction
{
    [TestFixture]
    public class WindsorConstructionFixture
    {
        [Test]
        public void Execute_RequestInstanceOfClass_ReturnsInstance()
        {
            //Assign
            var task = new WindsorConstruction();
            
            var context = Context.Create(DependencyResolver.CreateStandardResolver());
            var typeConfig = Substitute.For<AbstractTypeConfiguration>();
            typeConfig.Type = typeof (StubClass);

            var typeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            var service = Substitute.For<AbstractService>();

            var args = new ObjectConstructionArgs(context, typeCreationContext,  typeConfig , service);

            Assert.IsNull(args.Result);

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is StubClass);

        }

        [Test]
        public void Execute_ResultAlreadySet_NoInstanceCreated()
        {
            //Assign
            var task = new WindsorConstruction();


            var context = Context.Create(DependencyResolver.CreateStandardResolver());
            var typeConfig = Substitute.For<AbstractTypeConfiguration>();
            typeConfig.Type = typeof(StubClass);

            var args = new ObjectConstructionArgs(context, null, typeConfig , null);
            var result = new StubClass2();
            args.Result = result;

            Assert.IsNotNull(args.Result);

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is StubClass2);
            Assert.AreEqual(result, args.Result);

        }

        [Test]
        public void Execute_RequestInstanceOfInterface_ReturnsNullInterfaceNotSupported()
        {
            //Assign
            var task = new WindsorConstruction();


            var context = Context.Create(DependencyResolver.CreateStandardResolver());
            var typeConfig = Substitute.For<AbstractTypeConfiguration>();
            typeConfig.Type = typeof(StubInterface);

            var typeCreationContext = Substitute.For<AbstractTypeCreationContext>();

            var args = new ObjectConstructionArgs(context, typeCreationContext,  typeConfig , null);



            Assert.IsNull(args.Result);

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNull(args.Result);

        }

        [Test]
        public void Execute_RequestInstanceOfClassWithService_ReturnsInstanceWithService()
        {
            //Assign
            var task = new WindsorConstruction();

            var resolver = DependencyResolver.CreateStandardResolver() as DependencyResolver;
            var context = Context.Create(resolver);

            resolver.Container.Register(
                Component.For<StubServiceInterface>().ImplementedBy<StubService>().LifestyleTransient()
                );
            
            var typeConfig = Substitute.For<AbstractTypeConfiguration>();
            typeConfig.Type = typeof(StubClassWithService);

            var typeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            var service = Substitute.For<AbstractService>();


            var args = new ObjectConstructionArgs(context, typeCreationContext, typeConfig , service);

            Assert.IsNull(args.Result);

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is StubClassWithService);
            
            var stub = args.Result as StubClassWithService;

            Assert.IsNotNull(stub.Service);
            Assert.IsTrue(stub.Service is StubService);

        }

        [Test]
        public void Execute_RequestInstanceOfClassWithParameters_NoInstanceReturnedDoesntHandle()
        {
            //Assign
            var task = new WindsorConstruction();


            var context = Context.Create(DependencyResolver.CreateStandardResolver());
            var typeConfig = Substitute.For<AbstractTypeConfiguration>();
            typeConfig.Type = typeof(StubClassWithParameters);

            string param1 = "test param1";
            int param2 = 450;
            double param3 = 489;
            string param4 = "param4 test";

            var typeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            typeCreationContext.ConstructorParameters = new object[]{param1, param2, param3, param4};


            var args = new ObjectConstructionArgs(context, typeCreationContext,  typeConfig , null);

            Assert.IsNull(args.Result);

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNull(args.Result);


        }

        #region Stubs

        public class StubClass
        {
            
        }

        public class StubClass2
        {
            
        }
        public interface StubInterface
        {
            
        }
        public interface StubServiceInterface
        {
            
        }
        public class StubService: StubServiceInterface
        {
            
        }
        public class StubClassWithService 
        {
            public StubServiceInterface Service { get; set; }

            public StubClassWithService(StubServiceInterface service)
            {
                Service = service;
            }
        }

        public class StubClassWithParameters
        {
            public string Param1 { get; set; }
            public int Param2 { get; set; }
            public double Param3 { get; set; }
            public string Param4 { get; set; }

            public StubClassWithParameters(
                string param1,
                int param2,
                double param3,
                string param4
                )
            {
                Param1 = param1;
                Param2 = param2;
                Param3 = param3;
                Param4 = param4;
            }
        }

        #endregion
    }
}

