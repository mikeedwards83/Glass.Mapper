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
        private readonly GetItemOptions _getModelOptions;
        private readonly LazyLoadingHelper _lazyLoadingHelper;

        public ChildrenCast(ISitecoreService service, Item item, GetItemOptions getModelOptions, LazyLoadingHelper lazyLoadingHelper)
        {
            _service = service;
            _item = item;
            _getModelOptions = getModelOptions;
            _lazyLoadingHelper = lazyLoadingHelper;
        }

        public IEnumerable<T> As<T>() where T : class
        {
            var options = new GetItemsByFuncOptions((database) => _item.Children);
            options.Copy(_getModelOptions);
            return new LazyItemEnumerable<T>(options, _service, _lazyLoadingHelper);
        } 
    }
}
