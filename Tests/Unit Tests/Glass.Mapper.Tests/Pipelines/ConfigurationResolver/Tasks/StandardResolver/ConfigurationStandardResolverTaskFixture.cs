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



