using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Tests.DataMappers
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
