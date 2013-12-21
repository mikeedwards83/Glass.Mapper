using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Custom
{
    public class ChildrenCast
    {
        private readonly ISitecoreService _service;
        private readonly Item _item;
        private readonly bool _isLazy;
        private readonly bool _inferType;

        public ChildrenCast(ISitecoreService service, Item item, bool isLazy, bool inferType)
        {
            _service = service;
            _item = item;
            _isLazy = isLazy;
            _inferType = inferType;
        }

        public IEnumerable<T> As<T>() where T : class
        {
            return new LazyItemEnumerable<T>(() => _item.Children, _isLazy, _inferType, _service);
        } 
    }
}
