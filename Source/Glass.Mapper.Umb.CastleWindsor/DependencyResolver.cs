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
using System.Collections;
using Castle.Windsor;
using System.Collections.Generic;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Glass.Mapper.Caching;
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;

namespace Glass.Mapper.Umb.CastleWindsor
{
    /// <summary>
    /// Class DependencyResolver
    /// </summary>
    public class DependencyResolver : IDependencyResolver
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
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public IWindsorContainer Container { get; private set; }


        public Mapper.Config GetConfig()
        {
            return Container.Resolve<Mapper.Config>();
        }

        public ICacheManager GetCacheManager()
        {
            return Container.Resolve<ICacheManager>();
        }

        public IEnumerable<IDataMapperResolverTask> GetDataMapperResolverTasks()
        {
            return Container.ResolveAll<IDataMapperResolverTask>();
        }

        public IEnumerable<AbstractDataMapper> GetDataMappers()
        {
            return Container.ResolveAll<AbstractDataMapper>();
        }

        public IEnumerable<IConfigurationResolverTask> GetConfigurationResolverTasks()
        {
            return Container.ResolveAll<IConfigurationResolverTask>();
        }

        public IEnumerable<IObjectConstructionTask> GetObjectConstructionTasks()
        {
            return Container.ResolveAll<IObjectConstructionTask>();
        }

        public IEnumerable<IObjectSavingTask> GetObjectSavingTasks()
        {
            return Container.ResolveAll<IObjectSavingTask>();
        }
    }
}

