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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreDataMappingContext
    /// </summary>
    public class SitecoreDataMappingContext:AbstractDataMappingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreDataMappingContext"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="item">The item.</param>
        /// <param name="service">The service.</param>
        public SitecoreDataMappingContext(object obj, Item item, ISitecoreService service):base(obj)
        {

            //TODO: ME - should we assert that these are not null
            this.Item = item;
            Service = service;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>The item.</value>
        public Item Item { get; private set; }
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>The service.</value>
        public ISitecoreService Service { get; private set; }
    }
}




