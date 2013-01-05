using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public interface ISitecoreService: IAbstractService
    {
        Database Database { get; }
        object CreateClass(Type type, Item item, bool isLazy = false, bool inferType = false, params object[] constructorParameters);
       
        /// <summary>
        /// Create a collection of classes from the specified type
        /// </summary>
        /// <param name="isLazy">If true creates a proxy for each class</param>
        /// <param name="type">The type to return</param>
        /// <param name="getItems">A function that returns the list of items to load</param>
        /// <param name="inferType">Infer the type to be loaded from the item template</param>
        /// <returns>An enumerable of the items as the specified type</returns>
        IEnumerable CreateClasses(bool isLazy, bool inferType, Type type, Func<IEnumerable<Item>> getItems);
    }
}
