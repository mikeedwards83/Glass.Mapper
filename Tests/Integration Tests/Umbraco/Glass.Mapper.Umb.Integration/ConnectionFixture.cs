using NUnit.Framework;
using umbraco.cms.businesslogic;

namespace Glass.Mapper.Umb.Integration
{
    [TestFixture]
    public class ConnectionFixture : FixtureBase
    {
        [Test]
        public void ConnectionTest_ReadsDictionaryItem()
        {
            var value1 = Dictionary.DictionaryItem.hasKey("KEY");
            Assert.IsFalse(value1);

            Dictionary.DictionaryItem.addKey("KEY", "VALUE");
            
            var value2 = Dictionary.DictionaryItem.hasKey("KEY");
            Assert.IsTrue(value2);
        }
    }
}
