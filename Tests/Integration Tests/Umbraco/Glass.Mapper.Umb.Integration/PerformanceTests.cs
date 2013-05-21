/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-


using System;
using System.Diagnostics;
using System.Linq;
using Glass.Mapper.Umb.CastleWindsor;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Integration
{
    [TestFixture]
    public class PerformanceTests
    {
        private string _expected;
        private Context _context;
        private bool _hasRun;
        private Stopwatch _glassWatch;
        private Stopwatch _rawWatch;
        private double _glassTotal;
        private double _rawTotal;
        private ContentService _contentService;
        private UmbracoService _service;

        [SetUp]
        public void Setup()
        {
            if (_hasRun)
                return;
            
            _hasRun = true;

            _glassWatch = new Stopwatch();
            _rawWatch= new Stopwatch();
            
            _expected = "hello world";

            _context = Context.Create(DependencyResolver.CreateStandardResolver());

            _context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));
            
            _service = new UmbracoService(_contentService, _context);

            var content = _contentService.GetById(new Guid("{2867D837-B258-4DF1-90F1-D5E849FCAF84}"));
            content.Properties["Property"].Value = _expected;
            _contentService.Save(content);
        }
        
        [Test]
        [Timeout(120000)]
        [Repeat(10000)]
        public void GetItems(
            [Values(1000
                //, 10000, 50000
                )] int count
            )
        {
            for (int i = 0; i < count; i++)
            {
                _glassWatch.Reset();
                _rawWatch.Reset();

                _rawWatch.Start();
                var rawItem = _contentService.GetById(new Guid("{2867D837-B258-4DF1-90F1-D5E849FCAF84}"));
                var value1 = rawItem.Properties["Property"].Value;
                _rawWatch.Stop();
                _rawTotal = _rawWatch.ElapsedTicks;

                _glassWatch.Start();
                var glassItem = _service.GetItem<StubClass>(new Guid("{2867D837-B258-4DF1-90F1-D5E849FCAF84}"));
                var value2 = glassItem.Property;
                _glassWatch.Stop();
                _glassTotal = _glassWatch.ElapsedTicks;
            }

            double total = _glassTotal / _rawTotal;
            Console.WriteLine("Preformance Test Count: {0} Ratio: {1}".Formatted(count, total));
        }
        
        #region Stubs

        [TestFixtureSetUp]
        public void CreateStub()
        {
            string name = "Target";
            string contentTypeAlias = "TestType";
            string contentTypeName = "Test Type";

            var unitOfWork = Global.CreateUnitOfWork();
            var repoFactory = new RepositoryFactory();
            _contentService = new ContentService(unitOfWork, repoFactory);
            var contentTypeService = new ContentTypeService(unitOfWork, repoFactory,
                                                            new ContentService(unitOfWork),
                                                            new MediaService(unitOfWork, repoFactory));
            var dataTypeService = new DataTypeService(unitOfWork, repoFactory);

            var contentType = new ContentType(-1);
            contentType.Name = contentTypeName;
            contentType.Alias = contentTypeAlias;
            contentType.Thumbnail = string.Empty;
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);
            
            var definitions = dataTypeService.GetDataTypeDefinitionByControlId(new Guid("ec15c1e5-9d90-422a-aa52-4f7622c63bea"));
            dataTypeService.Save(definitions.FirstOrDefault());
            var propertyType = new PropertyType(definitions.FirstOrDefault());
            propertyType.Alias = "Property";
            contentType.AddPropertyType(propertyType);
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

            var content = new Content(name, -1, contentType);
            content.Key = new Guid("{2867D837-B258-4DF1-90F1-D5E849FCAF84}");
            _contentService.Save(content);
        }

        [UmbracoType]
        public class StubClass
        {
            [UmbracoProperty]
            public virtual string Property { get; set; }

            [UmbracoId]
            public virtual Guid Id { get; set; }
        }

        #endregion
    }
}

