using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public class ExpressionBuilder
    {
        private readonly Type _type;

        private List<MemberBinding> _bindings;
        private ParameterExpression _argsExpression;


        public ExpressionBuilder(Type type)
        {
            _type = type;
            _bindings = new List<MemberBinding>();
            _argsExpression = Expression.Parameter(typeof(AbstractDataMappingContext), "args");
        }


        public void AddMemberBinding(PropertyInfo propertyInfo, AbstractCommonDataMapper target, string method)
        {
            //TODO need to handle null return value from MapToProperty
            var methodInfo = target.GetType().GetMethod(method);
            var callExpression = Expression.Call(Expression.Constant(target), methodInfo, _argsExpression);

            //todo remove this cast
            var castExpression = Expression.Convert(callExpression, propertyInfo.PropertyType);
            var memberBinding = Expression.Bind(propertyInfo,  castExpression);
            _bindings.Add(memberBinding);

        }

        public MemberInitExpression CreateConstructor()
        {
            var constructorInfo = _type.GetConstructors()[0];
            var exNew = Expression.New(constructorInfo);
            return Expression.MemberInit(exNew, _bindings);
        }

        public Func<AbstractDataMappingContext, object> Build()
        {
            return Expression.Lambda<Func<AbstractDataMappingContext, object>>(
                CreateConstructor(), _argsExpression
                ).Compile();
        }
    }
}
