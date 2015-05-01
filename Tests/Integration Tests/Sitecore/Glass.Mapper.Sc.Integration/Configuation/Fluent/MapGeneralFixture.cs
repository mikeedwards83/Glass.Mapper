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
using Castle.MicroKernel.Registration;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.Maps;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Integration.Configuation.Fluent
{
    [TestFixture]
    public class MapGeneralFixture
    {

        [Test]
        public void General_RetrieveItemAndFieldsFromSitecore_ReturnPopulatedClass()
        {
            //Assign
            string fieldValue = "test field value";
            Guid id = new Guid("{A544AE18-BC21-457D-8852-438F53AAE7E1}");
            string name = "Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");

            DependencyResolver resolver = Utilities.CreateStandardResolver() as DependencyResolver;
            var context = Context.Create(resolver);

            resolver.Container.Register(Component.For<IDependencyResolver>().Instance(resolver));

            resolver.Container.Register(
                Component.For<IGlassMap, SitecoreGlassMap<SuperStub>>()
                    .ImplementedBy<SuperStubMap>()
                    .LifestyleCustom<NoTrackLifestyleManager>());
            resolver.Container.Register(
                Component.For<IGlassMap, SitecoreGlassMap<IStub>>()
                    .ImplementedBy<StubInterfaceMap>()
                    .LifestyleCustom<NoTrackLifestyleManager>());
            resolver.Container.Register(
                Component.For<IGlassMap, SitecoreGlassMap<Stub>>()
                    .ImplementedBy<StubMap>()
                    .LifestyleCustom<NoTrackLifestyleManager>());

            var loader = new SitecoreFluentConfigurationLoader();
            
            ConfigurationMap map = new ConfigurationMap(resolver);
            map.Load(loader);
            context.Load(loader);

            var item = db.GetItem(new ID(id));

            using (new ItemEditing(item, true))
            {
                item["Field"] = fieldValue;
            }

            var service = new SitecoreService(db, context);

            //Act
            var result = service.GetItem<Stub>(id);

            //Assert
            Assert.AreEqual(fieldValue, result.Field);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual("Random String", result.DelegatedField);

        }

        [Test]
        public void General_RetrieveItemAndFieldsFromSitecore_ReturnPopulatedDerivedClass()
        {
            //Assign
            string fieldValue = "test field value";
            Guid id = new Guid("{A544AE18-BC21-457D-8852-438F53AAE7E1}");
            string name = "Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");

            DependencyResolver resolver = Utilities.CreateStandardResolver() as DependencyResolver;

            resolver.Container.Register(Component.For<IDependencyResolver>().Instance(resolver));

            resolver.Container.Register(
                Component.For<IGlassMap, SitecoreGlassMap<SuperStub>>()
                    .ImplementedBy<SuperStubMap>()
                    .LifestyleCustom<NoTrackLifestyleManager>());
            resolver.Container.Register(
                Component.For<IGlassMap, SitecoreGlassMap<IStub>>()
                    .ImplementedBy<StubInterfaceMap>()
                    .LifestyleCustom<NoTrackLifestyleManager>());
            resolver.Container.Register(
                Component.For<IGlassMap, SitecoreGlassMap<Stub>>()
                    .ImplementedBy<StubMap>()
                    .LifestyleCustom<NoTrackLifestyleManager>());

            var context = Context.Create(resolver);

            var loader = new SitecoreFluentConfigurationLoader();

            ConfigurationMap map = new ConfigurationMap(resolver);
            map.Load(loader);
            context.Load(loader);

            var item = db.GetItem(new ID(id));

            using (new ItemEditing(item, true))
            {
                item["Field"] = fieldValue;
            }

            var service = new SitecoreService(db, context);

            //Act
            var result = service.GetItem<SuperStub>(id);

            //Assert
            Assert.AreEqual(fieldValue, result.Field);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual("/en/sitecore/content/Tests/Configuration/Fluent/Target.aspx", result.Url);
            Assert.AreEqual("Random String", result.DelegatedField);

        }


        #region Stub

        public class SuperStub : Stub
        {
            public virtual string Url { get; set; }
        }

        public interface IStub
        {
            Guid Id { get; set; }
            string Field { get; set; }
            string Name { get; set; }
            string DelegatedField { get; set; }
        }

        public class Stub : IStub
        {
            public virtual Guid Id { get; set; }
            public virtual string Field { get; set; }
            public virtual string Name { get; set; }
            public string DelegatedField { get; set; }
        }

        public class SuperStubMap : SitecoreGlassMap<SuperStub>
        {
            public SuperStubMap(IDependencyResolver dependencyResolver) : base(dependencyResolver)
            {
            }

            public override void Configure()
            {
                Map(
                    x =>
                    {
                        x.Info(y => y.Url).InfoType(SitecoreInfoType.Url);
                        ImportType<Stub>();
                        // Attempts to map the same property through other interface
                        ImportType<IStub>();
                    });
            }
        }

        public class StubMap : SitecoreGlassMap<Stub>
        {
            public StubMap(IDependencyResolver dependencyResolver) : base(dependencyResolver)
            {
            }

            public override void Configure()
            {
                Map(
                    x =>
                    {
                        x.Id(y => y.Id);
                        x.Field(y => y.Field);
                        x.Info(y => y.Name).InfoType(SitecoreInfoType.Name);
                        x.Delegate(y => y.DelegatedField).GetValue(GetDelegateFieldValue);
                    });
            }

            private string GetDelegateFieldValue(SitecoreDataMappingContext arg)
            {
                return "Random String";
            }
        }

        public class StubInterfaceMap : SitecoreGlassMap<IStub>
        {
            public StubInterfaceMap(IDependencyResolver dependencyResolver)
                : base(dependencyResolver)
            {
            }

            public override void Configure()
            {
                Map(
                    x =>
                    {
                        x.Id(y => y.Id);
                        x.Field(y => y.Field);
                        x.Info(y => y.Name).InfoType(SitecoreInfoType.Name);
                        x.Delegate(y => y.DelegatedField).GetValue(GetDelegateFieldValue);
                    });

            }
            private string GetDelegateFieldValue(SitecoreDataMappingContext arg)
            {
                return "Random String";
            }
        }

        #endregion

    }
}




