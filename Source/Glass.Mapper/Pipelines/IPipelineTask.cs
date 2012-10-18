using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines
{
    public interface IPipelineTask<T>  where T: AbstractPipelineArgs
    {
        void Execute(T args);
    }
}
