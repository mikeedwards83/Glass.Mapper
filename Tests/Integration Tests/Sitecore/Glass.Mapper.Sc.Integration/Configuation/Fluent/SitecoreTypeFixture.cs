using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration.Fluent;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.Configuation.Fluent
{
    [TestFixture]
    public class SitecoreTypeFixture
    {

        [Test]
        public void Import_ImportType_AddsPropertiesToNewType()
        {
            //Arrange
            var type1 = new SitecoreType<Stub1>();
            type1.Field(x => x.Field1);

            var type2 = new SitecoreType<Stub2>();
            type2.Field(x => x.Field2);

            //Act
            type2.Import(type1);

            //Assert
            Assert.AreEqual(2, type2.Config.Properties.Count());
            
        }

        [Test]
        public void Import_ImportTypeFieldDefinedTwice_DoesNotThrowException()
        {
            //Arrange
            var type1 = new SitecoreType<Stub1>();
            type1.Field(x => x.Field1);

            var type2 = new SitecoreType<Stub2>();
            type2.Field(x => x.Field1);
            type2.Field(x => x.Field2);

            //Act
            type2.Import(type1);

            //Assert
            Assert.AreEqual(2, type2.Config.Properties.Count());

        }


        #region Stub

        public class Stub1
        {
            public string Field1 { get; set; }
        }


        public class Stub2 : Stub1
        {
            public string Field2 { get; set; }
        }

        #endregion
    }
}
