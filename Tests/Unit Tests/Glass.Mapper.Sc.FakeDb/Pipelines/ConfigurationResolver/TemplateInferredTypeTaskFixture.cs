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
            var args = new ConfigurationResolverArgs(null, null, null, null);
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
            var args = new ConfigurationResolverArgs(null, new SitecoreTypeCreationContext(), null, null);
            var task = new TemplateInferredTypeTask();
            args.AbstractTypeCreationContext.InferType = false;

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
                var args = new ConfigurationResolverArgs(context, typeContext, null, null);
                var task = new TemplateInferredTypeTask();
                typeContext.InferType = true;
                typeContext.Item = database.GetItem(path);
                typeContext.RequestedType = typeof(IBase);
                args.RequestedType = typeof(IBase);



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
                var args1 = new ConfigurationResolverArgs(context, typeContext, null, null);
                var args2 = new ConfigurationResolverArgs(context, typeContext, null, null);
                var task = new TemplateInferredTypeTask();
                typeContext.InferType = true;
                typeContext.Item = database.GetItem(path);
                typeContext.RequestedType = typeof(IBase);
                args1.RequestedType = typeof(IBase);
                args2.RequestedType = typeof(IBase);



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
    }
}
