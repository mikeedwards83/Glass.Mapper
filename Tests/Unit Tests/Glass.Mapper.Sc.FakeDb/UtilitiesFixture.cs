/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/
//-CRE-

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

        #region GetMediaUrlOptions

        [Test]
        public void GetMediaUrlOptions_AbsolutePath()
        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.AlwaysIncludeServerUrl;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.IsTrue(result.AlwaysIncludeServerUrl,"AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_AllowStretch()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.AllowStretch;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.IsTrue(result.AllowStretch,"AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_AlwaysIncludeServerUrl()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.AlwaysIncludeServerUrl;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.IsTrue(result.AlwaysIncludeServerUrl,"AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_DisableBrowserCache()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.DisableBrowserCache;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.IsTrue(result.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_DisableMediaCache()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.DisableMediaCache;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.IsTrue(result.DisableMediaCache,  "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_IgnoreAspectRatio()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.IgnoreAspectRatio;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.IsTrue(result.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_IncludeExtension()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.RemoveExtension;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.IsFalse(result.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_LowercaseUrls()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.LowercaseUrls;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.IsTrue(result.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_Thumbnail()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.Thumbnail;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.IsTrue(result.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_UseDefaultIcon()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.UseDefaultIcon;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.IsTrue(result.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }

        [Test]
        public void GetMediaUrlOptions_UseItemPath()
        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.UseItemPath;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.IsTrue(result.UseItemPath,  "UseItemPath");

        }

        #endregion

        #region DoVersionCheck

        [Test]
        public void DoVersionCheck_DefaultValues_ReturnTrue()
        {
            //Arrange
            var config = new Config();
            IItemVersionHandler versionHandler = new ItemVersionHandler(config);

            //Act
            var result = versionHandler.VersionCountEnabled();

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void DoVersionCheck_DisableVersionCount_ReturnFalse()
        {
            //Arrange
            var config = new Config();
            config.DisableVersionCount = true;
            IItemVersionHandler versionHandler = new ItemVersionHandler(config);

            //Act
            var result = versionHandler.VersionCountEnabled();

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void DoVersionCheck_VersionCountDisabler_ReturnFalse()
        {
            //Arrange
            var config = new Config();
            config.DisableVersionCount = true;
            IItemVersionHandler versionHandler = new ItemVersionHandler(config);

            var result = true;

            //Act
            using (new VersionCountDisabler())
            {
                result = versionHandler.VersionCountEnabled();
            }

            //Assert
            Assert.IsFalse(result);
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

