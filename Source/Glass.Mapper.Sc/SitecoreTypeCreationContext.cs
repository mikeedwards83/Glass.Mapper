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
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreTypeCreationContext
    /// </summary>
    public class SitecoreTypeCreationContext : AbstractTypeCreationContext
    {

        /// <summary>
        /// Gets or sets the templateid.
        /// </summary>
        public ID TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public Item Item { get; set; }

        /// <summary>
        /// Gets or sets the sitecore service.
        /// </summary>
        /// <value>The sitecore service.</value>
        public ISitecoreService SitecoreService { get; set; }

        public override string DataSummary()
        {
            if (Item == null)
            {
                return "Item is null";
            }
            return Item.Paths.FullPath;
        }

        public override bool CacheEnabled
        {
            get { return SitecoreService.CacheEnabled 
                    && (Sitecore.Context.Site == null || Sitecore.Context.PageMode.IsNormal); }
        }
    }
}
    





