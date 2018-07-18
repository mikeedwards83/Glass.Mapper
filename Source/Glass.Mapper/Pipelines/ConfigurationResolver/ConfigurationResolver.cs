using System.Collections.Generic;

namespace Glass.Mapper.Pipelines.ConfigurationResolver
{
    /// <summary>
    /// Class ConfigurationResolver
    /// </summary>
    public class ConfigurationResolver : AbstractPipelineRunner<ConfigurationResolverArgs, AbstractConfigurationResolverTask>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationResolver"/> class.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        public ConfigurationResolver(IEnumerable<AbstractConfigurationResolverTask> tasks ):base( tasks)
        {

        }
    }
}




