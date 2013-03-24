using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver
{
    public class OnDemandLoader<T> : IConfigurationLoader where T: AbstractTypeConfiguration, new ()
    {
        private readonly Type _type;

        public OnDemandLoader(Type type)
        {
            _type = type;
        }

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
