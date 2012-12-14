using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines
{
    public class PipelineException : ApplicationException
    {
        public AbstractPipelineArgs Args { get; private set; }

        public PipelineException(string message, AbstractPipelineArgs args)
        {
            Args = args;
        }
    }
}
