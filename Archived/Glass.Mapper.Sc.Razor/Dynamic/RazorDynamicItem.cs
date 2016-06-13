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
using Glass.Mapper.Sc.Dynamic;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Razor.Dynamic
{
    /// <summary>
    /// Class RazorDynamicItem
    /// </summary>
    public class RazorDynamicItem : DynamicItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicItem" /> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public RazorDynamicItem(Item item):base(item)
        {
        }
        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="item">The item.</param>
        /// <returns>System.Object.</returns>
        protected override object GetField(string fieldName, global::Sitecore.Data.Items.Item item)
        {
            return new DynamicField(fieldName, item);
        }

        /// <summary>
        /// Creates the new.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>DynamicItem.</returns>
        protected override DynamicItem CreateNew(Item item)
        {
            return new RazorDynamicItem(item);
        }
    }
}

