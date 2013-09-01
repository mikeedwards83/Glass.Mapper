using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface
{
    public class CreateMultiInferaceTask : IObjectConstructionTask
    {
        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null)
                return;

            //if (args.Configurations.First().Type.IsInterface && args.Configurations.Count() == 1)
            //{

            //    args.Result = _generator.CreateInterfaceProxyWithoutTarget(args.Configurations.First().Type, new InterfacePropertyInterceptor(args));
            //    args.AbortPipeline();
            //}
        }
    }
}
