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
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver
{
    /// <summary>
    /// OnDemandLoader
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OnDemandLoader<T> : IConfigurationLoader where T: AbstractTypeConfiguration, new ()
    {
        private readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnDemandLoader{T}"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public OnDemandLoader(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <returns>
        /// IEnumerable{AbstractTypeConfiguration}.
        /// </returns>
        public IEnumerable<AbstractTypeConfiguration> Load()
        {

            var loader = new AttributeTypeLoader(_type);
            var config = loader.Load().FirstOrDefault();

            if (config == null)
            {
                config = new T();
                config.Type = _type;
                config.ConstructorMethods = Utilities.CreateConstructorDelegates(config.Type);
                config.AutoMap = true;
            }

            return new[] {config};

        }


    }
}

