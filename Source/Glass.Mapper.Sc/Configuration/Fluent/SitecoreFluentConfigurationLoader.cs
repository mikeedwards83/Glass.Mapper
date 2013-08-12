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

using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Class SitecoreFluentConfigurationLoader
    /// </summary>
    public class SitecoreFluentConfigurationLoader : IConfigurationLoader
    {
        List<ISitecoreType> _types = new List<ISitecoreType>();

        /// <summary>
        /// Adds the specified config.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config">The config.</param>
        public void Add<T>(SitecoreType<T> config)
        {
            _types.Add(config);
        }

        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>SitecoreType{``0}.</returns>
        public SitecoreType<T> Add<T>()
        {
            var config = new SitecoreType<T>();
            _types.Add(config);
            return config;
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <returns>IEnumerable{AbstractTypeConfiguration}.</returns>
        public IEnumerable<AbstractTypeConfiguration> Load()
        {
            return _types.Select(x => x.Config);
        }
    }
}




