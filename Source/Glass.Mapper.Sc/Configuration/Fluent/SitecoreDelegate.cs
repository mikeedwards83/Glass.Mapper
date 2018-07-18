using System;
using System.Linq.Expressions;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Delegates control of a field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreDelegate<T> : AbstractPropertyBuilder<T, SitecoreDelegateConfiguration>
    {
        public SitecoreDelegate(Expression<Func<T, object>> fieldExpression)
            : base(fieldExpression)
        {
        }

        /// <summary>
        /// Sets the value to be stored by the cms
        /// </summary>
        /// <param name="mapAction"></param>
        /// <returns></returns>
        public SitecoreDelegate<T> SetValue(Action<SitecoreDataMappingContext> mapAction)
        {
            Configuration.MapToCmsAction = mapAction;
            return this;
        }

        /// <summary>
        /// Gets the value to be returned in the object
        /// </summary>
        /// <param name="mapFunction"></param>
        /// <returns></returns>
        public SitecoreDelegate<T> GetValue(Func<SitecoreDataMappingContext, object> mapFunction)
        {
            Configuration.MapToPropertyAction = mapFunction;
            return this;
        }
    }
}