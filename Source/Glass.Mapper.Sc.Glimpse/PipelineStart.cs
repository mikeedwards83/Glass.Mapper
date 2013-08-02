using Glass.Mapper.Profilers;
using Glimpse.Core.Message;
using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Glimpse
{
    public class PipelineStart
    {

        private static string _profilerKey = "{81815A96-F53A-42B7-AAD0-BC7FA72050F8}";

        public static IPerformanceProfiler Profiler
        {
            get
            {
                if (Sitecore.Context.Items[_profilerKey] == null)
                    Sitecore.Context.Items[_profilerKey] = new Mapper.Glimpse.Profiler(TimelineCategory);

                return Sitecore.Context.Items[_profilerKey] as IPerformanceProfiler;
            }
        }

        public static  TimelineCategoryItem TimelineCategory = new TimelineCategoryItem("Sitecore", "#ff0000", "#ff0000");

        public string Name { get; set; }

        public void Pipeline()
        {

        }

        public void Process(PipelineArgs args)
        {
       
            Profiler.Start(Name);                
        }

        
    }
}
