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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Glass.Mapper.Sc.CastleWindsor;

namespace Glass.Mapper.Sc.Integration
{
    public static class Utilities
    {
        public static IDependencyResolver CreateStandardResolver(bool useWindsorContainer = false, params IRegistration []  registrations)
        {
            var resolver = DependencyResolver.CreateStandardResolver();
            if (registrations != null && registrations.Any())
            {
                resolver.Container.Register(registrations);
            }
            resolver.Container.Install(new SitecoreInstaller(new Glass.Mapper.Sc.CastleWindsor.Config() { UseWindsorContructor = useWindsorContainer }));
            return resolver;
        }
    }
}

