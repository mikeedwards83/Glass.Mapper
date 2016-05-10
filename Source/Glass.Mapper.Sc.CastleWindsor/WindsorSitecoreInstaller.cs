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
using Glass.Mapper.Caching;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.MultiInterfaceResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheAdd;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;
using Glass.Mapper.Sc.Caching;
using Glass.Mapper.Sc.CastleWindsor.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using Glass.Mapper.Sc.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.CastleWindsor
{
    /// <summary>
    /// The windsor specific Sitecore installer
    /// </summary>
    public class WindsorSitecoreInstaller : IWindsorInstaller
    {
        private readonly Config _config;

        public WindsorSitecoreInstaller(Config config)
        {
            _config = config;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Config>().Instance(_config).Named("ScConfig"),
                Component.For<ILog>().ImplementedBy<Log>(),
               Component.For<Mapper.Config>().Instance(_config).Named("Config"),
               Component.For<ICacheManager>()
                    .ImplementedBy<HttpCache>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
               Component.For<ICacheKeyGenerator>()
                   .ImplementedBy<CacheKeyGenerator>()
                   .LifestyleCustom<NoTrackLifestyleManager>()
               );

            #region DataMappers

            container.Register(
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreIgnoreMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreChildrenCastMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreChildrenMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldBooleanMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldDateTimeMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldDecimalMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldDoubleMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldEnumMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldFileMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldFloatMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldGuidMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldHtmlEncodingMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldIEnumerableMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldImageMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldIntegerMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldLinkMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldLongMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldNameValueCollectionMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldDictionaryMapper>()
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
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldNullableIntMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldNullableEnumMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldRulesMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldStreamMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldStringMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreFieldTypeMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreIdMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreItemMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreInfoMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreNodeMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreLinkedMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreParentMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>()
                    .ImplementedBy<SitecoreDelegateMapper>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<AbstractDataMapper>().ImplementedBy<SitecoreQueryMapper>()
                    .DynamicParameters((k, d) =>
                    {
                        d["parameters"] = k.ResolveAll<ISitecoreQueryParameter>();
                    })
                    .LifestyleCustom<NoTrackLifestyleManager>()
                );


            #endregion

            #region QueryParameters

            container.Register(
                Component.For<ISitecoreQueryParameter>()
                    .ImplementedBy<ItemDateNowParameter>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<ISitecoreQueryParameter>()
                    .ImplementedBy<ItemEscapedPathParameter>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<ISitecoreQueryParameter>()
                    .ImplementedBy<ItemIdNoBracketsParameter>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<ISitecoreQueryParameter>()
                    .ImplementedBy<ItemIdParameter>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<ISitecoreQueryParameter>()
                    .ImplementedBy<ItemPathParameter>()
                    .LifestyleCustom<NoTrackLifestyleManager>()
                );

            #endregion

            #region DataMapperTasks

            container.Register(
                Component.For<IDataMapperResolverTask>()
                    .ImplementedBy<DataMapperStandardResolverTask>()
                    .LifestyleCustom<NoTrackLifestyleManager>()
                );

            #endregion

            #region ConfigurationResolver

            container.Register(
                Component.For<IConfigurationResolverTask>()
                    .ImplementedBy<SitecoreItemResolverTask>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<IConfigurationResolverTask>()
                    .ImplementedBy<MultiInterfaceResolverTask>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<IConfigurationResolverTask>()
                    .ImplementedBy<TemplateInferredTypeTask>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<IConfigurationResolverTask>()
                    .ImplementedBy<ConfigurationStandardResolverTask>()
                    .LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<IConfigurationResolverTask>()
                    .ImplementedBy<ConfigurationOnDemandResolverTask<SitecoreTypeConfiguration>>()
                    .LifestyleCustom<NoTrackLifestyleManager>()
                );

            #endregion

            #region ObjectConstruction
            container.Register(
                Component.For<IObjectConstructionTask>().ImplementedBy<CacheCheckTask>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<IObjectConstructionTask>().ImplementedBy<CreateDynamicTask>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<IObjectConstructionTask>().ImplementedBy<SitecoreItemTask>().LifestyleCustom<NoTrackLifestyleManager>(),
                Component.For<IObjectConstructionTask>().ImplementedBy<EnforcedTemplateCheck>().LifestyleCustom<NoTrackLifestyleManager>()
            );

            if (_config.UseIoCConstructor)
            {
                container.Register(
                    Component.For<IObjectConstructionTask>().ImplementedBy<WindsorConstruction>().LifestyleCustom<NoTrackLifestyleManager>()
                    );
            }

            container.Register(
                // Tasks are called in the order they are specified below.
                Component.For<IObjectConstructionTask>().ImplementedBy<CreateMultiInferaceTask>().LifestyleTransient(),
                Component.For<IObjectConstructionTask>().ImplementedBy<CreateConcreteTask>().LifestyleTransient(),
                Component.For<IObjectConstructionTask>().ImplementedBy<CreateInterfaceTask>().LifestyleTransient(),
                Component.For<IObjectConstructionTask>().ImplementedBy<CacheAddTask>().LifestyleCustom<NoTrackLifestyleManager>()
                );

            #endregion

            #region ObjectSaving

            container.Register(
               Component.For<IObjectSavingTask>().ImplementedBy<StandardSavingTask>().LifestyleCustom<NoTrackLifestyleManager>()
               );

            #endregion
        }

    }
}
