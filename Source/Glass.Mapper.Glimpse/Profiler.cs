using Glass.Mapper.Profilers;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace Glass.Mapper.Glimpse
{
    public class Profiler : IPerformanceProfiler, IDisposable
    {
    //    ExecutionTimer _watch;
        GlimpseRuntime _runtime;
        public static TimelineCategoryItem TimerCategory = new TimelineCategoryItem("Glass.Mapper", "#2fa4e7", "#04498c");

        int _indentCounter = 0;

        Stack<TimingMessage> _messages;

        public Profiler()
            :this(HttpContext.Current.Application["__GlimpseRuntime"] as GlimpseRuntime)
        {
          

        }

        public Profiler(GlimpseRuntime runtime)
        {
            _runtime = runtime;
            _messages = new Stack<TimingMessage>();
        }

        public void Start(string key)
        {

            var watch = _runtime.Configuration.FrameworkProvider.HttpRequestStore.Get("__GlimpseTimer") as ExecutionTimer;

            var indent = new string('-', _indentCounter * 1);

            var message = new TimingMessage()
            {
                EventName = indent + key,
                EventCategory = TimerCategory


            }.AsTimedMessage(watch.Point());


            _messages.Push(message);

            _indentCounter++;
            _runtime.Configuration.MessageBroker.Publish(message);

        }

        public void End(string key)
        {
            _indentCounter--;

            var watch = _runtime.Configuration.FrameworkProvider.HttpRequestStore.Get("__GlimpseTimer") as ExecutionTimer;
            var point = watch.Point();

            var message = _messages.Pop();

            message.Duration = new TimeSpan(point.Offset.Ticks - message.Offset.Ticks);
          

        }

        public class TimingMessage : ITimelineMessage
        {
            public Guid Id
            {
                get { throw new NotImplementedException(); }
            }

            public TimelineCategoryItem EventCategory
            {
                get;
                set;
            }

            public string EventName
            {
                get;
                set;
            }

            public string EventSubText
            {
                get;
                set;
            }

            public TimeSpan Duration
            {
                get;
                set;
            }

            public TimeSpan Offset
            {
                get;
                set;
            }

            public DateTime StartTime
            {
                get;
                set;
            }
        }

        public void Dispose()
        {
        }
    }
}
