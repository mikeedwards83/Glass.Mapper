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
using Sitecore.Globalization;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    public class SitecoreLinkedMapperFixture : AbstractMapperFixture
    {
        #region Property - ReadOnly

        [Test]
        public void ReadOnly_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreLinkedMapper();

            //Act
            var result = mapper.ReadOnly;

            //Assert
            Assert.IsTrue(result);

        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_IsEnumerableOfMappedClassWithLinkedConfig_ReturnsTrue()
        {
            //Assign
            var config = new SitecoreLinkedConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubMapped>));
            var mapper = new SitecoreLinkedMapper();
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);
            
            //Assert
            Assert.IsTrue(result);

        }

        [Test]
        public void CanHandle_IsEnumerableOfNotMappedClassWithLinkedConfig_ReturnsFalse()
        {
            //Assign
            var config = new SitecoreLinkedConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubNotMapped>));
            var mapper = new SitecoreLinkedMapper();
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsFalse(result);

        }

        [Test]
        public void CanHandle_IsListOfMappedClassWithLinkedConfig_ReturnsFalse()
        {
            //Assign
            var config = new SitecoreLinkedConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(List<StubMapped>));
            var mapper = new SitecoreLinkedMapper();
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsFalse(result);

        }

        [Test]
        public void CanHandle_IsEnumerableOfMappedClassWithWrongConfig_ReturnsFalse()
        {
            //Assign
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubMapped>));
            var mapper = new SitecoreLinkedMapper();
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsFalse(result);

        }

        [Test]
        public void CanHandle_IsNonGenericOfMappedClassWithLinkedConfig_ReturnsTrue()
        {
            //Assign
            var config = new SitecoreLinkedConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(StubMapped));
            var mapper = new SitecoreLinkedMapper();
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsFalse(result);

        }

        #endregion

        #region Method - MapToProperty

        [Test]
        public void MapToProperty_GetAllReferrers_ReferrersListReturned()
        {
            //Assign
            var language = LanguageManager.GetLanguage("af-ZA");
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreLinkedMapper/Target");
            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreLinkedMapper/Source",
                                        language  );

            using (new ItemEditing(source, true))
            {
                source[FieldName] = "<a href=\"~/link.aspx?_id=216C0015-8626-4951-9730-85BCA34EC2A3&amp;_z=z\">Source</a>";
            }

            var config = new SitecoreLinkedConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubMapped>));
            config.Option = SitecoreLinkedOptions.Referrers;
            
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var mapper = new SitecoreLinkedMapper();
            mapper.Setup(new DataMapperResolverArgs(context, config));

            var service = new SitecoreService(Database, context);

            //Act
            var result =
                mapper.MapToProperty(new SitecoreDataMappingContext(null, item, service)) as IEnumerable<StubMapped>;

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(source.ID.Guid, result.First().Id);
            Assert.AreEqual(language, result.First().Language);
        }

        [Test]
        public void MapToProperty_GetAllReferences_ReferrersListReturned()
        {
            //Act
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreLinkedMapper/Target");
           
            //ME - when getting templates you have to disable the role manager
            using (new SecurityDisabler())
            {
                var template = Database.GetItem("/sitecore/templates/Tests/DataMappers/DataMappersSingleField");
         
            var config = new SitecoreLinkedConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubMapped>));
            config.Option = SitecoreLinkedOptions.References;

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var mapper = new SitecoreLinkedMapper();
            mapper.Setup(new DataMapperResolverArgs(context, config));

            var service = new SitecoreService(Database, context);

            //Act
            var result =
                mapper.MapToProperty(new SitecoreDataMappingContext(null, item, service)) as IEnumerable<StubMapped>;

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(template.ID.Guid, result.First().Id);
            }
        }

        [Test]
        public void MapToProperty_GetAll_ReferrersListReturned()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreLinkedMapper/Target");
            var language = LanguageManager.GetLanguage("af-ZA");

            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreLinkedMapper/Source",
                                             language);

            using (new ItemEditing(source, true))
            {
                source[FieldName] = "<a href=\"~/link.aspx?_id=216C0015-8626-4951-9730-85BCA34EC2A3&amp;_z=z\">Source</a>";
            }
        

        //ME - when getting templates you have to disable the role manager
            using (new SecurityDisabler())
            {

                var template = Database.GetItem("/sitecore/templates/Tests/DataMappers/DataMappersSingleField");
               

                var config = new SitecoreLinkedConfiguration();
                config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubMapped>));
                config.Option = SitecoreLinkedOptions.All;

                var context = Context.Create(new GlassConfig());
                context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

                var mapper = new SitecoreLinkedMapper();
                mapper.Setup(new DataMapperResolverArgs(context, config));

                var service = new SitecoreService(Database, context);

                //Act
                var result =
                    mapper.MapToProperty(new SitecoreDataMappingContext(null, item, service)) as IEnumerable<StubMapped>;

                //Assert
                Assert.AreEqual(2, result.Count());
                Assert.AreEqual(template.ID.Guid, result.First().Id);
                Assert.AreEqual(source.ID.Guid, result.Skip(1).First().Id);
                Assert.AreEqual(source.Language, result.Skip(1).First().Language);
            }
        }
        #endregion

        #region Stubs

        [SitecoreType]
        public class StubMapped
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreInfo(SitecoreInfoType.Language)]
            public virtual Language Language { get; set; }
        } 

        public class StubNotMapped
        {
        }

        #endregion
    }
}



