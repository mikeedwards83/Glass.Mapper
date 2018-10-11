using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.FakeDb
{
    [TestFixture]
    public class GetOptionsScFixture
    {
        #region Copy

        [Test]
        public void Copy_OnlyCopiesGraphProperties()
        {
            //Arrange

            var options = new GetOptionsSc();
            options.TemplateId = ID.NewID;
            options.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;
            options.VersionCount = true;

            options.Cache = Cache.Disabled;
            options.Type = this.GetType();
            options.Lazy = LazyLoading.Enabled;
            options.InferType = true;

            var copy = new GetOptionsSc();

            //Act
            copy.Copy(options);
            
            //Assert
            Assert.AreEqual(options.VersionCount, copy.VersionCount);
            Assert.AreEqual(options.InferType, copy.InferType);
            Assert.AreEqual(options.Lazy, copy.Lazy);
            Assert.AreEqual(options.Type, copy.Type);
            Assert.AreEqual(options.Cache, copy.Cache);

            Assert.AreNotEqual(options.TemplateId, copy.TemplateId);
            Assert.IsNull(copy.TemplateId);

            Assert.AreEqual(SitecoreEnforceTemplate.Default, copy.EnforceTemplate);


        }

        #endregion

    }
}
