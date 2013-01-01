using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class LazyItemEnumerable<T> : LazyEnumerable<T> where T:class
    {
        private readonly Func<IEnumerable<Item>> _getItems;
        private readonly Type _type;
        private readonly bool _isLazy;
        private readonly bool _inferType;
        private readonly ISitecoreService _service;

        public LazyItemEnumerable(
            Func<IEnumerable<Item>> getItems,
            bool isLazy,
            bool inferType,
            ISitecoreService service
            )
        {
            _getItems = getItems;
            _type = typeof(T);
            _isLazy = isLazy;
            _inferType = inferType;
            _service = service;

            GetItems = ProcessItems;
        }

        public IEnumerable<T> ProcessItems()
        {
            foreach (Item child in _getItems())
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

    }
    
}
