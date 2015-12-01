using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Glass.Mapper
{
    public class SettingStack<T> : IDisposable 
    {
        private const string ItemsKey = "D4F757BB-B53C-4600-8264-AA94D7D5050A";

        private bool _isDisposed;

        /// <summary>
        /// This is only used when a HttpContext.Items isn't available
        /// </summary>
        [ThreadStatic] private static Hashtable ThreadStack = new Hashtable();


        public SettingStack(T newPush)
        {
            CacheStack.Push(newPush);
        }

        protected static Stack<T> NewStack()
        {

            var stack = new Stack<T>();
            stack.Push(default(T));
            return stack;
        }
        protected static Stack<T> CacheStack
        {
            get
            {
                string key = ItemsKey + typeof (T).FullName;

                if (HttpContext.Current == null)
                {
                    
                    if (ThreadStack1[key] == null)
                    {
                        ThreadStack1[key] = NewStack();
                    }

                    return ThreadStack1[key] as Stack<T>;
                }

                if (HttpContext.Current.Items[key] == null)
                {
                    HttpContext.Current.Items[key] = NewStack();
                }
                return (Stack<T>)HttpContext.Current.Items[key];
            }
        }
        public static T Current
        {
            get { return CacheStack.Peek(); }
        }

        public static Hashtable ThreadStack1
        {
            get
            {
                return ThreadStack;
            }

            set
            {
                ThreadStack = value;
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            CacheStack.Pop();
            _isDisposed = true;
        }

    }
}

