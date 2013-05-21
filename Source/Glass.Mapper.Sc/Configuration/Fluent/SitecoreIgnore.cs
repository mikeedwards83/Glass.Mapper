using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// SitecoreIgnore
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreIgnore<T> : AbstractPropertyBuilder<T, SitecoreFieldConfiguration>
    {
     /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreField{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreIgnore(Expression<Func<T, object>> ex)
            : base(ex)
        {
            Configuration.FieldName = Configuration.PropertyInfo.Name;
        }
    }
}
