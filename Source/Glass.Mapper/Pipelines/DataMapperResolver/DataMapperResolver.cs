using System.Collections.Generic;

namespace Glass.Mapper.Pipelines.DataMapperResolver
{
    /// <summary>
    /// Class DataMapperResolver
    /// </summary>
    public class DataMapperResolver : AbstractPipelineRunner<DataMapperResolverArgs, AbstractDataMapperResolverTask>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataMapperResolver"/> class.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        public DataMapperResolver(IEnumerable<AbstractDataMapperResolverTask> tasks)
            : base(tasks)
        {
        }
    }
}




