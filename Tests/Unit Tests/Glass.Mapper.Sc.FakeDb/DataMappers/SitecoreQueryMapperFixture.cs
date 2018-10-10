


using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreQueryMapperFixture
    {

        #region Property - ReadOnly

        [Test]
        public void ReadOnly_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);

            //Act
            var result = mapper.ReadOnly;

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_CorrectConfigIEnumerableMappedClass_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");


            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));



            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_CorrectConfigIEnumerableNotMappedClass_ReturnsTrueOnDemand()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = typeof(StubClass).GetProperty("StubNotMappeds");

            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));



            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_CorrectConfigNotMappedClass_ReturnsTrueOnDemand()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = typeof(StubClass).GetProperty("StubNotMapped");

            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));



            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_CorrectConfigMappedClass_ReturnsFalse()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = typeof(StubClass).GetProperty("StubMapped");

            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));



            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_IncorrectConfigMappedClass_ReturnsFalse()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubClass).GetProperty("StubMapped");

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
        public void MapToProperty_RelativeQuery_ReturnsNoResults()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                     new Sitecore.FakeDb.DbItem("Child1"),
                     new Sitecore.FakeDb.DbItem("Child2")
                }
            })
            {

                var config = new SitecoreQueryConfiguration();
                config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");

                config.Query = "../Target/DoesNotExist/*";
                config.IsRelative = true;

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

                var mapper = new SitecoreQueryMapper(null);
                mapper.Setup(new DataMapperResolverArgs(context, config));

                var source = database.GetItem("/sitecore/content/Target");
                var service = new SitecoreService(database.Database, context);
                var options = new GetItemOptionsParams();

                //Act
                var results =
                    mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service, options)) as
                        IEnumerable<StubMapped>;

                //Assert
                Assert.AreEqual(0, results.Count());
            }
        }

        [Test]
        public void MapToProperty_RelativeQueryEnforceTemplate_ReturnsNoResults()
        {
            //Assign

            var templateId = ID.NewID;
            var childId = ID.NewID;

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child1", childId , templateId),
                    new Sitecore.FakeDb.DbItem("Child2")
                }
            })
            {

                var config = new SitecoreQueryConfiguration();
                config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");

                config.Query = "../Target/*";
                config.IsRelative = true;
                config.TemplateId = templateId;
                config.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

                var mapper = new SitecoreQueryMapper(null);
                mapper.Setup(new DataMapperResolverArgs(context, config));

                var source = database.GetItem("/sitecore/content/Target");
                var service = new SitecoreService(database.Database, context);
                var options = new GetItemOptionsParams();

                //Act
                var results =
                    mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service, options)) as
                        IEnumerable<StubMapped>;

                //Assert
                Assert.AreEqual(1, results.Count());
            }
        }



        [Test]
        public void MapToProperty_RelativeQuerySelf_ReturnsSelf()
        {
            //Assign
            //Assign

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child1"),
                    new Sitecore.FakeDb.DbItem("Child2")
                }
            })
            {
                var config = new SitecoreQueryConfiguration();
                config.PropertyInfo = typeof(StubClass).GetProperty("StubMapped");

                config.Query = "ancestor-or-self::*";
                config.IsRelative = true;

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

                var mapper = new SitecoreQueryMapper(null);
                mapper.Setup(new DataMapperResolverArgs(context, config));

                var source = database.GetItem("/sitecore/content/Target");
                var service = new SitecoreService(database.Database, context);
                var options = new GetItemOptionsParams();

                //Act
                var result =
                    mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service, options)) as StubMapped;

                //Assert
                Assert.AreEqual(source.ID.Guid, result.Id);
            }
        }

        [Test]
        public void MapToProperty_RelativeQuery_ReturnsResults()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child1"),
                    new Sitecore.FakeDb.DbItem("Child2")
                }
            })
            {
                var config = new SitecoreQueryConfiguration();
                config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");
                config.Query = "../Target/*";
                config.IsRelative = true;

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));


                var mapper = new SitecoreQueryMapper(null);
                mapper.Setup(new DataMapperResolverArgs(context, config));

                var source = database.GetItem("/sitecore/content/Target");
                var service = new SitecoreService(database.Database, context);

                var result1 = database.GetItem("/sitecore/content/Target/Child1");
                var result2 = database.GetItem("/sitecore/content/Target/Child2");
                var options = new GetItemOptionsParams();

                //Act
                var results =
                    mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service, options)) as
                        IEnumerable<StubMapped>;

                //Assert
                Assert.AreEqual(2, results.Count());
                Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
                Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));
            }
        }

        [Test]
        public void MapToProperty_AbsoluteQuery_ReturnsResults()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child1"),
                    new Sitecore.FakeDb.DbItem("Child2")
                }
            })
            {
                var config = new SitecoreQueryConfiguration();
                config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");

                config.Query = "/sitecore/content/Target/*";
                config.IsRelative = false;

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

                var mapper = new SitecoreQueryMapper(null);
                mapper.Setup(new DataMapperResolverArgs(context, config));

                var source = database.GetItem("/sitecore/content/Target");
                var service = new SitecoreService(database.Database, context);

                var result1 = database.GetItem("/sitecore/content/Target/Child1");
                var result2 = database.GetItem("/sitecore/content/Target/Child2");
                var options = new GetItemOptionsParams();

                //Act
                var results =
                    mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service, options)) as
                        IEnumerable<StubMapped>;

                //Assert
                Assert.AreEqual(2, results.Count());
                Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
                Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));
            }
        }

        [Test]
        public void MapToProperty_RelativeQuery_ReturnsSingleResults()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child1"),
                    new Sitecore.FakeDb.DbItem("Child2")
                }
            })
            {
                var config = new SitecoreQueryConfiguration();
                config.PropertyInfo = typeof(StubClass).GetProperty("StubMapped");
                config.Query = "../Target/Child1";
                config.IsRelative = true;

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

                var mapper = new SitecoreQueryMapper(null);
                mapper.Setup(new DataMapperResolverArgs(context, config));

                var source = database.GetItem("/sitecore/content/Target");
                var service = new SitecoreService(database.Database, context);

                var result1 = database.GetItem("/sitecore/content/Target/Child1");
                var options = new GetItemOptionsParams();

                //Act
                var result =
                    mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service, options)) as StubMapped;

                //Assert
                Assert.AreEqual(result1.ID.Guid, result.Id);
            }
        }

        [Test]
        public void MapToProperty_AbsoluteQuery_ReturnsSingleResults()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child1"),
                    new Sitecore.FakeDb.DbItem("Child2")
                }
            })
            {
                var config = new SitecoreQueryConfiguration();
                config.PropertyInfo = typeof(StubClass).GetProperty("StubMapped");
                config.Query = "/sitecore/content/Target/Child1";
                config.IsRelative = false;

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

                var mapper = new SitecoreQueryMapper(null);
                mapper.Setup(new DataMapperResolverArgs(context, config));

                var source = database.GetItem("/sitecore/content/Target");
                var service = new SitecoreService(database.Database, context);

                var result1 = database.GetItem("/sitecore/content/Target/Child1");
                var options = new GetItemOptionsParams();

                //Act
                var result =
                    mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service, options)) as StubMapped;

                //Assert
                Assert.AreEqual(result1.ID.Guid, result.Id);

            }
        }

        [Test]
        public void MapToProperty_RelativeQueryWithQueryContext_ReturnsResults()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child1"),
                    new Sitecore.FakeDb.DbItem("Child2")
                }
            })
            {
                var config = new SitecoreQueryConfiguration();
                config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");
                config.Query = "../Target/*";
                config.IsRelative = true;

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

                var mapper = new SitecoreQueryMapper(null);
                mapper.Setup(new DataMapperResolverArgs(context, config));

                var source = database.GetItem("/sitecore/content/Target");
                var service = new SitecoreService(database.Database, context);

                var result1 = database.GetItem("/sitecore/content/Target/Child1");
                var result2 = database.GetItem("/sitecore/content/Target/Child2");
                var options = new GetItemOptionsParams();

                //Act
                var results =
                    mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service, options)) as
                        IEnumerable<StubMapped>;

                //Assert
                Assert.AreEqual(2, results.Count());
                Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
                Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));
            }
        }

        [Test]
        public void MapToProperty_AbsoluteQueryWithQueryContext_ReturnsResults()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child1"),
                    new Sitecore.FakeDb.DbItem("Child2")
                }
            })
            {
                var config = new SitecoreQueryConfiguration();
                config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");

                config.Query = "/sitecore/content/Target/*";
                config.IsRelative = false;
                

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

                var mapper = new SitecoreQueryMapper(null);
                mapper.Setup(new DataMapperResolverArgs(context, config));

                var source = database.GetItem("/sitecore/content/Target");
                var service = new SitecoreService(database.Database, context);

                var result1 = database.GetItem("/sitecore/content/Target/Child1");
                var result2 = database.GetItem("/sitecore/content/Target/Child2");
                var options = new GetItemOptionsParams();

                //Act
                var results =
                    mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service, options)) as
                        IEnumerable<StubMapped>;

                //Assert
                Assert.AreEqual(2, results.Count());
                Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
                Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));
            }
        }

        [Test]
        [Category("LocalOnly")]
        public void MapToProperty_AbsoluteQueryWithParameter_ReturnsResults()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    },
                new Sitecore.FakeDb.DbItem("Results")
                {
                    new Sitecore.FakeDb.DbItem("Child1"),
                    new Sitecore.FakeDb.DbItem("Child2")
                }
            })
            {
                var config = new SitecoreQueryConfiguration();
                config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");
                config.Query = "{path}/../Results/*";
                config.IsRelative = false;
               

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubMapped)));

                var mapper = new SitecoreQueryMapper(new[] {new ItemPathParameter()});
                mapper.Setup(new DataMapperResolverArgs(context, config));

                var source = database.GetItem("/sitecore/content/Target");
                var service = new SitecoreService(database.Database, context);

                var result1 = database.GetItem("/sitecore/content/Results/Child1");
                var result2 = database.GetItem("/sitecore/content/Results/Child2");
                var options = new GetItemOptionsParams();

                //Act
                var results =
                    mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service, options)) as
                        IEnumerable<StubMapped>;

                //Assert
                Assert.AreEqual(2, results.Count());
                Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
                Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));

            }
        }

        #endregion

        #region Stubs

        [SitecoreType]
        public class StubMapped
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }
        }

        public class StubNotMapped { }

        public class StubClass
        {
            public IEnumerable<StubMapped> StubMappeds { get; set; }

            public IEnumerable<StubNotMapped> StubNotMappeds { get; set; }

            public StubMapped StubMapped { get; set; }
            public StubNotMapped StubNotMapped { get; set; }
        }

        #endregion
    }
}




