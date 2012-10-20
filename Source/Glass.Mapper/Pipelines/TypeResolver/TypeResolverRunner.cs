using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.TypeResolver
{
    public class TypeResolverRunner :AbstractPipelineRunner<TypeResolverArgs, ITypeResolverTask>
    {
        public TypeResolverRunner(IEnumerable<ITypeResolverTask> tasks):base(tasks)
        {
        }

    }
}
