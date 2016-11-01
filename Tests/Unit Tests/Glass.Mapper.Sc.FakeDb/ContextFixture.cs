using System;
using System.Linq;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Sc.FakeDb
{
    [TestFixture]
    public class ContextFixture
    {

        /// <summary>
        /// From issue https://github.com/mikeedwards83/Glass.Mapper/issues/85
        /// </summary>
        [Test]
        public void Load_LoadContextWithGenericType_CanGetTypeConfigsFromContext()
        {
            //Assign
            var type = typeof(Sample);

            //Act
            var context = Context.Create(Utilities.CreateStandardResolver());
            var loader = new AttributeConfigurationLoader("Glass.Mapper.Sc.FakeDb");
            loader.AllowedNamespaces = new[] { "Glass.Mapper.Sc.FakeDb.ContextFixture" };

            context.Load(loader);
            
            //Assert
            Assert.IsNotNull(Context.Default);
            Assert.AreEqual(Context.Contexts[Context.DefaultContextName], Context.Default);
            Assert.IsNotNull(Context.Default.TypeConfigurations[type]);
            Assert.AreEqual(3, Context.Default.TypeConfigurations[type].Properties.Count());
        }

        [Test]
        public void Load_LoadContextWithGenericTypeWithAutoMap_CanGetTypeConfigsFromContext()
        {
            //Assign
            var type = typeof(SampleAuto);

            //Act
            var context = Context.Create(Utilities.CreateStandardResolver());
            var loader = new AttributeConfigurationLoader("Glass.Mapper.Sc.FakeDb");
            loader.AllowedNamespaces = new[] { "Glass.Mapper.Sc.FakeDb.ContextFixture" };

            context.Load(loader);

            //Assert
            Assert.IsNotNull(Context.Default);
            Assert.AreEqual(Context.Contexts[Context.DefaultContextName], Context.Default);
            Assert.IsNotNull(Context.Default.TypeConfigurations[type]);
            Assert.AreEqual(4, Context.Default.TypeConfigurations[type].Properties.Count());
        }

        #region ISSUE 85

        [SitecoreType]
        public class ItemBase
        {
            [SitecoreId]
            public virtual Guid ItemId { get; set; }
        }

        public abstract class Generic<T> : ItemBase
        {
            public T Value { get; set; }

            [SitecoreField("Text")]
            public string Text { get; set; }
        }

        [SitecoreType(TemplateId = "{76036F5E-CBCE-46D1-AF0A-4143F9B557AA}")]
        public class Sample : Generic<string>
        {
            [SitecoreField("Title")]
            public string Title { get; set; }
        }

        [SitecoreType(TemplateId = "{76036F5E-CBCE-46D1-AF0A-4143F9B557AA}", AutoMap = true)]
        public class SampleAuto : Generic<string>
        {
            [SitecoreField("Title")]
            public string Title { get; set; }
        }
        #endregion
    }
}
