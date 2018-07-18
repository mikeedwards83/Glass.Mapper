using System.Collections.Generic;

namespace Glass.Mapper.Pipelines.ObjectSaving
{
    /// <summary>
    /// Class ObjectSaving
    /// </summary>
    public class ObjectSaving : AbstractPipelineRunner<ObjectSavingArgs,AbstractObjectSavingTask>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectSaving"/> class.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        public ObjectSaving(IEnumerable<AbstractObjectSavingTask> tasks) : base(tasks)
        {
        }
    }
}




