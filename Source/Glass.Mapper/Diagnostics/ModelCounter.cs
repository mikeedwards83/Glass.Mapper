using System;
using System.Collections;
using System.Diagnostics;
using System.Web;

namespace Glass.Mapper.Diagnostics
{
    public class ModelCounter
    {
        public static ModelCounter Instance
        {
            get
            {
                var counter = ThreadData.GetValue<ModelCounter>("5D73C248-829B-4672-9C55-508BAC882587");
                if (counter == null)
                {
                    counter = new ModelCounter();
                    ThreadData.SetValue("5D73C248-829B-4672-9C55-508BAC882587", counter);
                }

                return counter;
            }
        }

        public int ProxyModelsCreated { get; set; }
        public int ModelsRequested { get; set; }
        public int ModelsMapped { get; set; }
        public int CachedModels { get; set; }
        public int ConcreteModelCreated { get; set; }
        private Stopwatch Watch { get; set; }


        private ModelCounter()
        {

        }

        public void Reset()
        {
            ProxyModelsCreated = 0;
            ModelsMapped = 0;
            ModelsRequested = 0;
            CachedModels = 0;
            ConcreteModelCreated = 0;
        }

        public void Start()
        {
            if (Watch == null)
            {
                Watch = new Stopwatch();
            }

            if (!Watch.IsRunning)
            {
                Watch.Start();
            }
        }

        public void Stop()
        {
            if (Watch.IsRunning)
            {
                Watch.Stop();
            }
        }

        public long Elapsed
        {
            get { return Watch == null ? 0 : Watch.ElapsedMilliseconds; }
        }
    }

    public enum MonitorSetting
    {
        Disabled,
        Enabled
    }

    public class Monitor : IDisposable
    {
        private MonitorStack _stack;

        public Monitor() 
        {
            _stack = new MonitorStack();
            ModelCounter.Instance.Start();
        }


        public  void Dispose()
        {
            _stack.Dispose();
            _stack = null;
            if (MonitorStack.Current == MonitorSetting.Disabled)
            {
                ModelCounter.Instance.Stop();
            }
        }


        private class MonitorStack : SettingStack<MonitorSetting>
        {
            const string Key = "4BABF74B-AA89-4B82-8CD7-78DD3E317566";

            public MonitorStack() 
                : base(MonitorSetting.Enabled, Key)
            {
            }

            public static MonitorSetting Current
            {
                get { return GetCurrent(Key); }
            }
        }
    }
}
