using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

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
            var config = new T();

            config.Type = _type;
            config.ConstructorMethods = Utilities.CreateConstructorDelegates(config.Type);
            config.AutoMap = true;

            return new[] {config};
        }
    }
}
