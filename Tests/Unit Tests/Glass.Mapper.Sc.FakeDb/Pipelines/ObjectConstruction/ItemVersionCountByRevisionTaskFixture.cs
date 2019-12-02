using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.FakeDb.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.Pipelines.ObjectConstruction
{
    [TestFixture]
    public class ItemVersionCountByRevisionTaskFixture
    {
        //see pull request 397 https://github.com/mikeedwards83/Glass.Mapper/pull/397
        //currently this test does not work because fake DB doesn't replicate the Sitecore behaviour.
        [Test]
        public void Excute_TemplateWithEnAndDeStandardValues_ItemOnlyInEn_DoesntReturnDe()
        {
            //Arrange
            using (Db database = new Db
            {
                new DbTemplate(new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId))
                {
                    new Sitecore.FakeDb.DbField("Title")
                    {
                        { "en", "Hello!" },
                        { "de-de", "Hej!" }
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID,
                    new ID(TemplateInferredTypeTaskFixture.StubInferred.TemplateId))
            })
            {

                var path = "/sitecore/content/Target";
                var itemEn = database.GetItem(path, "en");
                var itemDe = database.GetItem(path, "de-de");

              //  itemEn.Template.CreateStandardValues();

                var nextTaskEn = new NextTask();
                var taskEn = new ItemVersionCountByRevisionTask();
                taskEn.SetNext(x=>nextTaskEn.Execute(x));

                var contextEn = new SitecoreTypeCreationContext();
                contextEn.Item = itemEn;
                var optionsEn = new GetItemByItemOptions(itemEn);
                contextEn.Options = optionsEn;
                var argsEn = new ObjectConstructionArgs(null, contextEn, null, null);
            
                //Act EN
                taskEn.Execute(argsEn);

                //Assert EN
                Assert.IsTrue(nextTaskEn.WasCalled);

                var nextTaskDe = new NextTask();
                var taskDe = new ItemVersionCountByRevisionTask();
                taskDe.SetNext(x => nextTaskDe.Execute(x));

                var contextDe = new SitecoreTypeCreationContext();
                var optionsDe = new GetItemByItemOptions(itemDe);
                contextDe.Item = itemDe;

                contextDe.Options = optionsDe;
                var argsDe = new ObjectConstructionArgs(null, contextDe, null, null);

                //Act De
                taskDe.Execute(argsDe);

                //Assert De
                Assert.IsFalse(nextTaskDe.WasCalled);
            }
        }

        public class NextTask : AbstractObjectConstructionTask
        {
            public bool WasCalled { get; set; }

            public override void Execute(ObjectConstructionArgs args)
            {
                WasCalled = true;
            }

        }

        public class MyModel
        {

        }
    }
}
