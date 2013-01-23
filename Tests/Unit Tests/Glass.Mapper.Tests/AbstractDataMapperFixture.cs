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
using Glass.Mapper.Pipelines.DataMapperResolver;
using NUnit.Framework;
using NSubstitute;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Tests
{
    [TestFixture]
    public class AbstractDataMapperFixture
    {

        [SetUp]
        public void Setup()
        {


        }

        #region Method - MapCmsToProperty

        [Test]
        public void MapCmsToProperty_ValueFromCms_WritesToProperty()
        {
            //Assign
            var obj = new StubClass();
            var config = Substitute.For<AbstractPropertyConfiguration>();
            var  dataMapper = new StubMapper();

            AbstractDataMappingContext context = Substitute.For<AbstractDataMappingContext>(obj);
           dataMapper.Setup(new DataMapperResolverArgs(null, config));
            config.PropertyInfo = typeof(StubClass).GetProperties().First(x => x.Name == "AProperty");

            //Act
            dataMapper.MapCmsToProperty(context);

            //Assert
            Assert.AreEqual("Hello world", obj.AProperty);
            Assert.AreEqual(context, dataMapper.MappingContext);

        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public string AProperty { get; set; }
        }

        public class StubMapper : AbstractDataMapper
        {

            public AbstractDataMappingContext MappingContext { get; set; }
            public override void MapToCms(AbstractDataMappingContext mappingContext)
            {
                throw new NotImplementedException();

            }

            public override object MapToProperty(AbstractDataMappingContext mappingContext)
            {
                this.MappingContext = mappingContext;
                return "Hello world";
            }

            public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

    }
}



