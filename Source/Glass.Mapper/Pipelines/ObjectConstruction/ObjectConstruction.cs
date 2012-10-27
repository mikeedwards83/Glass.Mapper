using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectConstruction
{
    public class ObjectConstruction : AbstractPipelineRunner<ObjectConstructionArgs, IObjectConstructionTask>
    {
        public ObjectConstruction(IEnumerable<IObjectConstructionTask> tasks ):base(tasks)
        {
        }



    }
}
