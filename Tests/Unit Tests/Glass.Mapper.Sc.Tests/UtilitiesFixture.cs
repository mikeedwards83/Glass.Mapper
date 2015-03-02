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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Sitecore.Links;

namespace Glass.Mapper.Sc.Tests
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
            PropertyInfo info = typeof(StubClass).GetProperty(propertyName, Utilities.Flags);
            Assert.IsNotNull(info);

            //Act
            var result = Utilities.GetPropertyFunc(info);

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
            PropertyInfo info = typeof(StubClass).GetProperty(propertyName, Utilities.Flags);
            Assert.IsNotNull(info);

            //Act
            var result = Utilities.SetPropertyAction(info);

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
            PropertyInfo info = typeof(StubClass).GetProperty(propertyName, Utilities.Flags);

            var getFunc = Utilities.GetPropertyFunc(info);
            var setActi = Utilities.SetPropertyAction(info);
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


            //Act
            Utilities.CreateUrlOptions(options, defaultOptions);

            //Assert
            Assert.AreEqual(addAspx, defaultOptions.AddAspxExtension);
            Assert.AreEqual(includeServer, defaultOptions.AlwaysIncludeServerUrl);
            Assert.AreEqual(encodeNames, defaultOptions.EncodeNames);
            Assert.AreEqual(shorten, defaultOptions.ShortenUrls);
            Assert.AreEqual(siteResolving, defaultOptions.SiteResolving);
            Assert.AreEqual(displayName, defaultOptions.UseDisplayName);



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

