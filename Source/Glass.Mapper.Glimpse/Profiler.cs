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
        public TimelineCategoryItem TimerCategory { get; private set; }

        int _indentCounter = 0;

        Stack<TimingMessage> _messages;

        public Profiler()
            :this(new TimelineCategoryItem("Glass.Mapper", "#2fa4e7", "#04498c"))
        {
          

        }

        public Profiler(TimelineCategoryItem category):this(HttpContext.Current.Application["__GlimpseRuntime"] as GlimpseRuntime, category){
        }

        public Profiler(GlimpseRuntime runtime, TimelineCategoryItem catogory)
        {
            TimerCategory = catogory;
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

   
        public void Dispose()
        {
        }
    }
}
