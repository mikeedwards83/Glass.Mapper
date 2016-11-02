using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Glass.Mapper
{
    public class DisableLazyLoad
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
                if (Stack.Count == 0)
                {
                    return LazyLoadSetting.Enabled;
                }
                var setting = Stack.Peek();
                return setting;
            }
        }

        public static void Push(Dictionary<string, object>  args)
        {
        
            Stack.Push(LazyLoadSetting.Disabled);
            args[Key] = LazyLoadSetting.Disabled;

            if (Stack.Count > 50)
            {
                //clear the stack to stop other problems
                throw new MapperException(Constants.Errors.ErrorLazyLoop);
            }
        }
        
        public static void Pop(Dictionary<string, object> args)
        {
            if (args.ContainsKey(Key) 
                && args[Key] != null
                && (LazyLoadSetting)args[Key] == LazyLoadSetting.Disabled)
            {
                Stack.Pop();
                args[Key] = LazyLoadSetting.Enabled;
            }
        }
    }

}
