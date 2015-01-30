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
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;

namespace Glass.Mapper.Sc.IoC
{
    /// <summary>
    /// Data Mapper Resolver Tasks -
    /// These tasks are run when Glass.Mapper tries to resolve which DataMapper should handle a given property, e.g.
    /// </summary>
    public class DataMapperTaskInstaller : IGlassInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        public List<IDependencyInstaller> Actions { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMapperTaskInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public DataMapperTaskInstaller(Config config)
        {
            Config = config;
            PopulateActions();
        }
        /// <summary>
        /// Performs the installation in the container
        /// </summary>
        protected void PopulateActions()
        {
            Actions = new List<IDependencyInstaller>
            {
                new DependencyInstaller("DataMapperStandardResolverTask", x => x.RegisterTransient<IDataMapperResolverTask, DataMapperStandardResolverTask>())
            };
        }
    }
}
