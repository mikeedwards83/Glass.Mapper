using System;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
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
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var item = database.GetItem("/sitecore/content/TestItem");
                var service = Substitute.For<ISitecoreService>();
                var options = new GetItemOptionsParams();
                options.Lazy = LazyLoading.Enabled;
                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                var config = new SitecoreParentConfiguration();
                config.PropertyInfo = typeof(Stub).GetProperty("Property");

                var mapper = new SitecoreParentMapper();
                mapper.Setup(new DataMapperResolverArgs(null, config));

                //Act
                var result = mapper.MapToProperty(scContext);

                //Assert

                //ME - I am not sure why I have to use the Arg.Is but just using item.Parent as the argument fails.
                service.Received().GetItem(Arg.Is<GetItemByItemOptions>(x=>x.Item.Uri == item.Parent.Uri && x.Lazy == LazyLoading.Enabled));
            }
        }

        [Test]
        public void MapToProperty_ConfigurationIsLazy_CallsCreateClassOnServiceWithIsLazy()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var item = database.GetItem("/sitecore/content/TestItem");
                var service = Substitute.For<ISitecoreService>();
                var options = new GetItemOptionsParams();
                options.Lazy = LazyLoading.Enabled;
                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                var config = new SitecoreParentConfiguration();
                config.PropertyInfo = typeof(Stub).GetProperty("Property");

                var mapper = new SitecoreParentMapper();
                mapper.Setup(new DataMapperResolverArgs(null, config));

                //Act
                var result = mapper.MapToProperty(scContext);

                //Assert

                service.Received()
                    .GetItem(Arg.Is<GetItemByItemOptions>(x => x.Item.Uri == item.Parent.Uri &&
                                                           x.Lazy == LazyLoading.Enabled));
            }
        }


        [Test]
        public void MapToProperty_EnforceTemplate_ReturnsParentItem()
        {
            //Assign

            ID templateId = ID.NewID;
            ID parentID = ID.NewID;

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Parent", parentID, templateId){
                    new Sitecore.FakeDb.DbItem("TestItem")
                }
            })
            {
                var item = database.GetItem("/sitecore/content/Parent/TestItem");
                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database, context);
                var options = new GetItemOptionsParams();
                options.Lazy = LazyLoading.Enabled;
                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                var config = new SitecoreParentConfiguration();
                config.PropertyInfo = typeof(Stub).GetProperty("Property");
                config.TemplateId = templateId;
                config.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;

                var mapper = new SitecoreParentMapper();
                mapper.Setup(new DataMapperResolverArgs(null, config));

                //Act
                var result = mapper.MapToProperty(scContext) as Stub;

                //Assert
                Assert.NotNull(result);
                Assert.AreEqual(parentID, result.Id);
            }
        }

        [Test]
        public void MapToProperty_EnforceTemplate_ReturnsNoItem()
        {
            //Assign

            ID templateId = ID.NewID;
            ID templateIdOther = ID.NewID;
            ID parentID = ID.NewID;

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Parent", parentID, templateId){
                    new Sitecore.FakeDb.DbItem("TestItem")
                }
            })
            {
                var item = database.GetItem("/sitecore/content/Parent/TestItem");
                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database, context);
                var options = new GetItemOptionsParams();
                options.Lazy = LazyLoading.Enabled;
                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                var config = new SitecoreParentConfiguration();
                config.PropertyInfo = typeof(Stub).GetProperty("Property");
                config.TemplateId = templateIdOther;
                config.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;

                var mapper = new SitecoreParentMapper();
                mapper.Setup(new DataMapperResolverArgs(null, config));

                //Act
                var result = mapper.MapToProperty(scContext) as Stub;

                //Assert
                Assert.Null(result);
            }
        }

        [Test]
        public void MapToProperty_ConfigurationInferType_CallsCreateClassOnServiceWithInferType()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var item = database.GetItem("/sitecore/content/TestItem");
                var service = Substitute.For<ISitecoreService>();
                var options = new GetItemOptionsParams();;
                options.Lazy = LazyLoading.Enabled;
                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                var config = new SitecoreParentConfiguration();
                config.PropertyInfo = typeof(Stub).GetProperty("Property");
                config.InferType = true;

                var mapper = new SitecoreParentMapper();
                mapper.Setup(new DataMapperResolverArgs(null, config));

                //Act
                var result = mapper.MapToProperty(scContext);

                //Assert

                service.Received().GetItem(Arg.Is<GetItemByItemOptions>(x => x.Item.Uri == item.Parent.Uri && x.Lazy == LazyLoading.Enabled));

            }
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
        public void MapToCms_ThrowsException()
        {
            //Assign
            var mapper = new SitecoreParentMapper();

            //Act
            Assert.Throws<NotSupportedException>(()=> mapper.MapToCms(null));
        }

        #endregion

        #region Stubs

        public class Stub
        {
            public virtual ID Id { get; set; }
            public virtual Stub Property { get; set; }
        }

        #endregion
    }
}




