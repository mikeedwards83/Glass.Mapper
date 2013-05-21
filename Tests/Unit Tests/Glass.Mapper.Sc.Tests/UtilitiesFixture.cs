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
using System.Text;
using NUnit.Framework;

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

