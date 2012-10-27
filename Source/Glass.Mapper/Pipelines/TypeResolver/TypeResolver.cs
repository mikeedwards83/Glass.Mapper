using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.TypeResolver
{
    public class TypeResolver :AbstractPipelineRunner<TypeResolverArgs, ITypeResolverTask>
    {
        public TypeResolver(IEnumerable<ITypeResolverTask> tasks):base(tasks)
        {
        }

    }
}
