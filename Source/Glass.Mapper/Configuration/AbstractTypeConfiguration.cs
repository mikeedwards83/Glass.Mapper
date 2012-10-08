using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public IEnumerable<AbstractPropertyConfiguration> Properties { get { return _properties; } }

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

