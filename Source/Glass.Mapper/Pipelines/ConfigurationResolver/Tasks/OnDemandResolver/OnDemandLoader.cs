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

