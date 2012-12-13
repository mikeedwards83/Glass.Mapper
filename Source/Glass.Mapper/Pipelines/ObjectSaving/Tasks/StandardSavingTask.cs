using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectSaving.Tasks
{
    public class StandardSavingTask : IObjectSavingTask
    {
        public void Execute(ObjectSavingArgs args)
        {
            var savingContext = args.SavingContext;
            AbstractDataMappingContext dataMappingContext = args.Service.CreateDataMappingContext(savingContext);

            if(savingContext.Config == null)
                throw new PipelineException("No config set, can not save object", args);

            savingContext.Config.Properties.ForEach(x => x.Mapper.MapPropertyToCms(dataMappingContext));
        }
    }
}
