using System;
using System.Collections.Generic;
using System.Linq;

namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Represents the configuration for a property on a .Net type
    /// </summary>
    public abstract class AbstractNodePropertyConfiguration:  AbstractPropertyConfiguration
    {
        private readonly bool _useGenericArgument;

        public AbstractNodePropertyConfiguration(bool useGenericArgument)
        {
            _useGenericArgument = useGenericArgument;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [infer type].
        /// </summary>
        /// <value><c>true</c> if [infer type]; otherwise, <c>false</c>.</value>
        public bool InferType { get; set; }

        /// <summary>
        /// This is the actual type that Glass will try to construct. This maybe different depending upon the
        /// property being mapped. For example "IEnumerable<T> Children" will set this value to "T";
        /// </summary>
        private Type _typeToConstruct;



        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as AbstractNodePropertyConfiguration;

            config.InferType = InferType;

            base.Copy(copy);
        }

        public override void GetPropertyOptions(GetOptions propertyOptions) 
        {

            propertyOptions.InferType = this.InferType;

            base.GetPropertyOptions(propertyOptions);

            propertyOptions.Type = GetTypeToConstruct();
        }

        public Type GetTypeToConstruct()
        {
            if (_typeToConstruct == null)
            {
                if (_useGenericArgument 
                    && PropertyInfo.PropertyType.IsGenericType 
                    && PropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    _typeToConstruct = PropertyInfo.PropertyType.GetGenericArguments().First();
                }
                else
                {
                    _typeToConstruct = PropertyInfo.PropertyType;
                }
            }
            return _typeToConstruct;
        }
    }
}




