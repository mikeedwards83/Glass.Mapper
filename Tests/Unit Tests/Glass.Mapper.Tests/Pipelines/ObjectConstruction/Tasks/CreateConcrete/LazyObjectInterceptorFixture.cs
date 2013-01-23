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
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.TypeResolver.Tasks.StandardResolver;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{
    [TestFixture]
    public class LazyObjectInterceptorFixture
    {
        #region Method - Intercept

        [Test]
        public void Intercept_CreatesObjectLazily_CallsInvokeMethod()
        {
            //Assign
            var typeContext = Substitute.For<AbstractTypeCreationContext>();
            var config = Substitute.For<AbstractTypeConfiguration>();
            var service = Substitute.For<IAbstractService>();

            var args = new ObjectConstructionArgs(
                null,
                typeContext, 
                config,
                service
                );

            var invocation = Substitute.For<IInvocation>();
            invocation.Method.Returns(typeof (StubClass).GetMethod("CalledMe"));
            service.InstantiateObject(typeContext).Returns(new StubClass());

            var interceptor = new LazyObjectInterceptor(args);

            //Act
            interceptor.Intercept(invocation);

            //Assert
            Assert.IsTrue((bool)invocation.ReturnValue);


        }

        #endregion

        #region Stubs

        public class StubClass
        {

            public bool CalledMe()
            {
                return true;
            }

        }

        #endregion
    }
}



