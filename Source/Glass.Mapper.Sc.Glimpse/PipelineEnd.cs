using Glass.Mapper.Profilers;
using Glimpse.Core.Message;
using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Glimpse
{
    public class PipelineEnd
    {
        public string Name { get; set; }

        public void Pipeline()
        {
        }

        public void Process(PipelineArgs args)
        {
            PipelineStart.Profiler.End(Name);                
        }
    }
}
