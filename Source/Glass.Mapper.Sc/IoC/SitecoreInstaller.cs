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
using Glass.Mapper.IoC;

namespace Glass.Mapper.Sc.IoC
{
    /// <summary>
    /// Class SitecoreRegister
    /// </summary>
    public class SitecoreInstaller : IGlassInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Gets or sets the data mapper installer.
        /// </summary>
        /// <value>
        /// The data mapper installer.
        /// </value>
        public IDependencyInstaller DataMapperInstaller { get; set; }

        /// <summary>
        /// Gets or sets the query parameter installer.
        /// </summary>
        /// <value>
        /// The query parameter installer.
        /// </value>
        public IDependencyInstaller QueryParameterInstaller { get; set; }

        /// <summary>
        /// Gets or sets the data mapper task installer.
        /// </summary>
        /// <value>
        /// The data mapper task installer.
        /// </value>
        public IDependencyInstaller DataMapperTaskInstaller { get; set; }

        /// <summary>
        /// Gets or sets the configuration resolver task installer.
        /// </summary>
        /// <value>
        /// The configuration resolver task installer.
        /// </value>
        public IDependencyInstaller ConfigurationResolverTaskInstaller { get; set; }

        /// <summary>
        /// Gets or sets the objection construction task installer.
        /// </summary>
        /// <value>
        /// The objection construction task installer.
        /// </value>
        public IDependencyInstaller ObjectionConstructionTaskInstaller { get; set; }

        /// <summary>
        /// Gets or sets the object saving task installer.
        /// </summary>
        /// <value>
        /// The object saving task installer.
        /// </value>
        public IDependencyInstaller ObjectSavingTaskInstaller { get; set; }

        protected IDependencyRegistrar DependencyRegistrar { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInstaller"/> class.
        /// </summary>
        public SitecoreInstaller(IDependencyRegistrar dependencyRegistrar)
            : this(new Config(), dependencyRegistrar)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public SitecoreInstaller(Config config, IDependencyRegistrar dependencyRegistrar)
        {
            Config = config;
            DependencyRegistrar = dependencyRegistrar;

            DataMapperInstaller = new DataMapperInstaller(config);
            QueryParameterInstaller = new QueryParameterInstaller(config);
            DataMapperTaskInstaller = new DataMapperTaskInstaller(config);
            ConfigurationResolverTaskInstaller = new ConfigurationResolverTaskInstaller(config);
            ObjectionConstructionTaskInstaller = new ObjectionConstructionTaskInstaller(config);
            ObjectSavingTaskInstaller = new ObjectSavingTaskInstaller(config);
        }


        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        public virtual void Install()
        {
            ProcessGlassInstaller(DataMapperInstaller);
            ProcessGlassInstaller(QueryParameterInstaller);
            ProcessGlassInstaller(DataMapperTaskInstaller);
            ProcessGlassInstaller(ConfigurationResolverTaskInstaller);
            ProcessGlassInstaller(ObjectionConstructionTaskInstaller);
            ProcessGlassInstaller(ObjectSavingTaskInstaller);

            DependencyRegistrar.RegisterInstance(Config);
        }

        private void ProcessGlassInstaller(IDependencyInstaller dependencyInstaller)
        {
            foreach (IDependencyRegister dependencyRegister in dependencyInstaller.Actions)
            {
                if (dependencyRegister.Action == null)
                {
                    continue;
                }

                dependencyRegister.Action(DependencyRegistrar);
            }
        }
    }
}
