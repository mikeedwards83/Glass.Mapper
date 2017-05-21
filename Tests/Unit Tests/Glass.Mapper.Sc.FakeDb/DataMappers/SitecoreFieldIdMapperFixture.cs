using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    public class SitecoreFieldIdMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetFieldValue_ContainsNull_ReturnsIDNull()
        {
            //Arrange
            string fieldValue = null;
            var mapper = new SitecoreFieldIdMapper();

            //Act
            var result = mapper.GetFieldValue(fieldValue, null, null);

            //Assert
            Assert.AreEqual(ID.Null, result);
            Assert.IsTrue(result is ID);
        }


        [Test]
        public void GetFieldValue_ContainsGuid_ReturnsID()
        {
            //Arrange
            string fieldValue = "F55E52AC-A555-4E4D-BCD5-78F37FB1CEB1";
            var mapper = new SitecoreFieldIdMapper();

            //Act
            var result = mapper.GetFieldValue(fieldValue, null, null);

            //Assert
            Assert.AreEqual(new ID(fieldValue), result);
            Assert.IsTrue(result is ID);
        }

        #endregion



        #region Stubs

        public class Stub
        {
            public ID Field { get; set; }
        }

        #endregion
    }
}
