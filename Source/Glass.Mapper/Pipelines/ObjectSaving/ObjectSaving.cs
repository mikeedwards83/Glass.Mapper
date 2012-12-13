using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectSaving
{
    public class ObjectSaving : AbstractPipelineRunner<ObjectSavingArgs,IObjectSavingTask>
    {
        public ObjectSaving(IEnumerable<IObjectSavingTask> tasks) : base(tasks)
        {
        }
    }
}
