using System.Collections.Specialized;
using System.Reflection;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc.FakeDb
{
    [TestFixture]
    public class UtilitiesFixture
    {
        [Test]
        public void GetPropertyFunc_UsingPropertyInfo_ReturnsFuction(
            [Values(
                "GetPublicSetPublic",
                "GetProtectedSetProtected",
                "GetPrivateSetPrivate",
                "GetPublicSetProtected",
                "GetPublicSetPrivate",
                "GetPublicNoSet"
                )] string propertyName
        )
        {
            //Assign
            PropertyInfo info = typeof(StubClass).GetProperty(propertyName, Sc.Utilities.Flags);
            Assert.IsNotNull(info);

            //Act
            var result = Sc.Utilities.GetPropertyFunc(info);

            //Assert
            Assert.IsNotNull(result);

        }

        [Test]
        public void SetPropertyAction_UsingPropertyInfo_ReturnsFuction(
            [Values(
                "GetPublicSetPublic",
                "GetProtectedSetProtected",
                "GetPrivateSetPrivate",
                "GetPublicSetProtected",
                "GetPublicSetPrivate",
                "GetPublicNoSet"
                )] string propertyName
        )
        {
            //Assign
            PropertyInfo info = typeof(StubClass).GetProperty(propertyName, Sc.Utilities.Flags);
            Assert.IsNotNull(info);

            //Act
            var result = Sc.Utilities.SetPropertyAction(info);

            //Assert
            Assert.IsNotNull(result);

        }

        [Test]
        [Sequential]
        public void GetPropertyFuncAndSetPropertyAction_ReadWriteUsingActionAndFunction_ReturnsString(
            [Values(
                "GetPublicSetPublic",
                "GetProtectedSetProtected",
                "GetPrivateSetPrivate",
                "GetPublicSetProtected",
                "GetPublicSetPrivate",
                "GetPublicNoSet"
                )] string propertyName,
            [Values(
                "some value",
                "some value",
                "some value",
                "some value",
                "some value",
                null
                )] string expected
        )
        {
            //Assign
            PropertyInfo info = typeof(StubClass).GetProperty(propertyName, Sc.Utilities.Flags);

            var getFunc = Sc.Utilities.GetPropertyFunc(info);
            var setActi = Sc.Utilities.SetPropertyAction(info);
            var stub = new StubClass();

            //Act
            setActi(stub, expected);
            var result = getFunc(stub) as string;

            //Assert
            Assert.AreEqual(expected, result);

        }

       
        #region CreateUrlOptions

        /// <summary>
        /// This test relates to https://github.com/mikeedwards83/Glass.Mapper/issues/97
        /// </summary>
        [Test]
        [TestCase(SitecoreInfoUrlOptions.AddAspxExtension, true, false, false, false, false, false)]
        [TestCase(SitecoreInfoUrlOptions.AlwaysIncludeServerUrl, false, true, false, false, false, false)]
        [TestCase(SitecoreInfoUrlOptions.EncodeNames, false, false, true, false, false, false)]
        [TestCase(SitecoreInfoUrlOptions.ShortenUrls, false, false, false, true, false, false)]
        [TestCase(SitecoreInfoUrlOptions.SiteResolving, false, false, false, false, true, false)]
        [TestCase(SitecoreInfoUrlOptions.UseUseDisplayName, false, false, false, false, false, true)]
        public void CreateUrlOptions_SetsOptionsOnDefaultOptions(
            SitecoreInfoUrlOptions options,
            bool addAspx,
            bool includeServer,
            bool encodeNames,
            bool shorten,
            bool siteResolving,
            bool displayName
            )
        {
            //Arrange

            var defaultOptions = new UrlOptions();
            defaultOptions.AddAspxExtension = false;
            defaultOptions.AlwaysIncludeServerUrl = false;
            defaultOptions.EncodeNames = false;
            defaultOptions.ShortenUrls = false;
            defaultOptions.SiteResolving = false;
            defaultOptions.UseDisplayName = false;
            var urlOptionsResolver = new UrlOptionsResolver();


            //Act
            urlOptionsResolver.CreateUrlOptions(options, defaultOptions);

            //Assert
            Assert.AreEqual(addAspx, defaultOptions.AddAspxExtension);
            Assert.AreEqual(includeServer, defaultOptions.AlwaysIncludeServerUrl);
            Assert.AreEqual(encodeNames, defaultOptions.EncodeNames);
            Assert.AreEqual(shorten, defaultOptions.ShortenUrls);
            Assert.AreEqual(siteResolving, defaultOptions.SiteResolving);
            Assert.AreEqual(displayName, defaultOptions.UseDisplayName);



        }

        #endregion

     

        

        #region ConvertAttributes

        [Test]
        public void ConvertAttributes_NameValueCollection_ReturnsString()
        {
            //Arrange
            var collection = new NameValueCollection();
            collection["key1"] = "value1";
            collection["key2"] = "value2";
            collection["key3"] = "value3";

            var expected = "key1='value1' key2='value2' key3='value3' ";

            //Act

            var result = Sc.Utilities.ConvertAttributes(collection);


            //Assert
            Assert.AreEqual(expected,result);
        }

        /// <summary>
        /// https://github.com/mikeedwards83/Glass.Mapper/issues/272
        /// </summary>
        [Test]
        public void ConvertAttributes_NameValueCollection1_ReturnsString()
        {
            //Arrange
            var collection = new NameValueCollection();
            collection["key1"] = "value1";
            collection["key2"] = "{value2}";
            collection["key3"] = "value3";

            var expected = "key1='value1' key2='{value2}' key3='value3' ";

            //Act

            var result = Sc.Utilities.ConvertAttributes(collection);


            //Assert
            Assert.AreEqual(expected, result);
        }
        #endregion

        #region Stubs

        public class StubClass
        {
            public string GetPublicSetPublic { get; set; }
            protected string GetProtectedSetProtected { get; set; }
            private string GetPrivateSetPrivate { get; set; }

            public string GetPublicSetProtected { get; protected set; }
            public string GetPublicSetPrivate { get; private set; }

            private string _getPublicNoSet;
            public string GetPublicNoSet
            {
                get { return _getPublicNoSet; }
            }
        }

        #endregion

    }
}

