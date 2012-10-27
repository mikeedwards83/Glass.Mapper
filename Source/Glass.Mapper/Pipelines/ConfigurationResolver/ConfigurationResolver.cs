using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ConfigurationResolver
{
    public class ConfigurationResolver : AbstractPipelineRunner<ConfigurationResolverArgs, IConfigurationResolverTask>
    {
        public ConfigurationResolver(IEnumerable<IConfigurationResolverTask> tasks ):base( tasks)
        {

        }
    }
}
