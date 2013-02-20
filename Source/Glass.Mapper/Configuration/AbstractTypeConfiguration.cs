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
using System.Diagnostics;
using System.Reflection;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Represents the configuration for a .Net type
    /// </summary>
    [DebuggerDisplay("Type: {Type}")]
    public abstract class AbstractTypeConfiguration
    {
        private List<AbstractPropertyConfiguration> _properties; 

        /// <summary>
        /// The type this configuration represents
        /// </summary>
        public Type Type { get;  set; }

        /// <summary>
        /// A list of the properties configured on a type
        /// </summary>
        public IEnumerable<AbstractPropertyConfiguration> Properties { get { return _properties; } }

        /// <summary>
        /// A list of the constructors on a type
        /// </summary>
        public IDictionary<ConstructorInfo, Delegate> ConstructorMethods { get; set; }


        public AbstractTypeConfiguration()
        {
            _properties = new List<AbstractPropertyConfiguration>();
        }

       

        public void AddProperty(AbstractPropertyConfiguration property)
        {
            _properties.Add(property);
        }


        public void MapPropertiesToObject( object obj, IAbstractService service, AbstractTypeCreationContext context)
        {
            //create properties 
            AbstractDataMappingContext dataMappingContext = service.CreateDataMappingContext(context, obj);

            foreach (var prop in Properties)
            {
                prop.Mapper.MapCmsToProperty(dataMappingContext);
            }
        }


    }
}




