using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb
{
    [TestFixture]
    public class LazyItemEnumerableFixture
    {
        [Test]
        public void ProcessItems_ResetsOptionsTemplateIdAndEnforceTemplateForEachChild()
        {
            //Assign
            using (var database = new Db
            {
                new DbItem("TestItem")
                {
                    new DbItem("Child1"),
                    new DbItem("Child2"),
                    new DbItem("Child3")
                }
            })
            {
                var getItemOptions = new GetItemsByFuncOptions
                {
                    ItemsFunc = _ => database.GetItem("/sitecore/content/TestItem").Children.ToArray(),
                    TemplateId = null,
                    EnforceTemplate = SitecoreEnforceTemplate.Default
                };

                var receivedOptions = new List<GetItemByItemOptions>();
                var receivedTemplateIds = new List<ID>();
                var receivedEnforceTemplate = new List<SitecoreEnforceTemplate>();

                var service = Substitute.For<ISitecoreService>();
                service.GetItem(Arg.Any<GetItemByItemOptions>())
                    .ReturnsNull()
                    .AndDoes(x =>
                    {
                        var options = x.Arg<GetItemByItemOptions>();
                        receivedOptions.Add(options);
                        receivedTemplateIds.Add(options.TemplateId);
                        receivedEnforceTemplate.Add(options.EnforceTemplate);

                        // Emulate ConfigureOptionsForTypeTask overwriting TemplateId and EnforceTemplate
                        options.TemplateId = ID.NewID;
                        options.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;
                    });

                var lazyItemEnumerable = new LazyItemEnumerable<Stub>(getItemOptions, service, new LazyLoadingHelper());

                //Act
                lazyItemEnumerable.ProcessItems();

                //Assert
                Assert.AreEqual(3, receivedOptions.Count);
                foreach (var options in receivedOptions)
                {
                    // Received options should have been modified from original values
                    Assert.IsNotNull(options.TemplateId);
                    Assert.AreEqual(options.EnforceTemplate, SitecoreEnforceTemplate.TemplateAndBase);
                }

                foreach (var id in receivedTemplateIds)
                {
                    // Received values should be original value
                    Assert.IsNull(id);
                }

                foreach (var enforceTemplate in receivedEnforceTemplate)
                {
                    // Received values should be original value
                    Assert.AreEqual(enforceTemplate, SitecoreEnforceTemplate.Default);
                }
            }
        }

        public class Stub
        {

        }
    }
}
