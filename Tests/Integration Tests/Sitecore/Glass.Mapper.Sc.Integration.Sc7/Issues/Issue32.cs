using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.ContentSearch.SearchTypes;

namespace Glass.Mapper.Sc.Integration.Sc7.Issues
{
    [TestFixture]
    public class Issue32
    {

        [Test]
        public void InterfaceIssueInPageEditorWhenInterfaceInheritsFromAnInterfaceWithSimilarName()
        {
            /*
             * This test is in response to issue 53 raised on the Glass.Sitecore.Mapper
             * project. When two interfaces have similar names are created as proxies
             * the method GetTypeConfiguration returns the wrong config.
             */


            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration.Sc7"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var scContext = new SitecoreContext(db);

            var glassHtml = new GlassHtml(scContext);
            var instance = scContext.GetItem<Sc7SitecoreItem>("/sitecore");

            //Act

            //This method should execute without error


        }


        #region Stubs

      

        [SitecoreType]
        public class Sc7SitecoreItem : SearchResultItem
        {
            //public int this[string t]
            //{
            //    get
            //    {
            //        return 0;
            //    }
            //}
        }

        #endregion
    }
}
