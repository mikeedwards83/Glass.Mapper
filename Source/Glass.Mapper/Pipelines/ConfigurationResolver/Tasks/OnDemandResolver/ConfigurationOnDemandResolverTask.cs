using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver
{
    /// <summary>
    /// ConfigurationOnDemandResolverTask
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigurationOnDemandResolverTask<T> : AbstractConfigurationResolverTask where T: AbstractTypeConfiguration, new ()
    {
        public ConfigurationOnDemandResolverTask()
        {
            Name = "ConfigurationOnDemandResolverTask";
        }

        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Execute(ConfigurationResolverArgs args)
        {

            if (args.Result == null)
            {
                var loader = new OnDemandLoader<T>(args.Options.Type);
                args.Context.Load(loader);
                args.Result = args.Context[args.Options.Type];
            }
            base.Execute(args);
        }
    }
}

