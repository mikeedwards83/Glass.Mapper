using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class LazyItemEnumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyItemEnumerable<T> : IEnumerable<T> where T:class 
    {
        private readonly GetItemsOptions _options;
        private ISitecoreService _service;
        private Lazy<IList<T>> _lazyItemList;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyItemEnumerable{T}"/> class.
        /// </summary>
        /// <param name="getItems">The get items.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="service">The service.</param>
        /// <param name="versionHandler"></param>
        public LazyItemEnumerable(
            GetItemsOptions options,
            ISitecoreService service,
            LazyLoadingHelper lazyLoadingHelper
            )
        {
            _options = options;
            _service = service;

            _lazyItemList = new Lazy<IList<T>>(() =>ProcessItems());

            if (!lazyLoadingHelper.IsEnabled(options))
            {
                // Force the loading of the items into the list so this occurs in the current security scope.
                var dummy = _lazyItemList.Value;
            }
        }

        /// <summary>
        /// Processes the items.
        /// </summary>
        /// <returns>IEnumerable{`0}.</returns>
        public virtual List<T> ProcessItems()
        {
            if (_service == null)
            {
                throw new NullReferenceException("SitecoreService has not been set");
            }

            var items = _options.GetItems(_service.Database);



            if (items == null)
            {
                items = new Item[] {};
            }

            items = items.ToArray();

            var results = new List<T>();

            var options = new GetItemByItemOptions();
            options.Copy(_options);

            foreach (Item child in items)
            {
                options.Item = child;
                var obj = _service.GetItem(options) as T;

                if (obj == null)
                    continue;
                
                results.Add(obj);
            }

            //release the service after full enumeration 
            _service = null;

            return results;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _lazyItemList.Value.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
}




