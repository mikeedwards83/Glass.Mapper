using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration
{
    
    [TestFixture]
    public class ConnectionFixture
    {
        [Test]
        public void ConnectionTest_ReadsSitecoreItem()
        {
            //Act
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var result = db.GetItem("/sitecore");
            
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("sitecore", result.Key);

        }


    }


}
