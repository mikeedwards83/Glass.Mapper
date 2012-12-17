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

        protected const string FieldName = "Field";
        protected readonly ID FieldId = new ID("{6B43481F-F129-4F53-BEEE-EA84F9B1A6D4}");


        [SetUp]
        public void Setup()
        {
            Database = Sitecore.Configuration.Factory.GetDatabase("master");
        }
    }
}
