using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    public abstract class AbstractPropertyBuilder<T, TK> where TK : AbstractPropertyConfiguration, new ()
    {

        public AbstractPropertyBuilder(Expression<Func<T, object>> ex)
        {
            Configuration = new TK();
            if (ex.Parameters.Count > 1)
                throw new MapperException("To many parameters in linq expression {0}".Formatted(ex.Body));
            Configuration.PropertyInfo = Mapper.Utilities.GetPropertyInfo(typeof(T), ex.Body);
        }

        public TK Configuration { get; private set; }
        public Expression<Func<T, object>> Expression { get; private set; }
    }
}
