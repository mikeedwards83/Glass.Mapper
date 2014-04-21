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
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.MultiInterfaceResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;
using Glass.Mapper.Sc.CastleWindsor.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using Glass.Mapper.Sc.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.CastleWindsor
{
    /// <summary>
    /// Class SitecoreInstaller
    /// </summary>
    public class SitecoreInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Gets or sets the data mapper installer.
        /// </summary>
        /// <value>
        /// The data mapper installer.
        /// </value>
        public IWindsorInstaller DataMapperInstaller { get; set; }

        /// <summary>
        /// Gets or sets the query parameter installer.
        /// </summary>
        /// <value>
        /// The query parameter installer.
        /// </value>
        public IWindsorInstaller QueryParameterInstaller { get; set; }

        /// <summary>
        /// Gets or sets the data mapper task installer.
        /// </summary>
        /// <value>
        /// The data mapper task installer.
        /// </value>
        public IWindsorInstaller DataMapperTaskInstaller { get; set; }

        /// <summary>
        /// Gets or sets the configuration resolver task installer.
        /// </summary>
        /// <value>
        /// The configuration resolver task installer.
        /// </value>
        public IWindsorInstaller ConfigurationResolverTaskInstaller { get; set; }

        /// <summary>
        /// Gets or sets the objection construction task installer.
        /// </summary>
        /// <value>
        /// The objection construction task installer.
        /// </value>
        public IWindsorInstaller ObjectionConstructionTaskInstaller { get; set; }

        /// <summary>
        /// Gets or sets the object saving task installer.
        /// </summary>
        /// <value>
        /// The object saving task installer.
        /// </value>
        public IWindsorInstaller ObjectSavingTaskInstaller { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInstaller"/> class.
        /// </summary>
        public SitecoreInstaller():this(new Config())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public SitecoreInstaller(Config config)
        {
            Config = config;



            DataMapperInstaller = new DataMapperInstaller(config);
            QueryParameterInstaller = new QueryParameterInstaller(config);
            DataMapperTaskInstaller = new DataMapperTaskInstaller(config);
            ConfigurationResolverTaskInstaller = new ConfigurationResolverTaskInstaller(config);
            ObjectionConstructionTaskInstaller = new ObjectionConstructionTaskInstaller(config);
            ObjectSavingTaskInstaller = new ObjectSavingTaskInstaller(config);
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
                DataMapperInstaller,
                QueryParameterInstaller,
                DataMapperTaskInstaller,
                ConfigurationResolverTaskInstaller,
                ObjectionConstructionTaskInstaller,
                ObjectSavingTaskInstaller
                );

            container.Register(
                Component.For<Glass.Mapper.Sc.Config>().Instance(Config)
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
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreIgnoreMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreChildrenCastMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreChildrenMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldBooleanMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldDateTimeMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldDecimalMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldDoubleMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldEnumMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldFileMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldFloatMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldGuidMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldHtmlEncodingMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldIEnumerableMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldImageMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldIntegerMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldLinkMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldLongMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                         .ImplementedBy<SitecoreFieldNameValueCollectionMapper>()
                         .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                         .ImplementedBy<SitecoreFieldNullableDateTimeMapper>()
                         .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                         .ImplementedBy<SitecoreFieldNullableDoubleMapper>()
                         .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                         .ImplementedBy<SitecoreFieldNullableDecimalMapper>()
                         .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                         .ImplementedBy<SitecoreFieldNullableFloatMapper>()
                         .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                         .ImplementedBy<SitecoreFieldNullableGuidMapper>()
                         .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldNullableIntMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldRulesMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldStreamMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldStringMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreFieldTypeMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreIdMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreItemMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreInfoMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreNodeMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreLinkedMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreParentMapper>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreQueryMapper>()
                         .DynamicParameters((k, d) =>
                         {
                             d["parameters"] = k.ResolveAll<ISitecoreQueryParameter>();
                         })
                         .LifestyleCustom<NoTrackLifestyleManager>()
                );
        }
    }

    /// <summary>
    /// Used by the SitecoreQueryMapper to replace placeholders in a query
    /// </summary>
    public class QueryParameterInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameterInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public QueryParameterInstaller(Config config)
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
                Component.For<ISitecoreQueryParameter>().ImplementedBy<ItemDateNowParameter>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<ISitecoreQueryParameter>().ImplementedBy<ItemEscapedPathParameter>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<ISitecoreQueryParameter>().ImplementedBy<ItemIdNoBracketsParameter>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<ISitecoreQueryParameter>().ImplementedBy<ItemIdParameter>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<ISitecoreQueryParameter>().ImplementedBy<ItemPathParameter>().LifestyleCustom<NoTrackLifestyleManager>()
                );
        }
    }

    /// <summary>
    /// Data Mapper Resolver Tasks -
    /// These tasks are run when Glass.Mapper tries to resolve which DataMapper should handle a given property, e.g.
    /// </summary>
    public class DataMapperTaskInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
         public Config Config { get; private set; }

         /// <summary>
         /// Initializes a new instance of the <see cref="DataMapperTaskInstaller"/> class.
         /// </summary>
         /// <param name="config">The config.</param>
        public DataMapperTaskInstaller(Config config)
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
                         .LifestyleCustom<NoTrackLifestyleManager>()
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
                       .ImplementedBy<SitecoreItemResolverTask>()
                       .LifestyleCustom<NoTrackLifestyleManager>()
              );
            container.Register(
              Component.For<IConfigurationResolverTask>()
                       .ImplementedBy<MultiInterfaceResolverTask>()
                       .LifestyleCustom<NoTrackLifestyleManager>()
              );
            container.Register(
               Component.For<IConfigurationResolverTask>()
                        .ImplementedBy<TemplateInferredTypeTask>()
                        .LifestyleCustom<NoTrackLifestyleManager>()
               );

            container.Register(
                Component.For<IConfigurationResolverTask>()
                         .ImplementedBy<ConfigurationStandardResolverTask>()
                         .LifestyleCustom<NoTrackLifestyleManager>()
                );

            container.Register(
                Component.For<IConfigurationResolverTask>()
                         .ImplementedBy<ConfigurationOnDemandResolverTask<SitecoreTypeConfiguration>>()
                         .LifestyleCustom<NoTrackLifestyleManager>()
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
            //dynamic must be first
            container.Register(
                Component.For<IObjectConstructionTask>().ImplementedBy<CreateDynamicTask>().LifestyleCustom<NoTrackLifestyleManager>()
                );

            container.Register(
                Component.For<IObjectConstructionTask>().ImplementedBy<SitecoreItemTask>().LifestyleCustom<NoTrackLifestyleManager>()
                );

            if (Config.UseWindsorContructor)
            {
                container.Register(
                    Component.For<IObjectConstructionTask>().ImplementedBy<WindsorConstruction>().LifestyleCustom<NoTrackLifestyleManager>() 
                    );
            }

            container.Register(
                // Tasks are called in the order they are specified below.
                Component.For<IObjectConstructionTask>().ImplementedBy<CreateMultiInferaceTask>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<IObjectConstructionTask>().ImplementedBy<CreateConcreteTask>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<IObjectConstructionTask>().ImplementedBy<CreateInterfaceTask>().LifestyleCustom<NoTrackLifestyleManager>()
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
                Component.For<IObjectSavingTask>().ImplementedBy<StandardSavingTask>().LifestyleCustom<NoTrackLifestyleManager>()
                );
        }
    }


}




