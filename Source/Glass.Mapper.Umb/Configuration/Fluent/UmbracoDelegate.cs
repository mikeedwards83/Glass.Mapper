using System;
using System.Linq.Expressions;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Umb.Configuration.Fluent
{
    /// <summary>
    /// Delegates control of a field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UmbracoDelegate<T> : AbstractPropertyBuilder<T, DelegatePropertyConfiguration<UmbracoDataMappingContext>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldExpression"></param>
        public UmbracoDelegate(Expression<Func<T, object>> fieldExpression)
            : base(fieldExpression)
        {
        }

        /// <summary>
        /// Sets the value to be stored by the cms
        /// </summary>
        /// <param name="mapAction"></param>
        /// <returns></returns>
        public UmbracoDelegate<T> SetValue(Action<UmbracoDataMappingContext> mapAction)
        {
            Configuration.MapToCmsAction = mapAction;
            return this;
        }

        /// <summary>
        /// Gets the value to be returned in the object
        /// </summary>
        /// <param name="mapFunction"></param>
        /// <returns></returns>
        public UmbracoDelegate<T> GetValue(Func<UmbracoDataMappingContext, object> mapFunction)
        {
            Configuration.MapToPropertyAction = mapFunction;
            return this;
        }
    }
}