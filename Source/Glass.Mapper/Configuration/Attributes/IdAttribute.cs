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

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Class IdAttribute
    /// </summary>
    public abstract class IdAttribute : AbstractPropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdAttribute"/> class.
        /// </summary>
        /// <param name="acceptedTypes">The accepted types.</param>
        public IdAttribute(IEnumerable<Type> acceptedTypes)
        {
            AcceptedTypes= acceptedTypes;
        }

        /// <summary>
        /// Gets the accepted types.
        /// </summary>
        /// <value>The accepted types.</value>
        protected IEnumerable<Type> AcceptedTypes { get; private set; }


        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        /// <exception cref="Glass.Mapper.Configuration.ConfigurationException">Property type {0} not supported as an ID on {1}
        ///                     .Formatted(propertyInfo.PropertyType.FullName, propertyInfo.DeclaringType.FullName)</exception>
        public virtual void Configure(System.Reflection.PropertyInfo propertyInfo, IdConfiguration config)
        {
            if(!AcceptedTypes.Any(x=>propertyInfo.PropertyType == x))
                throw new ConfigurationException("Property type {0} not supported as an ID on {1}"
                    .Formatted(propertyInfo.PropertyType.FullName, propertyInfo.DeclaringType.FullName));

            config.Type = propertyInfo.PropertyType;
            base.Configure(propertyInfo, config);
        
        }
    }
}




