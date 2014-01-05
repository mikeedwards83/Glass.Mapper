using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.Sc7.Issues
{

    /* ORIGINAL ISSUE:
     * 
        In one of our Sitecore projects I am using the glass mapper. Sometimes I have to load an item which contains 
        a childcollection. This child collection is marked with the [SitecoreChildren] attribute. After calling the GlassCast 
        method (item.GlassCast<MyItem>()) all children are loaded and I don't have to write some factory methods myself.

        Now I have a situation the children I already have loaded do contain a Child collection also. But after calling the glasscast 
        method these collections are not loaded. 

        It would be nice to have some possibility to add some include paths in the GlassCast method to define which child object 
        should be included in the cast also. (Just like the include paths in Entity Framework for example).

        The syntax should be something like this:
        item.GlassCast<MyItem>(Include: i => i.ChildObject.ChildCollection)
        or 
        item.GlassCast<MyItem>(Include: i => i.ChildCollection.ChildObject.ChildCollection)

        Can this currently be done in the glassmapper framework?
     * 
     * 
     * 
     * The models supplied by Sander were missing the virtual keyword for the properties meaning that castle's dynamic proxies 
     * couldn't load the class.
     * 
     */

    [TestFixture]
    public class GlassCastChildrenOfChildren
    {
        [Test]
        public void GlassCastChildrenOfChildren_RetrievesChildrenWithoutIssue()
        {

            //Arrange
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            var item = db.GetItem("/sitecore/content/Tests/Issues/GlassCastChildrenOfChildren/Target");

            //Act
            var result = item.GlassCast<StubModel>();

            //Assert
            Assert.AreEqual(2, result.Children.Count());
            Assert.AreEqual(3, result.Children.First().Children.Count());

        }

        [Test]
        public void GlassCastChildrenOfChildren_RetrievesChildrenWithoutIssueUsingSandersClasses()
        {

            //Arrange
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            var item = db.GetItem("/sitecore/content/Tests/Issues/GlassCastChildrenOfChildren/Target");

            //Act
            var result = item.GlassCast<NavigationFolder>();

            //Assert
            Assert.AreEqual(2, result.Children.Count());
            Assert.AreEqual(3, result.Children.First().Children.Count());

        }

        #region Stubs

        public class StubModel
        {
             public virtual  IEnumerable<StubModel> Children { get; set; } 
        }

        public abstract class GlassBase
        {
            [SitecoreId]
            public virtual Guid Id { get; private set; }
        }
        public class NavigationFolder : GlassBase
        {
            [SitecoreChildren]
            public virtual IEnumerable<NavigationItem> Children { get; set; }
        }
        public class NavigationItem : GlassBase
        {
            [SitecoreChildren]
            public virtual IEnumerable<NavigationItem> Children { get; set; }
        }

        #endregion
    }
}
