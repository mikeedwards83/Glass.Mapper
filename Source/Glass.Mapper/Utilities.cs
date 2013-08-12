/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Reflection;

namespace Glass.Mapper
{
    /// <summary>
    /// Class Utilities
    /// </summary>
    public   class Utilities 
    {
		private static readonly ConcurrentDictionary<Type, ActivationManager.CompiledActivator<object>> Activators = 
			new ConcurrentDictionary<Type, ActivationManager.CompiledActivator<object>>();

        /// <summary>
        /// Returns a delegate method that will load a class based on its constuctor
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>IDictionary{ConstructorInfoDelegate}.</returns>
        /// <exception cref="MapperException">Only supports constructors with  a maximum of 4 parameters</exception>
        public static IDictionary<ConstructorInfo, Delegate> CreateConstructorDelegates(Type type)
        {
            var constructors = type.GetConstructors();

            var dic = new Dictionary<ConstructorInfo, Delegate>();

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                var types = parameters.Select(x => x.ParameterType).ToArray();

                

                var dynMethod = new DynamicMethod("DM$OBJ_FACTORY_" + type.Name, type, types, type);

                ILGenerator ilGen = dynMethod.GetILGenerator();
                for (int i = 0; i < parameters.Count(); i++)
                {
                    ilGen.Emit(OpCodes.Ldarg, i);
                }

                ilGen.Emit(OpCodes.Newobj, constructor);

                ilGen.Emit(OpCodes.Ret);

                Type genericType;
                switch (parameters.Count())
                {
                    case 0:
                        genericType = typeof(Func<>);
                        break;
                    case 1:
                        genericType = typeof(Func<,>);
                        break;
                    case 2:
                        genericType = typeof(Func<,,>);
                        break;
                    case 3:
                        genericType = typeof(Func<,,,>);
                        break;
                    case 4:
                        genericType = typeof(Func<,,,,>);
                        break;
                    default:
                        throw new MapperException("Only supports constructors with  a maximum of 4 parameters");
                }

                var delegateType = genericType.MakeGenericType(parameters.Select(x => x.ParameterType).Concat(new[] { type }).ToArray());

                if (!types.Any())
                    types = Type.EmptyTypes;

                dic[constructor] = dynMethod.CreateDelegate(delegateType);
            }

            return dic;
        }

        /// <summary>
        /// The flags
        /// </summary>
        public static BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance;

        /// <summary>
        /// Gets a property based on the type and name
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>PropertyInfo.</returns>
        public static PropertyInfo GetProperty(Type type, string name)
        {
            var property = type.GetProperty(name, Flags);
            
            if(property == null)
            {
                var interfaces = type.GetInterfaces();
                foreach (var inter in interfaces)
                {
                      property = inter.GetProperty(name);
                      if (property != null)
                          return property;
                }
            }

            return property;
        }

        /// <summary>
        /// Gets all properties on a type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>IEnumerable{PropertyInfo}.</returns>
        public static IEnumerable<PropertyInfo> GetAllProperties(Type type)
        {
            List<Type> typeList = new List<Type>();
            typeList.Add(type);

            if (type.IsInterface)
            {
                typeList.AddRange(type.GetInterfaces());
            }

            List<PropertyInfo> propertyList = new List<PropertyInfo>();

            foreach (Type interfaceType in typeList)
            {
                foreach (PropertyInfo property in interfaceType.GetProperties(Flags))
                {
                    var finalProperty = GetProperty(property.DeclaringType, property.Name);
                    propertyList.Add(finalProperty);
                }
            }

            return propertyList;
        }


        /// <summary>
        /// Creates the type of the generic.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        public static object CreateGenericType(Type type, Type[] arguments, params  object[] parameters)
        {
            Type genericType = type.MakeGenericType(arguments);
            object obj;
	        if (parameters != null && parameters.Count() > 0)
		        obj = GetActivator(genericType, parameters.Select(p => p.GetType()))(parameters);
			else
				obj = GetActivator(genericType)();
            return obj;
        }

        public static string GetPropertyName(Expression expression)
        {
            string name = string.Empty;

            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    Expression operand = (expression as UnaryExpression).Operand;
                    name = operand.CastTo<MemberExpression>().Member.Name;
                    break;

                case ExpressionType.Call:
                    name = expression.CastTo<MethodCallExpression>().Method.Name;
                    break;
                case ExpressionType.MemberAccess:
                    name = expression.CastTo<MemberExpression>().Member.Name;
                    break;
                case ExpressionType.TypeAs:
                    var unaryExp = expression.CastTo<UnaryExpression>();
                    name = GetPropertyName(unaryExp.Operand);
                    break;
            }
            return name;
        }

        /// <summary>
        /// Returns a PropertyInfo based on a link expression, it will pull the first property name from the linq express.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>PropertyInfo.</returns>
        public static PropertyInfo GetPropertyInfo(Type type, Expression expression)
        {
            string name = GetPropertyName(expression);

          
            if(name.IsNullOrEmpty())
                throw new MapperException("Unable to get property name from lambda expression");

            PropertyInfo info = type.GetProperty(name);

            //if we don't find the property straight away then it is probably an interface
            //and we need to check all inherited interfaces.
            if (info == null)
            {
                info = GetAllProperties(type).FirstOrDefault(x => x.Name == name);
            }

            return info;
        }

	    /// <summary>
	    /// Creates an action delegate that can be used to set a property's value
	    /// </summary>
	    /// <remarks>
	    /// This compiles down to 'native' IL for maximum performance
	    /// </remarks>
		/// <param name="property">The property to create a setter for</param>
	    /// <returns>An action delegate</returns>
	    public static Action<object, object> SetPropertyAction(PropertyInfo property)
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

	            ParameterExpression instanceParameter = Expression.Parameter(typeof (object), "instance");
	            ParameterExpression valueParameter = Expression.Parameter(typeof (object), "value");

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
	    public static Func<object, object> GetPropertyFunc(PropertyInfo property)
		{
			PropertyInfo propertyInfo = property;
			Type type = property.DeclaringType;

			if (type == null)
			{
				throw new InvalidOperationException("PropertyInfo 'property' must have a valid (non-null) DeclaringType.");
			}

	        if (propertyInfo.CanWrite)
	        {
	            ParameterExpression instanceParameter = Expression.Parameter(typeof (object), "instance");

	            Expression<Func<object, object>> lambda = Expression.Lambda<Func<object, object>>(
	                Expression.Convert(
	                    Expression.Property(
	                        Expression.Convert(instanceParameter, type),
	                        propertyInfo),
	                    typeof (object)),
	                instanceParameter
	                );

	            return lambda.Compile();
	        }
	        else
	        {
	            return (object instance) => { return null; };
	        }
		}

        /// <summary>
        /// Gets the activator.
        /// </summary>
        /// <param name="forType">For type.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <returns></returns>
		protected static ActivationManager.CompiledActivator<object> GetActivator(Type forType, IEnumerable<Type> parameterTypes = null)
		{
			var paramTypes = parameterTypes == null ? null : parameterTypes.ToArray();
			return Activators.GetOrAdd(forType, type => ActivationManager.GetActivator<object>(type, paramTypes));
		}
   }
}




