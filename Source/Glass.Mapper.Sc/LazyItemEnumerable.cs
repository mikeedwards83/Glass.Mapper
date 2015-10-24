/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

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
        private readonly Func<IEnumerable<Item>> _getItems;
        private readonly Type _type;
        private readonly bool _isLazy;
        private readonly bool _inferType;
        private ISitecoreService _service;
        private Lazy<IList<T>> _lazyItemList;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyItemEnumerable{T}"/> class.
        /// </summary>
        /// <param name="getItems">The get items.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="service">The service.</param>
        public LazyItemEnumerable(
            Func<IEnumerable<Item>> getItems,
            bool isLazy,
            bool inferType,
            ISitecoreService service
            )
        {
            _getItems = getItems;
            _type = typeof(T);
            _isLazy = service.Config.UseProxiesForLazyEnumerables &&  isLazy;
            _inferType = inferType;
            _service = service;
            
            _lazyItemList = new Lazy<IList<T>>(() =>ProcessItems());

            if (isLazy == false)
            {
                // Force the loading of the items into the list so this occurs in the current security scope.
                var dummy = _lazyItemList.Value;
            }
        }

        /// <summary>
        /// Processes the items.
        /// </summary>
        /// <returns>IEnumerable{`0}.</returns>
        public List<T> ProcessItems()
        {
            if (_service == null)
            {
                throw new NullReferenceException("SitecoreService has not been set");
            }

            var items = _getItems();

            if (items == null)
            {
                items = new Item[] {};
            }

            items = items.ToArray();

            var results = new List<T>();

           
            foreach (Item child in items)
            {
                var obj = _service.CreateType(
                    _type,
                    child,
                    _isLazy,
                    _inferType, null) as T;

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




