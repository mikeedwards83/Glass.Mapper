using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data.Managers;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreNodeMapperFixture
    {
        protected const string FieldName = "Field";

        #region Property - ReadOnly

        [Test]
        public void ReadOnly_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreNodeMapper();

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
            var mapper = new SitecoreNodeMapper();

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_ConfigIsNotNodeAndClassMapped_ReturnsFalse()
        {
            //Assign
            var config = new SitecoreFieldConfiguration();
            var mapper = new SitecoreNodeMapper();


            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Method - MapToProperty

        [Test]
        public void MapToProperty_GetItemByPath_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Source"),
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var config = new SitecoreNodeConfiguration();
                var mapper = new SitecoreNodeMapper();
                var language = LanguageManager.GetLanguage("en");

                mapper.Setup(new DataMapperResolverArgs(null, config));

                var obj = new Stub();
                var source = database.Database.GetItem("/sitecore/content/Source", language);
                var target = database.Database.GetItem("/sitecore/content/Target", language);
                var service = Substitute.For<ISitecoreService>();
                var expected = new StubMapped();
                var options = new GetItemOptionsParams();

                config.PropertyInfo = typeof(Stub).GetProperty("StubMapped");
                config.Path = "/sitecore/content/Target";

                service.GetItem(Arg.Is<GetItemByItemOptions>(x => x.Item.Uri == target.Uri)).Returns(expected);
                  
                var mappingContext = new SitecoreDataMappingContext(obj, source, service, options);

                //Act
                var result = mapper.MapToProperty(mappingContext);

                //Assert
                Assert.AreEqual(expected, result);
            }

        }

    

        [Test]
        public void MapToProperty_GetItemByPathDifferentLanguage_ReturnsItem()
        {
            //Assign

            var language = LanguageManager.GetLanguage("af-ZA");

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Source")
                {
                    new Sitecore.FakeDb.DbField("Title")
                    {
                         {language.Name , language.Name }
                    }
                },
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbField("Title")
                    {
                         {language.Name , language.Name }
                    }
                },
            })
            {
                var config = new SitecoreNodeConfiguration();
                var mapper = new SitecoreNodeMapper();

                mapper.Setup(new DataMapperResolverArgs(null, config));

                var obj = new Stub();
                var source = database.Database.GetItem("/sitecore/content/Source", language);
                var target = database.Database.GetItem("/sitecore/content/Target", language);
                var service = Substitute.For<ISitecoreService>();
                var expected = new StubMapped();
                var options = new GetItemOptionsParams();

                config.PropertyInfo = typeof(Stub).GetProperty("StubMapped");
                config.Path = "/sitecore/content/Target";


                service.GetItem(Arg.Is<GetItemByItemOptions>(x => x.Item.Uri == target.Uri)).Returns(expected);
            

                var mappingContext = new SitecoreDataMappingContext(obj, source, service, options);

                //Act
                var result = mapper.MapToProperty(mappingContext);

                //Assert
                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void MapToProperty_GetItemByPathDifferentLanguageTargetDoesNotExistInLanguage_ReturnsNull()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var mapper = new SitecoreNodeMapper();
            var language = LanguageManager.GetLanguage("af-ZA");

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Source")
                {
                    new Sitecore.FakeDb.DbField("Title")
                    {
                        {language.Name, language.Name}
                    }
                },
                new Sitecore.FakeDb.DbItem("TargetOneLanguage")
                {
                   
                },
            })
            {
                mapper.Setup(new DataMapperResolverArgs(null, config));

                var obj = new Stub();
                var source = database.Database.GetItem("/sitecore/content/Source", language);
                var target = database.Database.GetItem(
                    "/sitecore/content/Tests/DataMappers/SitecoreNodeMapper/TargetOneLanguage", language);
                var service = Substitute.For<ISitecoreService>();
                var expected = new StubMapped();
                var options = new GetItemOptionsParams();

                config.PropertyInfo = typeof(Stub).GetProperty("StubMapped");
                config.Path = "/sitecore/content/Tests/DataMappers/SitecoreNodeMapper/Target";


                service.GetItem(Arg.Is<GetItemByItemOptions>(x => x.Item.Uri == target.Uri && x.Item.Versions.Count == 0)).Returns(expected);


                var mappingContext = new SitecoreDataMappingContext(obj, source, service,options);

                //Act
                var result = mapper.MapToProperty(mappingContext);

                //Assert
                Assert.IsNull(result);
            }
        }

        [Test]
        public void MapToProperty_GetItemByID_ReturnsItem()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(FakeDb.Utilities.CreateStandardResolver());
            var mapper = new SitecoreNodeMapper();
            var language = LanguageManager.GetLanguage("en");
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();

            using (Db database = new Db{ 
                     new Sitecore.FakeDb.DbItem("Source"),
                     new Sitecore.FakeDb.DbItem("Target")
                })
            {
                var source = database.Database.GetItem("/sitecore/content/Source", language);
                var target = database.Database.GetItem("/sitecore/content/Target", language);
                var service = Substitute.For<ISitecoreService>();
                var expected = new StubMapped();
                var options = new GetItemOptionsParams();

                config.PropertyInfo = typeof(Stub).GetProperty("StubMapped");
                config.Id = target.ID.Guid.ToString();


                service.GetItem(Arg.Is<GetItemByItemOptions>(x => x.Item.Uri == target.Uri )).Returns(expected);



                var mappingContext = new SitecoreDataMappingContext(obj, source, service, options);

                //Act
                var result = mapper.MapToProperty(mappingContext);

                //Assert
                Assert.AreEqual(expected, result);
            }

        }

        [Test]
        public void MapToProperty_GetItemByIdDifferentLanguage_ReturnsItem()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(FakeDb.Utilities.CreateStandardResolver());
            var mapper = new SitecoreNodeMapper();
            var language = LanguageManager.GetLanguage("af-ZA");
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Source"),
                new Sitecore.FakeDb.DbItem("Target")
            })
            {


                var source = database.Database.GetItem("/sitecore/content/Source", language);
                var target = database.Database.GetItem("/sitecore/content/Target", language);
                var service = Substitute.For<ISitecoreService>();
                var expected = new StubMapped();
                var options = new GetItemOptionsParams();

                config.PropertyInfo = typeof(Stub).GetProperty("StubMapped");
                config.Id = target.ID.Guid.ToString();

                service.GetItem(Arg.Is<GetItemByItemOptions>(x => x.Item.Uri == target.Uri)).Returns(expected);

                

                var mappingContext = new SitecoreDataMappingContext(obj, source, service, options);

                //Act
                var result = mapper.MapToProperty(mappingContext);

                //Assert
                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void MapToProperty_GetItemByIdDifferentLanguageTargetDoesNotExistInLanguage_ReturnsNull()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(FakeDb.Utilities.CreateStandardResolver());
            var mapper = new SitecoreNodeMapper();
            var language = LanguageManager.GetLanguage("af-ZA");



            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));


            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Source"),
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var source = database.Database.GetItem("/sitecore/content/Source", language);
                var target = database.Database.GetItem("/sitecore/content/Target", language);
                var service = new SitecoreService(database.Database, context);
                var options = new GetItemOptionsParams();

                config.PropertyInfo = typeof(Stub).GetProperty("StubMapped");
                config.Id = target.ID.Guid.ToString();

                var mappingContext = new SitecoreDataMappingContext(obj, source, service, options);

                //Act
                var result = mapper.MapToProperty(mappingContext);

                //Assert
                Assert.IsNull(result);
            }
        }

        [Test]
        public void MapToProperty_GetItemByPathIsLazy_ReturnsItem()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(FakeDb.Utilities.CreateStandardResolver());
            var mapper = new SitecoreNodeMapper();
            var language = LanguageManager.GetLanguage("en");
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Source"),
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var source = database.Database.GetItem("/sitecore/content/Source", language);
                var target = database.Database.GetItem("/sitecore/content/Target", language);
                var service = Substitute.For<ISitecoreService>();
                var expected = new StubMapped();
                var options = new GetItemOptionsParams
                {
                    Lazy = LazyLoading.Enabled
                };

                config.PropertyInfo = typeof(Stub).GetProperty("StubMapped");
                config.Path = "/sitecore/content/Target";


                service.GetItem(Arg.Is<GetItemByItemOptions>(x => x.Item.Uri == target.Uri && x.Lazy == LazyLoading.Enabled)).Returns(expected);

                var mappingContext = new SitecoreDataMappingContext(obj, source, service, options);

                //Act
                var result = mapper.MapToProperty(mappingContext);

                //Assert
                Assert.AreEqual(expected, result);
            }
        }


        [Test]
        public void MapToProperty_GetItemByPathInferType_ReturnsItem()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(FakeDb.Utilities.CreateStandardResolver());
            var mapper = new SitecoreNodeMapper();
            var language = LanguageManager.GetLanguage("en");
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Source"),
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var source = database.Database.GetItem("/sitecore/content/Source", language);
                var target = database.Database.GetItem("/sitecore/content/Target", language);
                var service = Substitute.For<ISitecoreService>();
                var expected = new StubMapped();
                var options = new GetItemOptionsParams();

                config.PropertyInfo = typeof(Stub).GetProperty("StubMapped");
                config.Path = "/sitecore/content/Target";
                config.InferType = true;

                service.GetItem(Arg.Is<GetItemByItemOptions>(x => x.Item.Uri == target.Uri)).Returns(expected);

                var mappingContext = new SitecoreDataMappingContext(obj, source, service, options);

                //Act
                var result = mapper.MapToProperty(mappingContext);

                //Assert
                Assert.AreEqual(expected, result);
            }
        }


        [Test]
        public void MapToProperty_GetItemByPathIsLazyIntegrationTest_ReturnsItem()
        {
            //Assign
            var config = new SitecoreNodeConfiguration();
            var context = Context.Create(FakeDb.Utilities.CreateStandardResolver());
            var mapper = new SitecoreNodeMapper();
            var language = LanguageManager.GetLanguage("en");
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

            mapper.Setup(new DataMapperResolverArgs(context, config));

            var obj = new Stub();
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Source"),
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var source = database.Database.GetItem("/sitecore/content/Source", language);
                var target = database.Database.GetItem("/sitecore/content/Target", language);
                var service = new SitecoreService(database.Database);
                var expected = new StubMapped();
                var options = new GetItemOptionsParams
                {
                    Lazy = LazyLoading.Enabled
                };

                config.PropertyInfo = typeof(Stub).GetProperty("StubMapped");
                config.Path = "/sitecore/content/Target";

                var mappingContext = new SitecoreDataMappingContext(obj, source, service, options);

                //Act
                var result = mapper.MapToProperty(mappingContext);

                //Assert
                Assert.True(result is StubMapped);
            }
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
            public StubMapped StubMapped { get; set; }
            public StubNotMapped StubNotMapped { get; set; }
        }


        #endregion

    }
}




