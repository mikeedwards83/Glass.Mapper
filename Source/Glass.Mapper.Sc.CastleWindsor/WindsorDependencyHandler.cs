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
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Glass.Mapper.IoC;
using Glass.Mapper.Sc.IoC;

namespace Glass.Mapper.Sc.CastleWindsor
{
    /// <summary>
    /// The dependency handler
    /// </summary>
    public class DependencyResolver : IDependencyHandler
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

        /// <summary>
        /// Resolves the specified args.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args">The args.</param>
        /// <returns>``0.</returns>
        public T Resolve<T>(IDictionary<string, object> args = null)
        {
            return args == null 
                ? Container.Resolve<T>() 
                : Container.Resolve<T>((IDictionary) args);
        }

        /// <summary>
        /// Resolves all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IEnumerable{``0}.</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return Container.ResolveAll<T>();
        }

        public bool CanResolve(Type type)
        {
            return Container.Kernel.HasComponent(type);
        }

        /// <summary>
        /// Registers an item in the container transiently
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        public void RegisterTransient<T, TComponent>() where T : class
        {
            Container.Register(Component.For<T, TComponent>().LifestyleCustom<NoTrackLifestyleManager>());
        }

        public void RegisterTransient(Type type)
        {
            Container.Register(Component.For(type).LifestyleCustom<NoTrackLifestyleManager>());
        }



        /// <summary>
        /// Registers an instance of an object
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <typeparam name="T"></typeparam>
        public void RegisterInstance<T>(T instance) where T : class
        {
            Container.Register(Component.For<T>().Instance(instance));
        }

        public IGlassInstaller CreateInstaller(Mapper.Config config)
        {
            return new WindsorSitecoreInstaller((Mapper.Sc.Config)config, this.Container );
        }

    }
}
