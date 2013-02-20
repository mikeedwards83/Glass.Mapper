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
using Glass.Mapper.Sc.DataMappers;
using NSubstitute;
using NUnit.Framework;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreParentMapperFixture
    {
        #region Property - ReadOnly

        [Test]
        public void ReadOnly_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreParentMapper();

            //Act
            var result = mapper.ReadOnly;

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Method - MapToProperty

        [Test]
        public void MapToProperty_ConfigurationSetupCorrectly_CallsCreateClassOnService()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var item = db.GetItem("/sitecore/content/Tests/DataMappers/SitecoreParentMapperFixture/EmptyItem");
            var service = Substitute.For<ISitecoreService>();
            var scContext = new SitecoreDataMappingContext(null, item, service);

            var config = new SitecoreParentConfiguration();
            config.PropertyInfo = typeof (Stub).GetProperty("Property");
            
            var mapper = new SitecoreParentMapper();
            mapper.Setup(new DataMapperResolverArgs(null,config));

            //Act
            var result = mapper.MapToProperty(scContext);

            //Assert

            //ME - I am not sure why I have to use the Arg.Is but just using item.Parent as the argument fails.
            service.Received().CreateType(config.PropertyInfo.PropertyType, Arg.Is<Item>(x => x.ID == item.Parent.ID), false, false);

        }

        [Test]
        public void MapToProperty_ConfigurationIsLazy_CallsCreateClassOnServiceWithIsLazy()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var item = db.GetItem("/sitecore/content/Tests/DataMappers/SitecoreParentMapperFixture/EmptyItem");
            var service = Substitute.For<ISitecoreService>();
            var scContext = new SitecoreDataMappingContext(null, item, service);

            var config = new SitecoreParentConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");
            config.IsLazy = true;

            var mapper = new SitecoreParentMapper();
            mapper.Setup(new DataMapperResolverArgs(null,config));

            //Act
            var result = mapper.MapToProperty(scContext);

            //Assert

            //ME - I am not sure why I have to use the Arg.Is but just using item.Parent as the argument fails.
            service.Received().CreateType(config.PropertyInfo.PropertyType, Arg.Is<Item>(x => x.ID == item.Parent.ID), true, false);
        }

        [Test]
        public void MapToProperty_ConfigurationInferType_CallsCreateClassOnServiceWithInferType()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var item = db.GetItem("/sitecore/content/Tests/DataMappers/SitecoreParentMapperFixture/EmptyItem");
            var service = Substitute.For<ISitecoreService>();
            var scContext = new SitecoreDataMappingContext(null, item, service);

            var config = new SitecoreParentConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");
            config.InferType = true;

            var mapper = new SitecoreParentMapper();
            mapper.Setup(new DataMapperResolverArgs(null,config));

            //Act
            var result = mapper.MapToProperty(scContext);

            //Assert

            //ME - I am not sure why I have to use the Arg.Is but just using item.Parent as the argument fails.
            service.Received().CreateType(config.PropertyInfo.PropertyType, Arg.Is<Item>(x => x.ID == item.Parent.ID), false, true);
        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_ConfigurationIsSitecoreParent_ReturnsTrue()
        {
            //Assign
            var config = new SitecoreParentConfiguration();
            var mapper = new SitecoreParentMapper();

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_ConfigurationIsSitecoreInfo_ReturnsFalse()
        {
            //Assign
            var config = new SitecoreInfoConfiguration();
            var mapper = new SitecoreParentMapper();

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Method - MapToCms

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void MapToCms_ThrowsException()
        {
            //Assign
            var mapper = new SitecoreParentMapper();

            //Act
            mapper.MapToCms(null);
        }

        #endregion

        #region Stubs

        public class Stub
        {
            public string Property { get; set; }
        }

        #endregion
    }
}



