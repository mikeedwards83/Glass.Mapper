using System;
using System.Linq.Expressions;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Indicates a field that contains the Sitecore item ID, this field must be a Guid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public  class SitecoreId<T> : AbstractPropertyBuilder<T, SitecoreIdConfiguration>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreId{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreId(Expression<Func<T, object>> ex):base(ex)
        {
            
        }
    }
}




