using System;
using System.Linq.Expressions;
using Glass.Mapper.Configuration;
using Glass.Mapper.Umb.Configuration.Fluent;

namespace Glass.Mapper.Umb
{
    public class Delegate<T, TK> : AbstractPropertyBuilder<T, DelegatePropertyConfiguration<TK>>
        where TK : AbstractDataMappingContext
    {
        public Delegate(Expression<Func<T, object>> fieldExpression)
            : base(fieldExpression)
        {
        }

        public Delegate<T, TK> SetValue(Action<TK> mapAction)
        {
            Configuration.MapToCmsAction = mapAction;
            return this;
        }

        public Delegate<T, TK> GetValue(Func<TK, object> mapFunction)
        {
            Configuration.MapToPropertyAction = mapFunction;
            return this;
        }
    }
}