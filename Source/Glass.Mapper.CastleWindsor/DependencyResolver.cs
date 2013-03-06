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

using System.Collections;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System.Collections.Generic;

namespace Glass.Mapper.CastleWindsor
{
    public class DependencyResolver : IDependencyResolver
    {
        public static IDependencyResolver CreateStandardResolver()
        {
            IWindsorContainer container=  new WindsorContainer();
            container.Install(new SitecoreInstaller());
            return new DependencyResolver(container);
        }

        public DependencyResolver(IWindsorContainer container)
        {
            Container = container;
        }
        public IWindsorContainer Container { get; private set; }

        public T Resolve<T>(IDictionary<string, object> args = null)
        {
            if (args == null)
                return Container.Resolve<T>();


            return Container.Resolve<T>((IDictionary) args);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return Container.ResolveAll<T>();
        }
    }
}



