using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public class LambdaPropertyAccessorFactory : IPropertyAccessorFactory
    {
        /// <summary>
        /// Creates an action delegate that can be used to set a property's value
        /// </summary>
        /// <remarks>
        /// This compiles down to 'native' IL for maximum performance
        /// </remarks>
        /// <param name="property">The property to create a setter for</param>
        /// <returns>An action delegate</returns>
        public Action<object, object> SetPropertyAction(PropertyInfo property)
        {
            PropertyInfo propertyInfo = property;
            Type type = property.DeclaringType;

            if (propertyInfo.CanWrite)
            {
                if (type == null)
                {
                    throw new InvalidOperationException(
                        "PropertyInfo 'property' must have a valid (non-null) DeclaringType.");
                }

                Type propertyType = propertyInfo.PropertyType;

                ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "instance");
                ParameterExpression valueParameter = Expression.Parameter(typeof(object), "value");

                Expression<Action<object, object>> lambda = Expression.Lambda<Action<object, object>>(
                    Expression.Assign(
                        Expression.Property(Expression.Convert(instanceParameter, type), propertyInfo),
                        Expression.Convert(valueParameter, propertyType)),
                    instanceParameter,
                    valueParameter
                );

                return lambda.Compile();
            }
            else
            {
                return (object instance, object value) =>
                {
                    //does nothing
                };
            }
        }

        /// <summary>
        /// Creates a function delegate that can be used to get a property's value
        /// </summary>
        /// <remarks>
        /// This compiles down to 'native' IL for maximum performance
        /// </remarks>
        /// <param name="property">The property to create a getter for</param>
        /// <returns>A function delegate</returns>
        public Func<object, object> GetPropertyFunc(PropertyInfo property)
        {
            PropertyInfo propertyInfo = property;
            Type type = property.DeclaringType;

            if (type == null)
            {
                throw new InvalidOperationException(
                    "PropertyInfo 'property' must have a valid (non-null) DeclaringType.");
            }


            if (propertyInfo.CanWrite)
            {
                ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "instance");

                Expression<Func<object, object>> lambda = Expression.Lambda<Func<object, object>>(
                    Expression.Convert(
                        Expression.Property(
                            Expression.Convert(instanceParameter, type),
                            propertyInfo),
                        typeof(object)),
                    instanceParameter
                );

                return lambda.Compile();
            }
            else
            {
                return (object instance) => { return null; };
            }
        }

    }
}
