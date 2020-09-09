using System;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Pipelines.ConfigurationResolver;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.Pipelines.ConfigurationResolver
{
    [TestFixture]
    public class TemplateInferredTypeTaskFixture
    {

        #region Execute

        [Test]
        public void Execute_ResultNotNull_Returns()
        {
            //Arrange
            var args = new ConfigurationResolverArgs(null, null, null);
            var task = new TemplateInferredTypeTask();
            var expected = new SitecoreTypeConfiguration();
            args.Result = expected;

            //Act
            task.Execute(args);

            //Assert
            Assert.AreEqual(expected, args.Result);
        }

        [Test]
        public void Execute_NotInferred_ResultNull()
        {
            //Arrange
            var args = new ConfigurationResolverArgs(null, new SitecoreTypeCreationContext(), null);
            var task = new TemplateInferredTypeTask();
            args.AbstractTypeCreationContext.Options = new GetItemOptions
            {
                Type = typeof(IBase),
                InferType = false
            };

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNull(args.Result);

        }

        [Test]
        public void Execute_CreatesInferredType()
        {
            //Arrange  
            using (Db database = new Db
            {
                new DbTemplate(new ID(StubInferred.TemplateId)),
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(StubInferred.TemplateId))
            })
            {
                var context = Context.Create(FakeDb.Utilities.CreateStandardResolver());
                var path = "/sitecore/content/Target";

                context.Load(new AttributeTypeLoader(typeof(StubInferred)));


                var typeContext = new SitecoreTypeCreationContext();
                var args = new ConfigurationResolverArgs(context, typeContext, null);
                var task = new TemplateInferredTypeTask();

                typeContext.Options = new GetItemOptions
                {
                    Type = typeof(IBase),
                    InferType = true
                };
                typeContext.Item = database.GetItem(path);



                //Act
                task.Execute(args);


                //Assert
                Assert.IsNotNull(args.Result);
                Assert.AreEqual(typeof(StubInferred), args.Result.Type);
            }
        }

        [Test]
        public void Execute_SecondRequestFromCacheInferredType()
        {
            //Arrange  

            using (Db database = new Db
            {
                new DbTemplate(new ID(StubInferred.TemplateId)),
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(StubInferred.TemplateId))
            })
            {
                var context = Context.Create(FakeDb.Utilities.CreateStandardResolver());
                var path = "/sitecore/content/Target";

                context.Load(new AttributeTypeLoader(typeof(StubInferred)));


                var typeContext = new SitecoreTypeCreationContext();
                var args1 = new ConfigurationResolverArgs(context, typeContext, null);
                var args2 = new ConfigurationResolverArgs(context, typeContext, null);
                var task = new TemplateInferredTypeTask();
                typeContext.Options = new GetItemOptions
                {
                    Type = typeof(IBase),
                    InferType = true
                };

                typeContext.Item = database.GetItem(path);

                //Act
                task.Execute(args1);
                task.Execute(args2);


                //Assert
                Assert.IsNotNull(args1.Result);
                Assert.AreEqual(typeof(StubInferred), args1.Result.Type);
                //Assert
                Assert.IsNotNull(args2.Result);
                Assert.AreEqual(typeof(StubInferred), args2.Result.Type);
            }
        }



        #endregion
        #region Stubs


        public interface IBase
        {

        }

        [SitecoreType(TemplateId = StubInferred.TemplateId)]
        public class StubInferred : IBase
        {
            public const string TemplateId = "{7FC4F278-ADDA-4683-944C-554D0913CB33}";
        }

        #endregion

        #region MISC


        [Test]
        public void MiscCacheKeyComparison()
        {

            //Arrange

            var context = Context.Create(FakeDb.Utilities.CreateStandardResolver());
            var type = typeof(TemplateInferredTypeTaskFixture);
            var id = ID.NewID;


            var tuple1 = new Tuple<Context, Type, ID>(context, type, id);
            var tuple2 = new Tuple<Context, Type, ID>(context, type, id);

            Assert.AreEqual(tuple1, tuple2);
        }

        #endregion
    }
}
