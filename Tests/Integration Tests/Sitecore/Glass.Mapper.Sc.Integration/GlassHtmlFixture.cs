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
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.SecurityModel;
using Sitecore.Sites;
using Sitecore.Web;
using System.Collections.Specialized;

namespace Glass.Mapper.Sc.Integration
{
    [TestFixture]
    public class GlassHtmlFixture
    {
        #region Method - Editable

        [Test]
        public void Editable_InEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);
            
            var model = service.GetItem<StubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue ;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
                new SiteInfo(
                    doc.FirstChild
                    )
                );

            siteContext.SetDisplayMode(DisplayMode.Edit);

            Sitecore.Context.Site = siteContext;

            //Act
            string result;

            using (new SecurityDisabler())
            {
                result = html.Editable(model, x => x.StringField);
            }

            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsTrue(result.Contains("scWebEditInput"));
            Console.WriteLine("result "+result);
        }

        [Test]
        public void Editable_NotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<StubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
               new SiteInfo(
                   doc.FirstChild
                   )
               );

            siteContext.SetDisplayMode(DisplayMode.Normal);

            Sitecore.Context.Site = siteContext;


            //Act
            var result = html.Editable(model, x => x.StringField);

            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsFalse(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_SimpleLambdaInEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<StubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
                new SiteInfo(
                    doc.FirstChild
                    )
                );

            siteContext.SetDisplayMode(DisplayMode.Edit);

            Sitecore.Context.Site = siteContext;

            //Act
             string result;

            using (new SecurityDisabler())
            {
                result = html.Editable(model, x => x.SubStub.StringField);
            }
            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsTrue(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_SimpleLambdaNotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<StubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
               new SiteInfo(
                   doc.FirstChild
                   )
               );

            siteContext.SetDisplayMode(DisplayMode.Normal);

            Sitecore.Context.Site = siteContext;


            //Act
            var result = html.Editable(model, x => x.SubStub.StringField);

            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsFalse(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_ComplexLambdaInEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<StubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
                new SiteInfo(
                    doc.FirstChild
                    )
                );

            siteContext.SetDisplayMode(DisplayMode.Edit);

            Sitecore.Context.Site = siteContext;

            //Act
            string result;

            using (new SecurityDisabler())
            {
                result = html.Editable(model, x => x.EnumerableSubStub.First().StringField);
            }
            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsTrue(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_InterfaceComplexLambdaNotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<StubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
               new SiteInfo(
                   doc.FirstChild
                   )
               );

            siteContext.SetDisplayMode(DisplayMode.Normal);

            Sitecore.Context.Site = siteContext;


            //Act
            var result = html.Editable(model, x => x.EnumerableSubStub.First().StringField);

            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsFalse(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }


        [Test]
        public void Editable_InterfaceInEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<IStubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
                new SiteInfo(
                    doc.FirstChild
                    )
                );

            siteContext.SetDisplayMode(DisplayMode.Edit);

            Sitecore.Context.Site = siteContext;

            //Act
             string result;

            using (new SecurityDisabler())
            {
                result = html.Editable(model, x => x.StringField);
            }
            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsTrue(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_InterfaceNotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<IStubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
               new SiteInfo(
                   doc.FirstChild
                   )
               );

            siteContext.SetDisplayMode(DisplayMode.Normal);

            Sitecore.Context.Site = siteContext;


            //Act
            var result = html.Editable(model, x => x.StringField);

            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsFalse(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_InterfaceSimpleLambdaInEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<IStubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
                new SiteInfo(
                    doc.FirstChild
                    )
                );

            siteContext.SetDisplayMode(DisplayMode.Edit);

            Sitecore.Context.Site = siteContext;

            //Act
             string result;

            using (new SecurityDisabler())
            {
                 result = html.Editable(model, x => x.SubStub.StringField);
            }
            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsTrue(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_InterfaceSimpleLambdaNotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<IStubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
               new SiteInfo(
                   doc.FirstChild
                   )
               );

            siteContext.SetDisplayMode(DisplayMode.Normal);

            Sitecore.Context.Site = siteContext;


            //Act
            var result = html.Editable(model, x => x.SubStub.StringField);

            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsFalse(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_InterfaceComplexLambdaInEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<IStubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
                new SiteInfo(
                    doc.FirstChild
                    )
                );

            siteContext.SetDisplayMode(DisplayMode.Edit);

            Sitecore.Context.Site = siteContext;

            //Act
             string result;

            using (new SecurityDisabler())
            {
                result = html.Editable(model, x => x.EnumerableSubStub.First().StringField);
            }
            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsTrue(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_ComplexLambdaNotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<IStubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
               new SiteInfo(
                   doc.FirstChild
                   )
               );

            siteContext.SetDisplayMode(DisplayMode.Normal);

            Sitecore.Context.Site = siteContext;


            //Act
             string result;

            using (new SecurityDisabler())
            {
                result = html.Editable(model, x => x.EnumerableSubStub.First().StringField);
            }
            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsFalse(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_InEditModeWithStandardOutput_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<StubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
                new SiteInfo(
                    doc.FirstChild
                    )
                );

            siteContext.SetDisplayMode(DisplayMode.Edit);

            Sitecore.Context.Site = siteContext;

            //Act

            string result;

            using (new SecurityDisabler())
            {
                 result = html.Editable(model, x => x.StringField, x => x.StringField);
            }
            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class

            Assert.IsTrue(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_NotInEditModeWithStandardOutput_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<StubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringField = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
                new SiteInfo(
                    doc.FirstChild
                    )
                );

            siteContext.SetDisplayMode(DisplayMode.Normal);

            Sitecore.Context.Site = siteContext;

            //Act
             string result;

            using (new SecurityDisabler())
            {
                result = html.Editable(model, x => x.StringField, x => x.StringField);
            }
            //Assert
            Assert.AreEqual(fieldValue, result);
            //this is the webedit class
            Assert.IsFalse(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }


        [Test]
        public void Editable_InEditModeWithStandardOutputAndFieldId_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<StubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringFieldId = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
                new SiteInfo(
                    doc.FirstChild
                    )
                );

            siteContext.SetDisplayMode(DisplayMode.Edit);

            Sitecore.Context.Site = siteContext;

            //Act
             string result;

            using (new SecurityDisabler())
            {
                result = html.Editable(model, x => x.StringFieldId, x => x.StringFieldId);
            }
            //Assert
            Assert.IsTrue(result.Contains(fieldValue));
            //this is the webedit class
            Assert.IsTrue(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }

        [Test]
        public void Editable_NotInEditModeWithStandardOutputAndFieldId_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/Tests/GlassHtml/MakeEditable/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            var model = service.GetItem<StubClass>(targetPath);

            var fieldValue = "test content field";

            model.StringFieldId = fieldValue;

            using (new SecurityDisabler())
            {
                service.Save(model);
            }

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            var siteContext = new SiteContextStub(
                new SiteInfo(
                    doc.FirstChild
                    )
                );

            siteContext.SetDisplayMode(DisplayMode.Normal);

            Sitecore.Context.Site = siteContext;

            //Act
            var result = html.Editable(model, x => x.StringFieldId, x => x.StringFieldId);

            //Assert
            Assert.AreEqual(fieldValue, result);
            //this is the webedit class
            Assert.IsFalse(result.Contains("scWebEditInput"));
            Console.WriteLine("result " + result);
        }
        #endregion


        #region RenderingParameters

        [Test]
        public void RenderingParameters_StringPassedInWithParameters_ReturnsModelWithValues()
        {
            //Arrange
            var templateId = new ID("{6C815B38-4D88-4F01-916D-8D7C6548005E}");
            var expectedNumber = 234;
            var expectedId1 = new Guid("{032B690F-5113-44C4-AEC7-A16B44382D4C}");
            var expectedId2 = new Guid("{6CF01319-0234-42C8-AEC1-FE757169F7A0}");
            var expectedFieldValue = "hello world";

            var parameters = "StringField={0}&Number={1}&Items={2}"
                .Formatted(
                    WebUtil.UrlEncode(expectedFieldValue), 
                    WebUtil.UrlEncode(expectedNumber.ToString()),
                    WebUtil.UrlEncode("{0}|{1}".Formatted(expectedId1, expectedId2)));
            
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            //Act
            var result = html.GetRenderingParameters<RenderingTest>(parameters, templateId);

            //Assert
            Assert.AreEqual(expectedNumber, result.Number);
            Assert.AreEqual(expectedFieldValue, result.StringField);
            Assert.IsTrue(result.Items.Any(x=>x.Id == expectedId1));
            Assert.IsTrue(result.Items.Any(x=>x.Id == expectedId2));
            Assert.AreEqual(2, result.Items.Count());
        }

        [Test]
        public void RenderingParameters_StringPassedInWithParametersUsingIdOnType_ReturnsModelWithValues()
        {
            //Arrange
            var expectedNumber = 234;
            var expectedId1 = new Guid("{032B690F-5113-44C4-AEC7-A16B44382D4C}");
            var expectedId2 = new Guid("{6CF01319-0234-42C8-AEC1-FE757169F7A0}");
            var expectedFieldValue = "hello world";

            var parameters = "StringField={0}&Number={1}&Items={2}"
                .Formatted(
                    WebUtil.UrlEncode(expectedFieldValue),
                    WebUtil.UrlEncode(expectedNumber.ToString()),
                    WebUtil.UrlEncode("{0}|{1}".Formatted(expectedId1, expectedId2)));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreContext(db);

            var html = new GlassHtml(service);

            //Act
            var result = html.GetRenderingParameters<RenderingTestWithAttribute>(parameters);

            //Assert
            Assert.AreEqual(expectedNumber, result.Number);
            Assert.AreEqual(expectedFieldValue, result.StringField);
            Assert.IsTrue(result.Items.Any(x => x.Id == expectedId1));
            Assert.IsTrue(result.Items.Any(x => x.Id == expectedId2));
            Assert.AreEqual(2, result.Items.Count());
        }

        #endregion


        #region Stubs

        public class RenderingTest
        {
            public virtual int Number { get; set; }
            public virtual string StringField { get; set; }
            public virtual IEnumerable<QuickInfo> Items { get; set; } 
        }
        //The template ID is the ID of the rendering parameters template
        [SitecoreType(TemplateId = "{6C815B38-4D88-4F01-916D-8D7C6548005E}", AutoMap = true)]
        public class RenderingTestWithAttribute
        {
            public virtual int Number { get; set; }
            public virtual string StringField { get; set; }
            public virtual IEnumerable<QuickInfo> Items { get; set; }
        }
        public class QuickInfo
        {
            public virtual string Name { get; set; }
            public virtual Guid Id { get; set; }
        }

        [SitecoreType]
        public class StubClass
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreField]
            public virtual string StringField { get; set; }

            [SitecoreField(FieldId = "{296D9461-09AA-48EE-ADF3-9E3812B570D2}")]
            public virtual string StringFieldId { get; set; }

            [SitecoreField]
            public virtual string DateField { get; set; }

            [SitecoreQuery("./../*", IsRelative = true)]
            public virtual StubClass SubStub { get; set; }

            [SitecoreQuery("./../*", IsRelative = true)]
            public virtual IEnumerable<StubLambdaClass> EnumerableSubStub { get; set; }
        }


        [SitecoreType]
        public interface IStubLambdaClass : IStubClass
        {
       
        }

        [SitecoreType]
        public interface IStubClass
        {
            [SitecoreId]
            Guid Id { get; set; }

            [SitecoreField]
            string StringField { get; set; }

            [SitecoreField]
            string DateField { get; set; }

            [SitecoreQuery("./../*", IsRelative = true)]
             StubClass SubStub { get; set; }

            [SitecoreQuery("./../*", IsRelative = true)]
            IEnumerable<StubLambdaClass> EnumerableSubStub { get; set; }
        }

        [SitecoreType]
        public class StubLambdaClass : StubClass
        {

        }



        public class SiteContextStub : SiteContext
        {
            public SiteContextStub(SiteInfo info) : base(info)
            {
                
            }

            public void SetDisplayMode(DisplayMode mode)
            {
                this.SetDisplayMode(mode, DisplayModeDuration.Temporary);
            }
        }

        #endregion


    }
}




