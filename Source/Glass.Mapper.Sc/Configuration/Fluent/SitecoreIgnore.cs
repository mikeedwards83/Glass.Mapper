using System;
using System.Linq.Expressions;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// SitecoreIgnore
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreIgnore<T> : AbstractPropertyBuilder<T, SitecoreIgnoreConfiguration>
    {
     /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreField{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreIgnore(Expression<Func<T, object>> ex)
            : base(ex)
        {
           // Configuration.FieldName = Configuration.PropertyInfo.Name;
        }
    }
}

