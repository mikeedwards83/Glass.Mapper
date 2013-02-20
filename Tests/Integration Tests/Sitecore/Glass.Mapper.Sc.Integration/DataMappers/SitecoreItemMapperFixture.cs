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
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreItemMapperFixture : AbstractMapperFixture
    {
        #region Property - ReadOnly

        [Test]
        public void ReadOnly_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreItemMapper();

            //Act
            var result = mapper.ReadOnly;

            //Assert
            Assert.IsTrue(result);

        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_ConfigIsNodeAndClassMapped_ReturnsTrue()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(new GlassConfig());
            var mapper = new SitecoreItemMapper();

            config.PropertyInfo = new FakePropertyInfo(typeof(StubMapped));
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_ConfigIsNodeAndClassNotMapped_ReturnsFalse()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(new GlassConfig());
            var mapper = new SitecoreItemMapper();

            config.PropertyInfo = new FakePropertyInfo(typeof(StubNotMapped));
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CanHandle_ConfigIsNotNodeAndClassMapped_ReturnsTrue()
        {
            //Assign
            var config = new SitecoreFieldConfiguration();
            var context = Context.Create(new GlassConfig());
            var mapper = new SitecoreItemMapper();

            config.PropertyInfo = new FakePropertyInfo(typeof(StubMapped));
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Method - MapToProperty

        [Test]
        public void MapToProperty_GetItemByPath_ReturnsItem()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(new GlassConfig());
            var mapper = new SitecoreItemMapper();
            var language = LanguageManager.GetLanguage("en");
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();
            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Source", language);
            var target = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target", language);
            var service = Substitute.For<ISitecoreService>();
            var expected = new StubMapped();

            config.PropertyInfo = typeof (Stub).GetProperty("Property");
            config.Path = "/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target";

            service.CreateType(
                typeof (StubMapped),
                Arg.Is<Item>(x => x.Paths.FullPath == target.Paths.FullPath && x.Language == language),
                false,
                false).Returns(expected);

            var mappingContext = new SitecoreDataMappingContext(obj, source, service);

            //Act
            var result = mapper.MapToProperty(mappingContext);

            //Assert
            Assert.AreEqual(expected, result);
            
        }

        [Test]
        public void MapToProperty_GetItemByPathDifferentLanguage_ReturnsItem()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(new GlassConfig());
            var mapper = new SitecoreItemMapper();
            var language = LanguageManager.GetLanguage("af-ZA");
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();
            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Source", language);
            var target = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target", language);
            var service = Substitute.For<ISitecoreService>();
            var expected = new StubMapped();

            config.PropertyInfo = typeof(Stub).GetProperty("Property");
            config.Path = "/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target";

            service.CreateType(
                typeof(StubMapped),
                Arg.Is<Item>(x => x.Paths.FullPath == target.Paths.FullPath && x.Language == language),
                false,
                false).Returns(expected);

            var mappingContext = new SitecoreDataMappingContext(obj, source, service);

            //Act
            var result = mapper.MapToProperty(mappingContext);

            //Assert
            Assert.AreEqual(expected, result); 
        }

        [Test]
        public void MapToProperty_GetItemByPathDifferentLanguageTargetDoesNotExistInLanguage_ReturnsNull()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(new GlassConfig());
            var mapper = new SitecoreItemMapper();
            var language = LanguageManager.GetLanguage("af-ZA");
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();
            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Source", language);
            var target = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/TargetOneLanguage", language);
            var service = Substitute.For<ISitecoreService>();
            var expected = new StubMapped();

            config.PropertyInfo = typeof(Stub).GetProperty("Property");
            config.Path = "/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target";

            service.CreateType(
                typeof(StubMapped),
                Arg.Is<Item>(x => x.Paths.FullPath == target.Paths.FullPath && x.Language == language),
                false,
                false).Returns(expected);

            var mappingContext = new SitecoreDataMappingContext(obj, source, service);

            //Act
            var result = mapper.MapToProperty(mappingContext);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void MapToProperty_GetItemByID_ReturnsItem()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(new GlassConfig());
            var mapper = new SitecoreItemMapper();
            var language = LanguageManager.GetLanguage("en");
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();
            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Source", language);
            var target = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target", language);
            var service = Substitute.For<ISitecoreService>();
            var expected = new StubMapped();

            config.PropertyInfo = typeof(Stub).GetProperty("Property");
            config.Id = "{EC4351CE-C5F1-4F01-B354-3D26DC7A66CD}";

            service.CreateType(
                typeof(StubMapped),
                Arg.Is<Item>(x => x.Paths.FullPath == target.Paths.FullPath && x.Language == language),
                false,
                false).Returns(expected);

            var mappingContext = new SitecoreDataMappingContext(obj, source, service);

            //Act
            var result = mapper.MapToProperty(mappingContext);

            //Assert
            Assert.AreEqual(expected, result);

        }

        [Test]
        public void MapToProperty_GetItemByIdDifferentLanguage_ReturnsItem()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(new GlassConfig());
            var mapper = new SitecoreItemMapper();
            var language = LanguageManager.GetLanguage("af-ZA");
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();
            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Source", language);
            var target = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target", language);
            var service = Substitute.For<ISitecoreService>();
            var expected = new StubMapped();

            config.PropertyInfo = typeof(Stub).GetProperty("Property");
            config.Id = "{EC4351CE-C5F1-4F01-B354-3D26DC7A66CD}";

            service.CreateType(
                typeof(StubMapped),
                Arg.Is<Item>(x => x.Paths.FullPath == target.Paths.FullPath && x.Language == language),
                false,
                false).Returns(expected);

            var mappingContext = new SitecoreDataMappingContext(obj, source, service);

            //Act
            var result = mapper.MapToProperty(mappingContext);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void MapToProperty_GetItemByIdDifferentLanguageTargetDoesNotExistInLanguage_ReturnsNull()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(new GlassConfig());
            var mapper = new SitecoreItemMapper();
            var language = LanguageManager.GetLanguage("af-ZA");
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();
            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Source", language);
            var target = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/TargetOneLanguage", language);
            var service = Substitute.For<ISitecoreService>();
            var expected = new StubMapped();

            config.PropertyInfo = typeof(Stub).GetProperty("Property");
            config.Id = "{03CDE6B5-B2A2-40D6-A944-53D66DDD2CA4}";

            service.CreateType(
                typeof(StubMapped),
                Arg.Is<Item>(x => x.Paths.FullPath == target.Paths.FullPath && x.Language == language),
                false,
                false).Returns(expected);

            var mappingContext = new SitecoreDataMappingContext(obj, source, service);

            //Act
            var result = mapper.MapToProperty(mappingContext);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void MapToProperty_GetItemByPathIsLazy_ReturnsItem()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(new GlassConfig());
            var mapper = new SitecoreItemMapper();
            var language = LanguageManager.GetLanguage("en");
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();
            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Source", language);
            var target = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target", language);
            var service = Substitute.For<ISitecoreService>();
            var expected = new StubMapped();

            config.PropertyInfo = typeof(Stub).GetProperty("Property");
            config.Path = "/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target";
            config.IsLazy = true;

            service.CreateType(
                typeof(StubMapped),
                Arg.Is<Item>(x => x.Paths.FullPath == target.Paths.FullPath && x.Language == language),
                true,
                false).Returns(expected);

            var mappingContext = new SitecoreDataMappingContext(obj, source, service);

            //Act
            var result = mapper.MapToProperty(mappingContext);

            //Assert
            Assert.AreEqual(expected, result);

        }
        [Test]
        public void MapToProperty_GetItemByPathInferType_ReturnsItem()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(new GlassConfig());
            var mapper = new SitecoreItemMapper();
            var language = LanguageManager.GetLanguage("en");
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();
            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Source", language);
            var target = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target", language);
            var service = Substitute.For<ISitecoreService>();
            var expected = new StubMapped();

            config.PropertyInfo = typeof(Stub).GetProperty("Property");
            config.Path = "/sitecore/content/Tests/DataMappers/SitecoreItemMapper/Target";
            config.InferType = true;

            service.CreateType(
                typeof(StubMapped),
                Arg.Is<Item>(x => x.Paths.FullPath == target.Paths.FullPath && x.Language == language),
                false,
                true).Returns(expected);

            var mappingContext = new SitecoreDataMappingContext(obj, source, service);

            //Act
            var result = mapper.MapToProperty(mappingContext);

            //Assert
            Assert.AreEqual(expected, result);

        }
        #endregion

        #region Stubs

        [SitecoreType]
        public class StubMapped
        {
           

        }

        public class StubNotMapped
        {
        }

        public class Stub
        {
            public StubMapped Property { get; set; }
        }


        #endregion

    }
}



