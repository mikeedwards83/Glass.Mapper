using System;
using System.Linq.Expressions;

namespace Glass.Mapper.Sc.Configuration.Fluent
{

    /// <summary>
    /// Used to map item information to a class property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreItem<T> : AbstractPropertyBuilder<T, SitecoreItemConfiguration>
    {
        private readonly SitecoreTypeConfiguration _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInfo{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreItem(Expression<Func<T, object>> ex, SitecoreTypeConfiguration owner):base(ex)
        {
            _owner = owner;
        }
    }
}




