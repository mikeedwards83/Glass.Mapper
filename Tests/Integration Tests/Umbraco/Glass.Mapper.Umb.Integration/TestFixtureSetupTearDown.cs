using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Web;

namespace Glass.Mapper.Umb.Integration
{
    [SetUpFixture]
    public class TestFixtureSetupTearDown
    {
        static bool _hasBooted = false;
        [SetUp]
        public void FixtureSetup()
        {
            if (!_hasBooted)
            {
                Global.ConfigureConnectionString();

                Global.InitializeUmbraco();

                Global.ConfigureDatabase();
                _hasBooted = true;
            }


        }

       
    }
}

