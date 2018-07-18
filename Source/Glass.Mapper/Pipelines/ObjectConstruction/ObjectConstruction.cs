using System.Collections.Generic;

namespace Glass.Mapper.Pipelines.ObjectConstruction
{
    /// <summary>
    /// Class ObjectConstruction
    /// </summary>
    public class ObjectConstruction : AbstractPipelineRunner<ObjectConstructionArgs, AbstractObjectConstructionTask>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConstruction"/> class.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        public ObjectConstruction(IEnumerable<AbstractObjectConstructionTask> tasks ):base(tasks)
        {
        }



    }
}




