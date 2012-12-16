using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper
{
    public class LazyEnumerable<T> : IEnumerable<T> where T : class
    {
        protected Func<IEnumerable<T>> GetItems { get; set; }

        List<T> _itemList = null;
        private bool _loaded;

        public LazyEnumerable()
        {
            
        }

        public LazyEnumerable(Func<IEnumerable<T>> getItems)
        {
            GetItems = getItems;
        }

        private void LoadItems()
        {
            Type type = typeof(T);
            _itemList = new List<T>();

            if (GetItems == null) throw new NullReferenceException("No function to return items");

            _itemList.AddRange(GetItems.Invoke());

            _loaded = true;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            if (!_loaded) LoadItems();
            return _itemList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

}
