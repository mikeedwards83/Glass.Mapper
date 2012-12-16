using NUnit.Framework;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    public class AbstractMapperFixture
    {
        protected Database Database { get; set; }

        [SetUp]
        public void Setup()
        {
            Database = Sitecore.Configuration.Factory.GetDatabase("master");
        }
    }
}
