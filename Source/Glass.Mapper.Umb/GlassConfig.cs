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

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;
using Glass.Mapper.Umb.DataMappers;

namespace Glass.Mapper.Umb
{
    public class GlassConfig : GlassCastleConfigBase
    {
        public override void Configure(WindsorContainer container, string contextName)
        {
            // For more on component registration read: http://docs.castleproject.org/Windsor.Registering-components-one-by-one.ashx
            container.Install(
                new DataMapperInstaller(),
                new DataMapperTasksInstaller(),
                new ConfigurationResolverTaskInstaller(),
                new ObjectionConstructionTaskInstaller(), 
                new ObjectSavingTaskInstaller()
                );
        }
    }

    /// <summary>
    /// Installs the components descended from AbstractDataMapper. These are used to map data
    /// to and from the CMS.
    /// </summary>
    public class DataMapperInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container,
                            Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {

            container.Register(
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoChildrenMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyBooleanMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldDateTimeMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldDecimalMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldDoubleMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldEnumMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldFileMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldFloatMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldGuidMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldIEnumerableMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldImageMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldIntegerMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldLinkMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldLongMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>()
                //         .ImplementedBy<UmbracoFieldNameValueCollectionMapper>()
                //         .LifestyleTransient(),
                //Component.For<AbstractDataMapper>()
                //         .ImplementedBy<UmbracoFieldNullableDateTimeMapper>()
                //         .LifestyleTransient(),
                //Component.For<AbstractDataMapper>()
                //         .ImplementedBy<UmbracoFieldNullableDoubleMapper>()
                //         .LifestyleTransient(),
                //Component.For<AbstractDataMapper>()
                //         .ImplementedBy<UmbracoFieldNullableDecimalMapper>()
                //         .LifestyleTransient(),
                //Component.For<AbstractDataMapper>()
                //         .ImplementedBy<UmbracoFieldNullableFloatMapper>()
                //         .LifestyleTransient(),
                //Component.For<AbstractDataMapper>()
                //         .ImplementedBy<UmbracoFieldNullableGuidMapper>()
                //         .LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldNullableIntMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldRulesMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldStreamMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyStringMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoFieldTypeMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoIdMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoInfoMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoItemMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoLinkedMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoParentMapper>().LifestyleTransient()
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoQueryMapper>()
                //         .DynamicParameters((k, d) =>
                //                                {
                //                                    d["parameters"] = k.ResolveAll<IUmbracoQueryParameter>();
                //                                })
                //         .LifestyleTransient()
                );
        }
    }

    /// <summary>
    /// Data Mapper Resolver Tasks -
    /// These tasks are run when Glass.Mapper tries to resolve which DataMapper should handle a given property, e.g. 
    /// </summary>
    public class DataMapperTasksInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            /// Tasks are called in the order they are specified.

            container.Register(
                Component.For<IDataMapperResolverTask>()
                         .ImplementedBy<DataMapperStandardResolverTask>()
                         .LifestyleTransient()
                );
        }
    }

    /// <summary>
    /// Configuration Resolver Tasks - These tasks are run when Glass.Mapper tries to find the configration the user has requested based on the type passsed.
    /// </summary>
    public class ConfigurationResolverTaskInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // These tasks are run when Glass.Mapper tries to find the configration the user has requested based on the type passsed, e.g. 
            // if your code contained
            //       service.GetItem<MyClass>(id) 
            // the standard resolver will return the MyClass configuration. 
            // Tasks are called in the order they are specified below.
            container.Register(
                Component.For<IConfigurationResolverTask>()
                         .ImplementedBy<ConfigurationStandardResolverTask>()
                         .LifestyleTransient()
                );
        }
    }

    /// <summary>
    /// Object Construction Tasks - These tasks are run when an a class needs to be instantiated by Glass.Mapper.
    /// </summary>
    public class ObjectionConstructionTaskInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                // Tasks are called in the order they are specified below.
                Component.For<IObjectConstructionTask>().ImplementedBy<CreateConcreteTask>().LifestyleTransient(),
                Component.For<IObjectConstructionTask>().ImplementedBy<CreateInterfaceTask>().LifestyleTransient()
                );
        }
    }

    /// <summary>
    /// Object Saving Tasks - These tasks are run when an a class needs to be saved by Glass.Mapper.
    /// </summary>
    public class ObjectSavingTaskInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Tasks are called in the order they are specified below.
            container.Register(
                Component.For<IObjectSavingTask>().ImplementedBy<StandardSavingTask>().LifestyleTransient()
                );
        }
    }
}