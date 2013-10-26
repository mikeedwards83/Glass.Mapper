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
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ConfigurationResolver
{
    /// <summary>
    /// Class ConfigurationResolverArgs
    /// </summary>
    public class ConfigurationResolverArgs : AbstractPipelineArgs
    {
        public IAbstractService Service { get; set; }

        /// <summary>
        /// Gets the abstract type creation context.
        /// </summary>
        /// <value>The abstract type creation context.</value>
        public AbstractTypeCreationContext AbstractTypeCreationContext { get; private set; }
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public AbstractTypeConfiguration Result { get; set; }

        public Type RequestedType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationResolverArgs"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="abstractTypeCreationContext">The abstract type creation context.</param>
        public ConfigurationResolverArgs(Context context,  AbstractTypeCreationContext abstractTypeCreationContext, Type requestedType, IAbstractService service) :base(context)
        {
            Service = service;
            AbstractTypeCreationContext = abstractTypeCreationContext;
            RequestedType = requestedType;
        }
    }
}




