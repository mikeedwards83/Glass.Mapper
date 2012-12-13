using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectSaving
{
    public class ObjectSavingArgs : AbstractPipelineArgs
    {
        public object Target { get; private set; }
        public AbstractTypeSavingContext SavingContext { get; private set; }
        public IAbstractService Service { get; private set; }

        public ObjectSavingArgs(
            Context context, 
            object target, 
            AbstractTypeSavingContext savingContext,
            IAbstractService service)
            : base(context)
        {
            Target = target;
            SavingContext = savingContext;
            Service = service;
        }
    }
}
