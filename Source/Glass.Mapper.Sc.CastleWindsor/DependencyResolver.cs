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

using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Glass.Mapper.Caching;
using Glass.Mapper.Maps;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using Glass.Mapper.Sc.IoC;

namespace Glass.Mapper.Sc.CastleWindsor
{
    /// <summary>
    /// The dependency handler
    /// </summary>
    public class DependencyResolver : AbstractDependencyResolver
    {
        /// <summary>
        /// Creates the standard resolver.
        /// </summary>
        /// <returns>IDependencyResolver.</returns>
        public static DependencyResolver CreateStandardResolver()
        {
            IWindsorContainer container = new WindsorContainer();

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            return new DependencyResolver(container);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public DependencyResolver(IWindsorContainer container)
        {
            Container = container;
            QueryParameterFactory = new WindsorConfigFactory<ISitecoreQueryParameter>(Container);
            DataMapperResolverFactory = new WindsorConfigFactory<IDataMapperResolverTask>(Container);
            DataMapperFactory = new WindsorConfigFactory<AbstractCommonDataMapper>(Container);
            ConfigurationResolverFactory = new WindsorConfigFactory<IConfigurationResolverTask>(Container);
            ObjectConstructionFactory = new WindsorConfigFactory<IObjectConstructionTask>(Container);
            ObjectSavingFactory = new WindsorConfigFactory<IObjectSavingTask>(Container);
            ConfigurationMapFactory = new WindsorConfigFactory<IGlassMap>(Container);
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public IWindsorContainer Container { get; private set; }

        public override Mapper.Config GetConfig()
        {
            return Container.Resolve<Mapper.Config>();
        }

        public override ICacheManager GetCacheManager()
        {
            return Container.Resolve<ICacheManager>();
        }
    }
}
