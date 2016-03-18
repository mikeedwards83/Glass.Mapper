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

namespace Glass.Mapper.Pipelines.DataMapperResolver.Tasks
{
    /// <summary>
    /// Class DataMapperStandardResolverTask
    /// </summary>
    public class DataMapperStandardResolverTask : IDataMapperResolverTask
    {
        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <exception cref="Glass.Mapper.MapperException">Could not find data mapper to handler property {0}.Formatted(args.PropertyConfiguration)</exception>
        public virtual void Execute(DataMapperResolverArgs args)
        {
            if (args.Result == null)
            {
                var mapper = args.DataMappers.FirstOrDefault(x => x.CanHandle(args.PropertyConfiguration, args.Context));
                
                if(mapper == null) 
                    throw new MapperException("Could not find a data mapper to handle property {0}".Formatted(args.PropertyConfiguration));
                
                mapper.Setup(args);
                args.Result = mapper;
            }
        }
    }
}




