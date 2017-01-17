using System;
using System.Collections.Generic;

namespace Glass.Mapper
{
    public class DisableLazyLoading : IDisposable
    {
        const string Key = "1A8CEC32-8455-41ED-A363-1C08591CB755";
        
        protected static Stack<LazyLoadSetting> Stack
        {
            get
            {
                var value = ThreadData.GetClass<Stack<LazyLoadSetting>>(Key);
                if (value == null)
                {
                    value = new Stack<LazyLoadSetting>();
                    ThreadData.SetClass(Key, value);
                }
                return value;
            }
        }

        public static LazyLoadSetting Current
        {
            get
            {
                if (Stack.Count == 0 )
                {
                    return LazyLoadSetting.Enabled;
                }
                var setting = Stack.Peek();
                return setting;
            }
        }

        public  DisableLazyLoading()
        {
            Stack.Push(LazyLoadSetting.Disabled);
        }

        public void Dispose()
        {
            Stack.Pop();
        }
    }

}