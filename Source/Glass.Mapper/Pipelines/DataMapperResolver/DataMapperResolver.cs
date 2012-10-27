using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.DataMapperResolver
{
    public class DataMapperResolver : AbstractPipelineRunner<DataMapperResolverArgs, IDataMapperResolverTask>
    {
        public DataMapperResolver(IEnumerable<IDataMapperResolverTask> tasks)
            : base(tasks)
        {
        }
    }
}
