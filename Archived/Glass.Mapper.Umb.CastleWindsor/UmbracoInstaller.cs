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
            container.Register(
                Component.For<Config>().Instance(Config).Named("UmbConfig"),
                Component.For<ILog>().ImplementedBy<NullLogger>(),
                Component.For<Mapper.Config>().Instance(Config)
                );

            #region DataMapper

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
             Component.For<AbstractDataMapper>().ImplementedBy<UmbracoDelegateMapper>().LifestyleTransient(),
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoItemMapper>().LifestyleTransient(),
             Component.For<AbstractDataMapper>().ImplementedBy<UmbracoParentMapper>().LifestyleTransient()//,
                //Component.For<AbstractDataMapper>().ImplementedBy<UmbracoQueryMapper>()
                //         .DynamicParameters((k, d) =>
                //         {
                //             d["parameters"] = k.ResolveAll<IUmbracoQueryParameter>();
                //         })
                //         .LifestyleTransient()
             );

            #endregion

            #region DataMapperResolvers

            container.Register(
                Component.For<IDataMapperResolverTask>()
                         .ImplementedBy<DataMapperStandardResolverTask>()
                         .LifestyleTransient()
                );

            #endregion

            #region ConfigurationResolver

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

            #endregion

            #region ObjectionConstruction

            if (Config.UseIoCConstructor)
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

            #endregion

            #region ObjectSaving

            // Tasks are called in the order they are specified below.
            container.Register(
                Component.For<IObjectSavingTask>().ImplementedBy<StandardSavingTask>().LifestyleTransient()
                );

            #endregion
        }
    }
}

