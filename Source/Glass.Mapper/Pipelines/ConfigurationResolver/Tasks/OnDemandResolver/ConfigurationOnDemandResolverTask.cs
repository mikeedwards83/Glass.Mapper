using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver
{
    /// <summary>
    /// ConfigurationOnDemandResolverTask
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigurationOnDemandResolverTask<T> : IConfigurationResolverTask where T: AbstractTypeConfiguration, new ()
    {
        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public void Execute(ConfigurationResolverArgs args)
        {
            if (args.Result == null)
            {
                var loader = new OnDemandLoader<T>(args.AbstractTypeCreationContext.RequestedType);
                args.Context.Load(loader);
                args.Result = args.Context[args.AbstractTypeCreationContext.RequestedType];
            }

        }
    }
}
