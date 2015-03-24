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
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.MultiInterfaceResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Pipelines.ConfigurationResolver;

namespace Glass.Mapper.Sc.IoC
{
    /// <summary>
    /// Configuration Resolver Tasks - These tasks are run when Glass.Mapper tries to find the configuration the user has requested based on the type passsed.
    /// </summary>
    public class ConfigurationResolverTaskInstaller : IDependencyInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// The actions
        /// </summary>
        public List<IDependencyRegister> Actions { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationResolverTaskInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ConfigurationResolverTaskInstaller(Config config)
        {
            Config = config;
            PopulateActions();
        }


        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        public void PopulateActions()
        {
            // These tasks are run when Glass.Mapper tries to find the configuration the user has requested based on the type passed, e.g. 
            // if your code contained
            //       service.GetItem<MyClass>(id) 
            // the standard resolver will return the MyClass configuration. 
            // Tasks are called in the order they are specified below.

            Actions = new List<IDependencyRegister>
            {
                new DependencyRegister("SitecoreItemResolverTask", x => x.RegisterTransient<IConfigurationResolverTask, SitecoreItemResolverTask>()),
                new DependencyRegister("MultiInterfaceResolverTask", x => x.RegisterTransient<IConfigurationResolverTask, MultiInterfaceResolverTask>()),
                new DependencyRegister("ConfigurationStandardResolverTask", x => x.RegisterTransient<IConfigurationResolverTask, ConfigurationStandardResolverTask>()),
                new DependencyRegister("ConfigurationOnDemandResolverTask<SitecoreTypeConfiguration>", x => x.RegisterTransient<IConfigurationResolverTask, ConfigurationOnDemandResolverTask<SitecoreTypeConfiguration>>())
            };
        }
    }
}
