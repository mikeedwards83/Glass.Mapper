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

using System.Linq;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver
{
    /// <summary>
    /// ConfigurationOnDemandResolverTask
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigurationOnDemandResolverTask<T> : IConfigurationResolverTask where T: AbstractTypeConfiguration, new ()
    {
        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public void Execute(ConfigurationResolverArgs args)
        {

            if (args.Result == null)
            {
                var loader = new OnDemandLoader<T>(args.RequestedType);
                args.Context.Load(loader);
                args.Result = args.Context[args.RequestedType];
            }

        }
    }
}

