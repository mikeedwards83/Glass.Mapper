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
    public class LazyItemEnumerable<T> : IEnumerable<T> where T:class 
    {
        private readonly Func<IEnumerable<Item>> _getItems;
        private readonly Type _type;
        private readonly bool _isLazy;
        private readonly bool _inferType;
        private readonly ISitecoreService _service;
        private Lazy<IList<T>> _lazyItemList;

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
            
            _lazyItemList = new Lazy<IList<T>>(() =>ProcessItems().ToList());
        }

        public IEnumerable<T> ProcessItems()
        {
            foreach (Item child in _getItems())
            {
                var obj = _service.CreateType(
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



