using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public static class ItemExtensions
    {
        /// <summary>
        /// Casts and item to a stongly typed. Does not infertype or lazy load.
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="item">Item to read from</param>
        /// <returns>A strongly type class representation of the item</returns>
        public static T GlassCast<T>(this Item item, ISitecoreService service, bool isLazy = false, bool inferType = false) where T : class
        {
            return service.CreateType<T>(item, isLazy, inferType);
        }

        /// <summary>
        /// Read from a class and write the data to an item. Ensure item is in editing state before calling.
        /// </summary>
        /// <typeparam name="T">The type of the class to read from.</typeparam>
        /// <param name="item">The item to write to</param>
        /// <param name="target">The class to read from</param>
        public static void GlassRead<T>(this Item item, T target) where T : class
        {
            ISitecoreService service = new SitecoreService(item.Database);
            service.WriteToItem<T>(target, item);

        }
    }
}
