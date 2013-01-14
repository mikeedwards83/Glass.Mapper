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
        public IDictionary<Type[], Delegate> ConstructorMethods { get; set; }


        public AbstractTypeConfiguration()
        {
            _properties = new List<AbstractPropertyConfiguration>();
        }

       

        public void AddProperty(AbstractPropertyConfiguration property)
        {
            _properties.Add(property);
        }


    }
}

