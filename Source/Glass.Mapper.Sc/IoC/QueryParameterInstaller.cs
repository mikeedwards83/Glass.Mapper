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
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;

namespace Glass.Mapper.Sc.IoC
{
    /// <summary>
    /// Used by the SitecoreQueryMapper to replace placeholders in a query
    /// </summary>
    public class QueryParameterInstaller : IDependencyInstaller
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
        /// Initializes a new instance of the <see cref="QueryParameterInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public QueryParameterInstaller(Config config)
        {
            Config = config;
            PopulateActions();
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        protected void PopulateActions()
        {
            Actions = new List<IDependencyRegister>
            {
                new DependencyRegister("ItemDateNowParameter" , x => x.RegisterTransient<ISitecoreQueryParameter, ItemDateNowParameter>()),
                new DependencyRegister("ItemEscapedPathParameter", x=> x.RegisterTransient<ISitecoreQueryParameter, ItemEscapedPathParameter>()),
                new DependencyRegister("ItemIdNoBracketsParameter", x=> x.RegisterTransient<ISitecoreQueryParameter, ItemIdNoBracketsParameter>()),
                new DependencyRegister("ItemIdParameter", x=> x.RegisterTransient<ISitecoreQueryParameter, ItemIdParameter>()),
                new DependencyRegister("ItemPathParameter", x=> x.RegisterTransient<ISitecoreQueryParameter, ItemPathParameter>())
            };
        }
    }
}
