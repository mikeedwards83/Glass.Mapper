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
using Glass.Mapper.Caching;
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.IoC
{
    /// <summary>
    /// Object Construction Tasks - These tasks are run when an a class needs to be instantiated by Glass.Mapper.
    /// </summary>
    public class ObjectionConstructionTaskInstaller : IDependencyInstaller
    {

        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        public List<IDependencyRegister> Actions { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectionConstructionTaskInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ObjectionConstructionTaskInstaller(Config config)
        {
            Config = config;
            PopulateActions();
        }


        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        public void PopulateActions()
        {
            Actions = new List<IDependencyRegister>
            {

                new DependencyRegister("CreateDynamicTask", x => x.RegisterTransient<IObjectConstructionTask, CreateDynamicTask>()),
                new DependencyRegister("SitecoreItemTask", x => x.RegisterTransient<IObjectConstructionTask, SitecoreItemTask>()),
                new DependencyRegister("CacheCheckTask", x => x.RegisterTransient<IObjectConstructionTask, Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck.CacheCheckTask>()),

                new DependencyRegister("EnforcedTemplateCheck", x => x.RegisterTransient<IObjectConstructionTask, EnforcedTemplateCheck>()),
            /*if (Config.UseWindsorContructor)
            {
                container.Register(
                    Component.For<IObjectConstructionTask>().ImplementedBy<WindsorConstruction>().LifestyleCustom<NoTrackLifestyleManager>() 
                    );
            }*/
                new DependencyRegister("CreateMultiInferaceTask", x => x.RegisterTransient<IObjectConstructionTask, CreateMultiInferaceTask>()),
                new DependencyRegister("CreateConcreteTask", x => x.RegisterTransient<IObjectConstructionTask, CreateConcreteTask>()),
                new DependencyRegister("CreateInterfaceTask", x => x.RegisterTransient<IObjectConstructionTask, CreateInterfaceTask>()),
                new DependencyRegister("CacheAddTask", x => x.RegisterTransient<IObjectConstructionTask, Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheAdd.CacheAddTask>()),


                new DependencyRegister("HttpCache", x => x.RegisterTransient<ICacheManager, Glass.Mapper.Caching.HttpCache>()),

            };


             
        }
    }
}
