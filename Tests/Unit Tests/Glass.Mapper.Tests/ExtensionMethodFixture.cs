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
using System.Text;
using NUnit.Framework;

namespace Glass.Mapper.Tests
{
    [TestFixture]
    public class ExtensionMethodFixture
    {
        #region Method - Formatted

        [Test]
        [Sequential]
        public void Formatted_ParametersPassedToMethod_ReturnsCorrectlyFormattedString(
            [Values("a", null, "a")] string param1,
            [Values(null, "a", "b")] string param2,
            [Values("{0}", "{1}", "{0} {1}")] string input,
            [Values("a", "a", "a b")] string expected)
        {
            //Act
            string result = input.Formatted(param1, param2);
            
            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region Method - IsNullOrEmpty

        [Test]
        [Sequential]
        public void IsNullOrEmpty_TestSetOfValues_ReturnsExpectedResult(
            [Values(null, "", "ab")] string value,
            [Values(true, true, false)] bool expected)
        {
            //Act
            var result = value.IsNullOrEmpty();

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region Method - IsNotNullOrEmpty

        [Test]
        [Sequential]
        public void IsNotNullOrEmpty_TestSetOfValues_ReturnsExpectedResult(
            [Values(null, "", "ab")] string value,
            [Values(false, false, true)] bool expected)
        {
            //Act
            var result = value.IsNotNullOrEmpty();

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

       
    }

    
}



