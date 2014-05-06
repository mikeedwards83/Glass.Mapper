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
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;
using Glass.Mapper.Umb.CastleWindsor.Pipelines.ConfigurationResolver;
using Glass.Mapper.Umb.CastleWindsor.Pipelines.ObjectConstruction;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.DataMappers;

namespace Glass.Mapper.Umb.CastleWindsor
{
    /// <summary>
    /// UmbracoInstaller
    /// </summary>
    public class UmbracoInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoInstaller"/> class.
        /// </summary>
        public UmbracoInstaller() : this(new Config())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public UmbracoInstaller(Config config)
        {
            Config = config;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public virtual void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // For more on component registration read: http://docs.castleproject.org/Windsor.Registering-components-one-by-one.ashx
            container.Install(
                new DataMapperInstaller(Config),
                new DataMapperTasksInstaller(Config),
                new ConfigurationResolverTaskInstaller(Config),
                new ObjectionConstructionTaskInstaller(Config),
                new ObjectSavingTaskInstaller(Config)
                );
        }
    }

    /// <summary>
    /// Installs the components descended from AbstractDataMapper. These are used to map data
    /// to and from the CMS.
    /// </summary>
    public class DataMapperInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMapperInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public DataMapperInstaller(Config config)
        {
            Config = config;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public virtual void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoChildrenMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyBooleanMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyDateTimeMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyDecimalMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyDoubleMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyEnumMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyFileMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyFloatMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyGuidMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyIEnumerableMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyImageMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyIntegerMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyLinkMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyLongMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyNameValueCollectionMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyNullableDateTimeMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyNullableDoubleMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyNullableDecimalMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyNullableFloatMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyNullableGuidMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyNullableIntegerMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyNullableLongMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyRulesMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyStreamMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyStringMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoPropertyTypeMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoIdMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoInfoMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoItemMapper>().LifestyleTransient(),
                Component.For<AbstractDataMapper>().ImplementedBy<UmbracoParentMapper>().LifestyleTransient()//,
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoQueryMapper>()
                //         .DynamicParameters((k, d) =>
                //         {
                //             d["parameters"] = k.ResolveAll<IUmbracoQueryParameter>();
                //         })
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
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMapperTasksInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public DataMapperTasksInstaller(Config config)
        {
            Config = config;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public virtual void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Tasks are called in the order they are specified.
            container.Register(
                Component.For<IDataMapperResolverTask>()
                         .ImplementedBy<DataMapperStandardResolverTask>()
                         .LifestyleTransient()
                );
        }
    }

    /// <summary>
    /// Configuration Resolver Tasks - These tasks are run when Glass.Mapper tries to find the configuration the user has requested based on the type passsed.
    /// </summary>
    public class ConfigurationResolverTaskInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationResolverTaskInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ConfigurationResolverTaskInstaller(Config config)
        {
            Config = config;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public virtual void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // These tasks are run when Glass.Mapper tries to find the configuration the user has requested based on the type passed, e.g. 
            // if your code contained
            //       service.GetItem<MyClass>(id) 
            // the standard resolver will return the MyClass configuration. 
            // Tasks are called in the order they are specified below.
            container.Register(
                Component.For<IConfigurationResolverTask>()
                         .ImplementedBy<ConfigurationStandardResolverTask>()
                         .LifestyleTransient()
                );

            container.Register(
                Component.For<IConfigurationResolverTask>()
                         .ImplementedBy<ConfigurationOnDemandResolverTask<UmbracoTypeConfiguration>>()
                         .LifestyleTransient()
                );

            container.Register(
                Component.For<IConfigurationResolverTask>()
                            .ImplementedBy<ConfigurationInferTypeResolver>()
                            .LifestyleTransient()
                );
        }
    }

    /// <summary>
    /// Object Construction Tasks - These tasks are run when an a class needs to be instantiated by Glass.Mapper.
    /// </summary>
    public class ObjectionConstructionTaskInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectionConstructionTaskInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ObjectionConstructionTaskInstaller(Config config)
        {
            Config = config;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public virtual void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (Config.UseWindsorContructor)
            {
                container.Register(
                    Component.For<IObjectConstructionTask>().ImplementedBy<WindsorConstruction>().LifestyleTransient()
                    );
            }

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
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectSavingTaskInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ObjectSavingTaskInstaller(Config config)
        {
            Config = config;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public virtual void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Tasks are called in the order they are specified below.
            container.Register(
                Component.For<IObjectSavingTask>().ImplementedBy<StandardSavingTask>().LifestyleTransient()
                );
        }
    }
}

