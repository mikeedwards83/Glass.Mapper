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

        private bool _isDisposed;

        private Stack<T> _cacheStack;

        public SettingStack(T newPush, string key)
        {
            _cacheStack = ThreadData.GetClass<Stack<T>>(key);
            if (_cacheStack == null)
            {
                _cacheStack = new Stack<T>();
                ThreadData.SetClass(key, _cacheStack);
            }
            _cacheStack.Push(newPush);
        }

        protected static T GetCurrent(string key)
        {
            var cacheStack = ThreadData.GetClass<Stack<T>>(key);
            if (cacheStack == null || cacheStack.Count == 0)
            {
                return default(T);
            }
            return cacheStack.Peek();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _cacheStack.Pop();
            _isDisposed = true;
        }

    }
}

