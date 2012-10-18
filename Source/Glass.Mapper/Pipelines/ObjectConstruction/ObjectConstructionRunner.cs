using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectConstruction
{
    public class ObjectConstructionRunner : AbstractPipelineRunner<ObjectConstructionArgs, IObjectConstructionTask>
    {
        public ObjectConstructionRunner(IList<IObjectConstructionTask> tasks ):base(tasks)
        {
        }



    }
}
