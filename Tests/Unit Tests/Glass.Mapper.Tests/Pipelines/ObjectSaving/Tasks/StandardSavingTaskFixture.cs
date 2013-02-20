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
using Glass.Mapper.Pipelines;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectSaving.Tasks
{
    [TestFixture]
    public class StandardSavingTaskFixture
    {

        #region Method - Execute


        [Test]
        [ExpectedException(typeof(PipelineException))]
        public void Execute_ConfigurationNotSet_ThrowsException()
        {
            //Assign
            var target = new object();
            var savingContext = Substitute.For<AbstractTypeSavingContext>();
            var service = Substitute.For<IAbstractService>();
            var args = new ObjectSavingArgs(null, target, savingContext, service);
            var task = new StandardSavingTask();


            //Act
            task.Execute(args);

            //Assert

        }

        [Test]
        public void Execute_RunnerCorrectlyConfigured_CallsEachDataMapper()
        {
            //Assign
            var target = new Stub();
            var savingContext = Substitute.For<AbstractTypeSavingContext>();
            var service = Substitute.For<IAbstractService>();
            var args = new ObjectSavingArgs(null, target, savingContext, service);
            var task = new StandardSavingTask();

            var dataContext = Substitute.For<AbstractDataMappingContext>(target);
            service.CreateDataMappingContext(savingContext).Returns(dataContext);

            var property1 = Substitute.For<AbstractPropertyConfiguration>();
            var config1 = Substitute.For<AbstractPropertyConfiguration>();
            var mapper1 = new StubMapper();

            property1.Mapper = mapper1;
            config1.PropertyInfo = typeof(Stub).GetProperty("Property");
            mapper1.Setup(new DataMapperResolverArgs(null,config1));
           
            var property2 = Substitute.For<AbstractPropertyConfiguration>();
            var config2 = Substitute.For<AbstractPropertyConfiguration>();
            var mapper2 = new StubMapper();

            property2.Mapper = mapper2;
            config2.PropertyInfo = typeof(Stub).GetProperty("Property");
            mapper2.Setup(new DataMapperResolverArgs(null,config2));
          
            savingContext.Config = Substitute.For<AbstractTypeConfiguration>();
            savingContext.Config.AddProperty(property1);
            savingContext.Config.AddProperty(property2);

            //Act
            task.Execute(args);

            //Assert
            Assert.IsTrue(mapper1.MapCalled);
            Assert.IsTrue(mapper2.MapCalled);
        }

        #endregion

        #region Stubs

        public class Stub
        {
            public string Property { get; set; }
        }

        public class StubMapper : AbstractDataMapper
        {
           
            public bool MapCalled { get; set; }
            public override void MapToCms(AbstractDataMappingContext mappingContext)
            {
                MapCalled = true;
            }

            public override object MapToProperty(AbstractDataMappingContext mappingContext)
            {
                throw new NotImplementedException();
            }

            public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}



