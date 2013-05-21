using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Glass.Mapper.Sc.Configuration.Fluent;

namespace Glass.Mapper.Sc.Upgrade.Configuration.Fluent
{
    /// <summary>
    /// SitecoreClass
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreClass<T>: SitecoreType<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreClass{T}"/> class.
        /// </summary>
        public SitecoreClass()
        {

        }

        /// <summary>
        /// Map a Sitecore item to a class property
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreNode<T> Item(Expression<Func<T, object>> ex)
        {
            return Node(ex);
        }


        /// <summary>
        /// Map Sitecore items to a class properties
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public SitecoreType<T> Items(Action<ISitecoreClassNodes<T>> items)
        {
            items.Invoke(this);
            return this;
        }
    }
}
