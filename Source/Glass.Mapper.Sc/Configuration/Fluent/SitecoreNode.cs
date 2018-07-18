using System;
using System.Linq.Expressions;

namespace Glass.Mapper.Sc.Configuration.Fluent
{


    /// <summary>
    /// Used to pull in a Sitecore item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreNode<T> :AbstractPropertyBuilder <T, SitecoreNodeConfiguration>
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreNode{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreNode(Expression<Func<T, object>> ex)
            : base(ex)
        {
        }

        /// <summary>
        /// The path to the item. If both a path and ID are specified the ID will be used.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>SitecoreNode{`0}.</returns>
        public SitecoreNode<T> Path(string path)
        {
            Configuration.Path = path;
            return this;

        }
        /// <summary>
        /// The Id of the item.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SitecoreNode{`0}.</returns>
        public SitecoreNode<T> Id(Guid id)
        {
            Configuration.Id = id.ToString();
            return this;
        }
        

    }
}




