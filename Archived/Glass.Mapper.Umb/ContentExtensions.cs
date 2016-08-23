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


using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb
{
    /// <summary>
    /// Class ContentExtensions
    /// </summary>
    public static class ContentExtensions
    {
        /// <summary>
        /// Casts and item to a strongly typed. .
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="content">The content.</param>
        /// <param name="service">The service.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// A strongly typed class representation of the item
        /// </returns>
        public static T GlassCast<T>(this IContent content, IUmbracoService service, bool isLazy = false, bool inferType = false) where T : class
        {
            return service.CreateType<T>(content, isLazy, inferType);
        }

        /// <summary>
        /// Casts and item to a strongly typed. Uses the default Context to load types.
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="content">The content.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// A strongly typed class representation of the item
        /// </returns>
        public static T GlassCast<T>(this IContent content, bool isLazy = false, bool inferType = false) where T : class
        {
            var service = new UmbracoService(new ContentService());
            return content.GlassCast<T>(service, isLazy, inferType);
        }

        /// <summary>
        /// Read from a class and write the data to an item. Ensure item is in editing state before calling.
        /// </summary>
        /// <typeparam name="T">The type of the class to read from.</typeparam>
        /// <param name="content">The content.</param>
        /// <param name="target">The class to read from</param>
        public static void GlassRead<T>(this IContent content, T target) where T : class
        {
            IUmbracoService service = new UmbracoService(new ContentService());
            service.WriteToItem(target, content);
        }
    }
}




