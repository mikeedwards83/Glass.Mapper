using NUnit.Framework;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    public class AbstractMapperFixture
    {
        [SetUp]
        public void Setup()
        {
            new SecurityDisabler();
        }
    }

}
