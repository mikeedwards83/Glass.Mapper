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
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Reflection;
using Glass.Mapper.Configuration;

namespace Glass.Mapper
{
    /// <summary>
    /// Class Utilities
    /// </summary>
    public class Utilities
    {
        private static readonly ConcurrentDictionary<Type, ActivationManager.CompiledActivator<object>> Activators =
            new ConcurrentDictionary<Type, ActivationManager.CompiledActivator<object>>();


        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        /// <summary>
        /// Returns a delegate method that will load a class based on its constuctor
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>IDictionary{ConstructorInfoDelegate}.</returns>
        /// <exception cref="MapperException">Only supports constructors with  a maximum of 10 parameters</exception>
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
                    case 5:
                        genericType = typeof(Func<,,,,,>);
                        break;
                    case 6:
                        genericType = typeof(Func<,,,,,,>);
                        break;
                    case 7:
                        genericType = typeof(Func<,,,,,,,>);
                        break;
                    case 8:
                        genericType = typeof(Func<,,,,,,,,>);
                        break;
                    case 9:
                        genericType = typeof(Func<,,,,,,,,>);
                        break;
                    case 10:
                        genericType = typeof(Func<,,,,,,,,,>);
                        break;
                    default:
                        throw new MapperException("Only supports constructors with a maximum of 10 parameters for type {0}".Formatted(type.FullName));
                }

                var delegateType =
                    genericType.MakeGenericType(parameters.Select(x => x.ParameterType).Concat(new[] { type }).ToArray());

                if (!types.Any())
                    types = Type.EmptyTypes;

