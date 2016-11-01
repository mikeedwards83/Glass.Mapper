using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Glass.Mapper
{
    public class ThreadData
    {
        private readonly string _key;

        private const string ItemsKey = "D4F757BB-B53C-4600-8264-AA94D7D5050A";

        [ThreadStatic] private static Hashtable _threadStack = null;

        public static Hashtable ThreadStack
        {
            get
            {
                if (_threadStack == null)
                {
                    _threadStack = new Hashtable();
                }
                return _threadStack;

            }
        }

        public static T GetClass<T>(string key) where T:class
        {

            string fullKey = ItemsKey + key;


            if (HttpContext.Current == null)
            {

                return ThreadStack[fullKey] as T;
            }

            return HttpContext.Current.Items[fullKey] as T;
        }

        public static void SetClass<T>(string key, T value) where T : class
        {
            string fullKey = ItemsKey + key;

            if (HttpContext.Current == null)
            {
                ThreadStack[fullKey] = value;
            }
            else
            {
                HttpContext.Current.Items[fullKey] = value;
            }
        }

        public static T GetValue<T>(string key) 
        {

            string fullKey = ItemsKey + key;

            object value = null;
            if (HttpContext.Current == null)
            {
                value = ThreadStack[fullKey];
            }
            else
            {
                value = HttpContext.Current.Items[fullKey];
            }

            return (T)(value ?? default(T));

        }

        public static void SetValue<T>(string key, T value) 
        {
            string fullKey = ItemsKey + key;

            if (HttpContext.Current == null)
            {
                ThreadStack[fullKey] = value;
            }
            else
            {
                HttpContext.Current.Items[fullKey] = value;
            }
        }

    }
}
