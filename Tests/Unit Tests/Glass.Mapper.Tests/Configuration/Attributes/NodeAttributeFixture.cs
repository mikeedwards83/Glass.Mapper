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
using Glass.Mapper.Configuration;
using NSubstitute;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Tests.Configuration.Attributes
{
    [TestFixture]
    public class NodeAttributeFixture
    {
        [Test]
        public void Does_PNodeAttribute_Extend_AbstractPropertyAttribute()
        {
            Assert.IsTrue(typeof(AbstractPropertyAttribute).IsAssignableFrom(typeof(NodeAttribute)));
        }

        [Test]
        [TestCase("IsLazy")]
        [TestCase("Path")]
        [TestCase("Id")]
        public void Does_NodeAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(NodeAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        [Test]
        public void Does_Constructor_Set_IsLazy_True()
        {
            Assert.IsTrue(new StubNodeAttribute().IsLazy);
        }


        #region Method - Configure

        [Test]
        public void Configure_DefaultValues_ConfigContainsDefaults()
        {
            //Assign
            var attr = new StubNodeAttribute();
            var config = new NodeConfiguration();
			var propertyInfo = typeof(StubItem).GetProperty("X");

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.IsNullOrEmpty(config.Path);
            Assert.IsNullOrEmpty(config.Id);
        }

        [Test]
        public void Configure_PathHasValue_ConfigHasPathValue()
        {
            //Assign
            var attr = new StubNodeAttribute();
            var config = new NodeConfiguration();
			var propertyInfo = typeof(StubItem).GetProperty("X");
            var path = "some path";
            
            attr.Path = path;

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.AreEqual(path, config.Path);
            Assert.IsNullOrEmpty(config.Id);
        }

        [Test]
        public void Configure_IdHasValue_ConfigHasIdValue()
        {
            //Assign
            var attr = new StubNodeAttribute();
            var config = new NodeConfiguration();
			var propertyInfo = typeof(StubItem).GetProperty("X");
            var id = "some id";

            attr.Id = id;

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.AreEqual(id, config.Id);
            Assert.IsNullOrEmpty(config.Path);
        }

      

        #endregion


        #region Stubs

        private class StubNodeAttribute : NodeAttribute
        {
            public StubNodeAttribute()
            {
            }

            public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
            {
                throw new NotImplementedException();
            }
        }

		public class StubItem
		{
			public object X { get; set; }
		}

        #endregion 
    }
}



