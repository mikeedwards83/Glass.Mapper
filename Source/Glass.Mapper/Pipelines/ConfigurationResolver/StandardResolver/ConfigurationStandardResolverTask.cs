
namespace Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver
{
    /// <summary>
    /// Class ConfigurationStandardResolverTask
    /// </summary>
    public class ConfigurationStandardResolverTask : AbstractConfigurationResolverTask
    {
        public ConfigurationStandardResolverTask()
        {
            Name = "ConfigurationStandardResolverTask";
        }
        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Execute(ConfigurationResolverArgs args)
        {
            if (args.Result == null)
            {
                args.Result = args.Context[args.Options.Type];
            }
            base.Execute(args);
        }
    }
}