                dic[constructor] = dynMethod.CreateDelegate(delegateType);
            }

            return dic;
        }

        /// <summary>
        /// The flags
        /// </summary>
        public static BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy |
                                           BindingFlags.Instance;

        /// <summary>
        /// Gets a property based on the type and name
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>PropertyInfo.</returns>
        public static PropertyInfo GetProperty(Type type, string name)
        {
            PropertyInfo property = null;
            try
            {
                property = type.GetProperty(name, Flags);
            }
            catch (AmbiguousMatchException ex)
            {
                //this is probably caused by an item having two indexers e.g SearchResultItem;
            }

            if (property == null)
            {
                var interfaces = type.GetInterfaces();
                foreach (var inter in interfaces)
                {
                    try
                    {
                        property = inter.GetProperty(name);
                        if (property != null)
                            return property;
                    }
                    catch (AmbiguousMatchException ex)
                    {
                        //this is probably caused by an item having two indexers e.g SearchResultItem;
                    }
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


        public static NameValueCollection GetPropertiesCollection(object target, bool lowerCaseName = false,
            bool underscoreForHyphens = true)
        {
            NameValueCollection nameValues = new NameValueCollection();
            if (target != null)
            {
                var type = target.GetType();
                var properties = GetAllProperties(type);

                foreach (var propertyInfo in properties)
                {
                    var value = propertyInfo.GetValue(target, null);

                    var key = lowerCaseName ? propertyInfo.Name.ToLower() : propertyInfo.Name;

                    if (underscoreForHyphens)
                    {
                        key = key.Replace("_", "-");
                    }

                    nameValues.Add(key, value == null ? string.Empty : value.ToString());
                }
            }
            return nameValues;

        }

        /// <summary>
        /// Creates the type of the generic.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        public static object CreateGenericType(Type type, Type[] arguments, params object[] parameters)
        {
            Type genericType = type.MakeGenericType(arguments);
            object obj;
            if (parameters != null && parameters.Any())
            {
                var paramTypes = parameters.Select(p => p.GetType()).ToArray();
                obj = GetActivator(genericType, paramTypes)(parameters);
            }
            else
                obj = GetActivator(genericType)();
            return obj;
        }

        public static ActivationManager.CompiledActivator<object> GetActivator(Type type, 
            Type[] arguments,
            params object[] parameters)
        {
            Type genericType = type.MakeGenericType(arguments);
            var paramTypes = parameters.Select(p => p.GetType()).ToArray();
            return GetActivator(genericType, paramTypes);
        }

        /// <summary>
        /// Gets the generic argument.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Type.</returns>
        /// <exception cref="Glass.Mapper.MapperException">
        /// Type {0} has more than one generic argument.Formatted(type.FullName)
        /// or
        /// The type {0} does not contain any generic arguments.Formatted(type.FullName)
        /// </exception>
        public static Type GetGenericArgument(Type type)
        {
            Type[] types = type.GetGenericArguments();
            var count = types.Count();
            if (count == 1)
                return types[0];
            else if(count > 1)
                throw new MapperException("Type {0} has more than one generic argument".Formatted(type.FullName));
            else
                throw new MapperException("The type {0} does not contain any generic arguments".Formatted(type.FullName));
        }

        public static string GetPropertyName(Expression expression)
        {
            string name = String.Empty;

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


            if (name.IsNullOrEmpty())
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
        public static Func<PropertyInfo, Action<object, object>> SetPropertyAction = (PropertyInfo property) =>
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
        };

        /// <summary>
        /// Creates a function delegate that can be used to get a property's value
        /// </summary>
        /// <remarks>
        /// This compiles down to 'native' IL for maximum performance
        /// </remarks>
        /// <param name="property">The property to create a getter for</param>
        /// <returns>A function delegate</returns>
        public static Func<PropertyInfo, Func<object, object>> GetPropertyFunc = (PropertyInfo property) =>
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
        };

        /// <summary>
        /// Gets the activator.
        /// </summary>
        /// <param name="forType">For type.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <returns></returns>
        protected static ActivationManager.CompiledActivator<object> GetActivator(Type forType,
            Type [] parameterTypes = null)
        {
            return Activators.GetOrAdd(forType, type => ActivationManager.GetActivator<object>(type, parameterTypes));
        }


        public static AbstractPropertyConfiguration GetGlassProperty<T, K>(
            Expression<Func<T, object>> field,
            Context context,
            T model) where K : AbstractTypeConfiguration, new()
        {

            MemberExpression memberExpression;

            var finalTarget = GetTargetObjectOfLamba(field, model, out memberExpression);

            if (context == null)
                throw new NullReferenceException("Context cannot be null");

            var config = context.GetTypeConfiguration<K>(finalTarget);

            //lambda expression does not always return expected memberinfo when inheriting
            //c.f. http://stackoverflow.com/questions/6658669/lambda-expression-not-returning-expected-memberinfo
            var prop = config.Type.GetProperty(memberExpression.Member.Name);

            //interfaces don't deal with inherited properties well
            if (prop == null && config.Type.IsInterface)
            {
                Func<Type, PropertyInfo> interfaceCheck = null;
                interfaceCheck = (inter) =>
                {
                    var interfaces = inter.GetInterfaces();
                    var properties =
                        interfaces.Select(x => x.GetProperty(memberExpression.Member.Name)).Where(
                            x => x != null);
                    if (properties.Any()) return properties.First();
                    else
                        return interfaces.Select(x => interfaceCheck(x)).FirstOrDefault(x => x != null);
                };
                prop = interfaceCheck(config.Type);
            }

            if (prop != null && prop.DeclaringType != prop.ReflectedType)
            {
                //properties mapped in data handlers are based on declaring type when field is inherited, make sure we match
                prop = prop.DeclaringType.GetProperty(prop.Name);
            }

            if (prop == null)
                throw new MapperException(
                    "Page editting error. Could not find property {0} on type {1}".Formatted(
                        memberExpression.Member.Name, config.Type.FullName));

            //ME - changed this to work by name because properties on interfaces do not show up as declared types.
            var dataHandler = config.Properties.FirstOrDefault(x => x.PropertyInfo.Name == prop.Name);
            if (dataHandler == null)
            {
                throw new MapperException(
                    "Page editing error. Could not find data handler for property {2} {0}.{1}".Formatted(
                        prop.DeclaringType, prop.Name, prop.MemberType));
            }


            return dataHandler;
        }

        public static K GetTypeConfig<T, K>(Expression<Func<T, object>> field, Context context, T model)
            where K : AbstractTypeConfiguration, new()
        {
            MemberExpression memberExpression;
            var finalTarget = GetTargetObjectOfLamba(field, model, out memberExpression);

            if (context == null)
                throw new NullReferenceException("Context cannot be null");

            var config = context.GetTypeConfiguration<K>(finalTarget);

            return config;
        }


        public static object GetTargetObjectOfLamba<T>(Expression<Func<T, object>> field, T model,
            out MemberExpression memberExpression)
        {
            if (field.Parameters.Count > 1)
                throw new MapperException("To many parameters in linq expression {0}".Formatted(field.Body));

            if (field.Body is UnaryExpression)
            {
                memberExpression = ((UnaryExpression)field.Body).Operand as MemberExpression;
            }
            else if (!(field.Body is MemberExpression))
            {
                throw new MapperException("Expression doesn't evaluate to a member {0}".Formatted(field.Body));
            }
            else
            {
                memberExpression = (MemberExpression)field.Body;
            }

            //we have to deconstruct the lambda expression to find the 
            //correct model object
            //For example if we have the lambda expression x =>x.Children.First().Content
            //we have to evaluate what the first Child object is, then evaluate the field to edit from there.

            //this contains the expression that will evaluate to the object containing the property
            var objectExpression = memberExpression.Expression;

            var finalTarget =
                Expression.Lambda(objectExpression, field.Parameters).Compile().DynamicInvoke(model);

            return finalTarget;
        }
    }
}



