using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace Glass.Mapper.Umb.Integration
{
    [SetUpFixture]
    public class TestFixtureSetupTearDown
    {
        static bool _hasBooted;

        [SetUp]
        public void FixtureSetup()
        {
            if (!_hasBooted)
            {
                Global.CleanPreviousRun();

                Global.ConfigureConnectionString();

                Global.InitializeUmbraco();

                Global.ConfigureDatabase();
                _hasBooted = true;
            }
        }
    }
}
