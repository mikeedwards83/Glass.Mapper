using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.TypeResolver.Tasks.StandardResolver
{
    public class StandardResolverTask:ITypeResolverTask
    {
        public void Execute(TypeResolverArgs args)
        {
            if(!args.InferType)
            {
                args.FinalType = args.RequestedType;
                args.AbortPipeline();
            }

        }
    }
}
