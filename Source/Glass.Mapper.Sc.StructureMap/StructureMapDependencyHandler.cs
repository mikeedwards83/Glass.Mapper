///*
//   Copyright 2012 Michael Edwards
 
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
 
//*/
////-CRE-


//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Glass.Mapper.IoC;
//using Glass.Mapper.Sc.IoC;
//using StructureMap;
//using StructureMap.Pipeline;

//namespace Glass.Mapper.Sc.StructureMap
//{
//    /// <summary>
//    /// The dependency handler
//    /// </summary>
//    public class DependencyResolver : IDependencyHandler
//    {
//        /// <summary>
//        /// Creates the standard resolver.
//        /// </summary>
//        /// <returns>IDependencyResolver.</returns>
//        public static DependencyResolver CreateStandardResolver()
//        {
//            IContainer container = new Container();           
//            return new DependencyResolver(container);
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="DependencyResolver"/> class.
//        /// </summary>
//        /// <param name="container">The container.</param>
//        public DependencyResolver(IContainer container)
//        {
//            Container = container;
//        }

//        /// <summary>
//        /// Gets the container.
//        /// </summary>
//        /// <value>The container.</value>
//        public IContainer Container { get; private set; }

//        /// <summary>
//        /// Resolves the specified args.
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="args">The args.</param>
//        /// <returns>``0.</returns>
//        public T Resolve<T>(IDictionary<string, object> args = null)
//        {
//            ExplicitArguments explicitArguments = new ExplicitArguments(args);
//            return args == null 
//                ? Container.GetInstance<T>()
//                : Container.GetInstance<T>(explicitArguments);
//        }

//        /// <summary>
//        /// Resolves all.
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <returns>IEnumerable{``0}.</returns>
//        public IEnumerable<T> ResolveAll<T>()
//        {
//            return Container.GetAllInstances<T>();
//        }

//        public bool CanResolve(Type type)
//        {
//            return Container.TryGetInstance(type) != null;
//        }

//        /// <summary>
//        /// Registers an item in the container transiently
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <typeparam name="TComponent"></typeparam>
//        public void RegisterTransient<T, TComponent>() where T : class
//        {
//            if (Container.TryGetInstance<T>() != null)
//            {
//                Container.Configure(x => x.For<T>().Add<T>());   
//            }

//            Container.Configure(x => x.For<T>().Use<T>());
//        }

//        public void RegisterTransient(Type type)
//        {
//            if (Container.TryGetInstance(type) != null)
//            {
//                    Container.Configure(x => x.For(type).Add(type));
//            }

//                Container.Configure(x => x.For(type).Use(type));

//        }
        

//        /// <summary>
//        /// Registers an instance of an object
//        /// </summary>
//        /// <param name="instance">The instance</param>
//        /// <typeparam name="T"></typeparam>
//        public void RegisterInstance<T>(T instance) where T : class
//        {
//            Container.Configure(x => x.For<T>().Use(instance));
//        }

//        public IGlassInstaller CreateInstaller(Mapper.Config config)
//        {
//            return new StructureMapSitecoreInstaller((Mapper.Sc.Config) config, this.Container);
//        }
//    }
//}
