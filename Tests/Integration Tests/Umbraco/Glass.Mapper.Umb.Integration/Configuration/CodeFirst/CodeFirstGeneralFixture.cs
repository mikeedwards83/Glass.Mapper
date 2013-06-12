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
using System.Linq;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Integration.Configuration.CodeFirst
{
	[TestFixture]
	public class CodeFirstGeneralFixture
	{
		#region [ Properties ]

		public ServiceContext ServiceContext
		{
			get { return ApplicationContext.Services; }
		}

		public ApplicationContext ApplicationContext
		{
			get { return ApplicationContext.Current; }
		}

		public IContentService ContentService
		{
			get { return ServiceContext.ContentService; }
		}

		public IContentTypeService ContentTypeService
		{
			get { return ServiceContext.ContentTypeService; }
		}

		public IDataTypeService DataTypeService
		{
			get { return ServiceContext.DataTypeService; }
		}

		#endregion

		[TestFixtureSetUp]
		public void Setup()
		{
			// applicationBase.Start(applicationBase, new EventArgs());
		}

		[Test]
		public void General_RetrieveItemsAndFieldsFromUmbraco_ReturnPopulatedClass()
		{
			const string fieldValue = "test field value";
			const string name = "Target";
			const string contentTypeAlias = "TestType";
			const string contentTypeName = "Test Type";
			const string contentTypeProperty = "TestProperty";

            var context = WindsorContainer.GetContext();
			var loader = new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration");
			context.Load(loader);

			IContentType contentType = new ContentType(-1);
			contentType.Name = contentTypeName;
			contentType.Alias = contentTypeAlias;
			contentType.Thumbnail = string.Empty;
			ContentTypeService.Save(contentType);
			Assert.Greater(contentType.Id, 0);

			var definitions = DataTypeService.GetDataTypeDefinitionByControlId(new Guid("ec15c1e5-9d90-422a-aa52-4f7622c63bea"));
			var firstDefinition = definitions.FirstOrDefault();
			DataTypeService.Save(firstDefinition);
			var propertyType = new PropertyType(firstDefinition)
				{
					Alias = contentTypeProperty
				};
			contentType.AddPropertyType(propertyType);
			ContentTypeService.Save(contentType);
			Assert.Greater(contentType.Id, 0);

			var content = new Content(name, -1, contentType);
			content.SetPropertyValue(contentTypeProperty, fieldValue);
			ContentService.Save(content);


            var umbracoService = new UmbracoService(ContentService, context);

			//Act
			var result = umbracoService.GetItem<AttributeStub>(content.Id);

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(fieldValue, result.TestProperty);
			Assert.AreEqual(content.Id, result.Id);
			Assert.AreEqual(content.Key, result.Key);
			Assert.AreEqual(name, result.Name);
			Assert.AreEqual(contentTypeName, result.ContentTypeName);
			Assert.AreEqual(content.ParentId + "," + content.Id, result.Path);
			Assert.AreEqual(contentTypeAlias, result.ContentTypeAlias);
			Assert.AreEqual(content.Version, result.Version);
		}
	}
	
	#region Stub

	[UmbracoType]
	public class AttributeStub
	{
		[UmbracoId]
		public virtual int Id { get; set; }
		[UmbracoId]
		public virtual Guid Key { get; set; }
		[UmbracoProperty]
		public virtual string TestProperty { get; set; }
		[UmbracoInfo(UmbracoInfoType.Name)]
		public virtual string Name { get; set; }
		[UmbracoInfo(UmbracoInfoType.ContentTypeName)]
		public virtual string ContentTypeName { get; set; }
		[UmbracoInfo(UmbracoInfoType.ContentTypeAlias)]
		public virtual string ContentTypeAlias { get; set; }
		[UmbracoInfo(UmbracoInfoType.Path)]
		public virtual string Path { get; set; }
		[UmbracoInfo(UmbracoInfoType.Version)]
		public virtual Guid Version { get; set; }
	}

	#endregion

}

