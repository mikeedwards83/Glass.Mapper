using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using umbraco.interfaces;

namespace Glass.Mapper.Umb
{
    public class LazyNodeEnumerable<T> : IEnumerable<T> where T:class
    {
        private readonly Func<IEnumerable<INode>> _getItems;
        private readonly Type _type;
        private readonly bool _isLazy;
        private readonly bool _inferType;
        private readonly IUmbracoService _service;


        private Lazy<IList<T>> _lazyItemList;

        public LazyNodeEnumerable(
            Func<IEnumerable<INode>> getItems,
            Type type,
            bool isLazy,
            bool inferType,
            IUmbracoService service
            )
        {
            _getItems = getItems;
            _type = type;
            _isLazy = isLazy;
            _inferType = inferType;
            _service = service;

            _lazyItemList = new Lazy<IList<T>>(() => ProcessItems().ToList());
        }

        public IEnumerable<T> ProcessItems()
        {
            foreach (INode child in _getItems())
            {
                var obj = _service.CreateClass(
                    _type,
                    child,
                    _isLazy,
                    _inferType) as T;

                if (obj == null)
                    continue;
                yield return obj;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _lazyItemList.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
}
