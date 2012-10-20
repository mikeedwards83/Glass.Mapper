using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.DataMapperResolver
{
    public class DataMapperResolverRunner : AbstractPipelineRunner<DataMapperResolverArgs, IDataMapperResolverTask>
    {
        public DataMapperResolverRunner(IEnumerable<IDataMapperResolverTask> tasks)
            : base(tasks)
        {
        }
    }
}
