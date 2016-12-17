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
using System.Reflection;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Pipelines.DataMapperResolver.Tasks
{
    /// <summary>
    /// Class DataMapperStandardResolverTask
    /// </summary>
    public class DataMapperStandardResolverTask : AbstractDataMapperResolverTask
    {
        private Dictionary<PropertyInfo, AbstractDataMapper> AttributeMappers { get; }

        public DataMapperStandardResolverTask()
        {
            Name = "DataMapperStandardResolverTask";
            AttributeMappers = new Dictionary<PropertyInfo, AbstractDataMapper>();
        }

        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <exception cref="Glass.Mapper.MapperException">Could not find data mapper to handle property {0}.Formatted(args.PropertyConfiguration)</exception>
        public override void Execute(DataMapperResolverArgs args)
        {
            if (args.Result == null)
            {
                var mapper = LoadFromDataMapperAttribute(args) ??
                             args.DataMappers.FirstOrDefault(x => x.CanHandle(args.PropertyConfiguration, args.Context));

                if(mapper == null)
                    throw new MapperException("Could not find a data mapper to handle property {0}".Formatted(args.PropertyConfiguration));

                mapper.Setup(args);
                args.Result = mapper;
            }
            base.Execute(args);
        }

        /// <summary>
        /// Loads a data mapper specifed with the <see cref="DataMapperAttribute"/> if it has been applied to the current property.
        /// </summary>
        private AbstractDataMapper LoadFromDataMapperAttribute(DataMapperResolverArgs args)
        {
            var propertyInfo = args.PropertyConfiguration.PropertyInfo;

            AbstractDataMapper mapper;
            if (AttributeMappers.TryGetValue(propertyInfo, out mapper))
                return mapper;

            var mapperType = propertyInfo.GetCustomAttribute<DataMapperAttribute>()?.DataMapperType;

            if (mapperType == null)
            {
                AttributeMappers.Add(propertyInfo, null);
                return null;
            }

            var isAbstractDataMapper = typeof (AbstractDataMapper).IsAssignableFrom(mapperType);
            if (!isAbstractDataMapper)
            {
                throw new MapperException(
                    "Specified data mapper {0} does not inherit from AbstractDataMapper. {1}".Formatted(mapperType.FullName, args.PropertyConfiguration));
            }

            // Look through registered mappers first
            mapper = args.DataMappers.FirstOrDefault(x => x.GetType() == mapperType);

            if (mapper == null)
            {
                // Create new instance using the default constructor
                var constructor = mapperType.GetConstructor(Type.EmptyTypes);
                if (constructor == null)
                {
                    throw new MapperException(
                        "Specified data mapper {0} does not have a default constructor. {1}".Formatted(mapperType.FullName, args.PropertyConfiguration));
                }

                mapper = (AbstractDataMapper) Activator.CreateInstance(mapperType);
            }

            AttributeMappers.Add(propertyInfo, mapper);

            return mapper;
        }
    }
}




