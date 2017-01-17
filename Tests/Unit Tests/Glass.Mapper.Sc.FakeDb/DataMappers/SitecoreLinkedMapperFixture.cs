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
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.FakeDb;
using Sitecore.Globalization;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    public class SitecoreLinkedMapperFixture
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
            config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");

            var mapper = new SitecoreLinkedMapper();
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));



            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);

        }

        [Test]
        public void CanHandle_IsEnumerableOfNotMappedClassWithLinkedConfig_ReturnsTrueOnDemand()
        {
            //Assign
            var config = new SitecoreLinkedConfiguration();
            config.PropertyInfo = typeof(StubClass).GetProperty("StubNotMappeds");

            var mapper = new SitecoreLinkedMapper();
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));



            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);

        }

        [Test]
        public void CanHandle_IsListOfMappedClassWithLinkedConfig_ReturnsFalse()
        {
            //Assign
            var config = new SitecoreLinkedConfiguration();
            config.PropertyInfo = typeof(StubClass).GetProperty("StubMappedsList");

            var mapper = new SitecoreLinkedMapper();
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));



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
            config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");
            var mapper = new SitecoreLinkedMapper();
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));



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
            config.PropertyInfo = typeof(StubClass).GetProperty("StubMapped");

            var mapper = new SitecoreLinkedMapper();
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));



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
            var templateId = ID.NewID;
            var fieldId = ID.NewID;
            var targetId = ID.NewID;
            var language = LanguageManager.GetLanguage("af-ZA");

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),
                new Sitecore.FakeDb.DbItem("Source", ID.NewID, templateId)
                {
                    new DbField("Field", fieldId)
                    {
                        { language.Name, 1, targetId.ToString()}
                    }
                }
            })
            {

                var target = database.Database.GetItem("/sitecore/content/Target");
                var source = database.Database.GetItem("/sitecore/content/Source", language);

                var behavior = Substitute.For<Sitecore.Links.LinkDatabase>();

                behavior.GetReferrers(target).Returns(new[]
                {
                    new Sitecore.Links.ItemLink(source, fieldId, target, target.Paths.FullPath),
                });

                using (new Sitecore.FakeDb.Links.LinkDatabaseSwitcher(behavior))
                {

                    var config = new SitecoreLinkedConfiguration();
                    config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");
                    config.Option = SitecoreLinkedOptions.Referrers;

                    var context = Context.Create(Utilities.CreateStandardResolver());
                    context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

                    var mapper = new SitecoreLinkedMapper();
                    mapper.Setup(new DataMapperResolverArgs(context, config));

                    var service = new SitecoreService(database.Database, context);

                    //Act

                    var result =
                        mapper.MapToProperty(new SitecoreDataMappingContext(null, target, service)) as
                            IEnumerable<StubMapped>;

                    //Assert
                    Assert.AreEqual(1, result.Count());
                    Assert.AreEqual(source.ID.Guid, result.First().Id);
                    Assert.AreEqual(language, result.First().Language);
                }
            }
        }

        [Test]
        public void MapToProperty_GetAllReferences_ReferrersListReturned()
        {
            //Arrange
            var templateId = ID.NewID;
            var fieldId = ID.NewID;
            var targetId = ID.NewID;
            var language = LanguageManager.GetLanguage("af-ZA");

            using (Db database = new Db
            {
                new DbTemplate(templateId),
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),
            })
            {

                var target = database.Database.GetItem("/sitecore/content/Target");
                var source = database.Database.GetItem("/sitecore/content/Source");
                var template = database.Database.GetTemplate(templateId);

                var behavior = Substitute.For<Sitecore.Links.LinkDatabase>();

                behavior.GetReferences(target).Returns(new[]
                {
                    new Sitecore.Links.ItemLink(target, fieldId, template, template.InnerItem.Paths.FullPath),
                });
              

                using (new Sitecore.FakeDb.Links.LinkDatabaseSwitcher(behavior))
                {
                    //Act
                    var item = database.GetItem("/sitecore/content/Target");

                    //ME - when getting templates you have to disable the role manager
                    using (new SecurityDisabler())
                    {
                        var config = new SitecoreLinkedConfiguration();
                        config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");
                        config.Option = SitecoreLinkedOptions.References;

                        var context = Context.Create(Utilities.CreateStandardResolver());
                        context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));


                        var mapper = new SitecoreLinkedMapper();
                        mapper.Setup(new DataMapperResolverArgs(context, config));

                        var service = new SitecoreService(database.Database, context);

                        //Act
                        var result =
                            mapper.MapToProperty(new SitecoreDataMappingContext(null, item, service)) as
                                IEnumerable<StubMapped>;

                        //Assert
                        Assert.AreEqual(1, result.Count());
                        Assert.AreEqual(templateId.Guid, result.First().Id);
                    }
                }
            }
        }

        [Test]
        public void MapToProperty_GetAll_ReferrersListReturned()
        {
            //Assign

            var templateId = ID.NewID;
            var fieldId = ID.NewID;
            var targetId = ID.NewID;
            var language = LanguageManager.GetLanguage("af-ZA");

            using (Db database = new Db
            {
                new DbTemplate(templateId),
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),
                new Sitecore.FakeDb.DbItem("Source", ID.NewID, templateId)
                {
                    new DbField("Field", fieldId)
                    {
                        {language.Name, 1, targetId.ToString()}
                    }
                }
            })
            {


                var target = database.GetItem("/sitecore/content/Target");
                var source = database.Database.GetItem("/sitecore/content/Source", language);
                var template = database.Database.GetTemplate(templateId);

                var behavior = Substitute.For<Sitecore.Links.LinkDatabase>();

                behavior.GetReferences(target).Returns(new[]
                {
                    new Sitecore.Links.ItemLink(target, fieldId, template, template.InnerItem.Paths.FullPath),
                });
                behavior.GetReferrers(target).Returns(new[]
                {
                    new Sitecore.Links.ItemLink(source, fieldId, target, target.Paths.FullPath),
                });


                using (new Sitecore.FakeDb.Links.LinkDatabaseSwitcher(behavior))
                {
                    //ME - when getting templates you have to disable the role manager
                    using (new SecurityDisabler())
                    {

                        var config = new SitecoreLinkedConfiguration();
                        config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");
                        config.Option = SitecoreLinkedOptions.All;

                        var context = Context.Create(Utilities.CreateStandardResolver());
                        context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

                        var mapper = new SitecoreLinkedMapper();
                        mapper.Setup(new DataMapperResolverArgs(context, config));

                        var service = new SitecoreService(database.Database, context);

                        //Act
                        var result =
                            mapper.MapToProperty(new SitecoreDataMappingContext(null, target, service)) as
                                IEnumerable<StubMapped>;

                        //Assert
                        Assert.AreEqual(2, result.Count());
                        Assert.AreEqual(template.ID.Guid, result.First().Id);
                        Assert.AreEqual(source.ID.Guid, result.Skip(1).First().Id);
                        Assert.AreEqual(source.Language, result.Skip(1).First().Language);
                    }
                }
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

        public class StubClass
        {
            public IEnumerable<StubMapped> StubMappeds { get; set; }
            public IEnumerable<StubNotMapped> StubNotMappeds { get; set; }
            public List<StubMapped> StubMappedsList { get; set; }
            public StubMapped StubMapped { get; set; }
        }

        #endregion
    }
}




