using Glass.Mapper.Profilers;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace Glass.Mapper.Glimpse
//{
//    public class TimelineTab : ITab, IKey
//    {
//        public string Name
//        {
//            get { return "Glass.Mapper Timeline"; }
//        }

//        public string Key
//        {
//            get { return "glimpse_timeline"; }
//        }

//        public object GetData(ITabContext context)
//        {
//            var result = new TimelineModel();

//            List< Profiler.Timing> messages = new List< Profiler.Timing>();

//            foreach (var con in Context.Contexts)
//            {

//                var profiler = con.Value.DependencyResolver.Resolve<IPerformanceProfiler>() as Profiler;

//                if (profiler != null)
//                {
//                    messages.AddRange(profiler.Timings);
//                }
//            }

//            var maxEndPoint = TimeSpan.Zero;
//            var events = new List<TimelineEventModel>();

//            foreach (var message in messages.OrderBy(x => x.Start))
//            {
//                var timelineEvent = new TimelineEventModel();
//                timelineEvent.Title = message.Key;
//                timelineEvent.Category = "Glass.Mapper";
//                timelineEvent.SubText = string.Empty;
//                timelineEvent.Duration = new TimeSpan(message.Duration);
//                timelineEvent.StartPoint = new TimeSpan(message.Start);
//             //   timelineEvent.StartTime = viewRenderMessage.StartTime;
//                var endPoint = timelineEvent.EndPoint;
//                if (endPoint > maxEndPoint)
//                {
//                    maxEndPoint = endPoint;
//                }
//            }

//            result.Events = events;
//            result.Duration = maxEndPoint;

//            return result;

//        }

//        public RuntimeEvent ExecuteOn
//        {
//            get { return RuntimeEvent.EndSessionAccess; }
//        }

//        public Type RequestContextType
//        {
//            get { return null; }
//        }
//    }
//}
