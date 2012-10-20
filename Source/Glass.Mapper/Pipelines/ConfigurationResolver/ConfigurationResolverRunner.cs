using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ConfigurationResolver
{
    public class ConfigurationResolverRunner : AbstractPipelineRunner<ConfigurationResolverArgs, IConfigurationResolverTask>
    {
        public ConfigurationResolverRunner(IEnumerable<IConfigurationResolverTask> tasks ):base( tasks)
        {

        }
    }
}
