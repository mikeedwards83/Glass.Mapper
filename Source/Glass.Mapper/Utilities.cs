using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Reflection;

namespace Glass.Mapper
{
    public static  class Utilities
    {

        /// <summary>
        /// Returns a delegate method that will load a class based on its constuctor
        /// </summary>
        /// <returns></returns>
        public static IDictionary<ConstructorInfo, Delegate> CreateConstructorDelegates(Type type)
        {
            var constructors = type.GetConstructors();

            var dic = new Dictionary<ConstructorInfo, Delegate>();

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                var dynMethod = new DynamicMethod("DM$OBJ_FACTORY_" + type.Name, type, parameters.Select(x => x.ParameterType).ToArray(), type);

                ILGenerator ilGen = dynMethod.GetILGenerator();
                for (int i = 0; i < parameters.Count(); i++)
                {
                    ilGen.Emit(OpCodes.Ldarg, i);
                }

                ilGen.Emit(OpCodes.Newobj, constructor);

                ilGen.Emit(OpCodes.Ret);

                Type genericType = null;
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


                dic[constructor] = dynMethod.CreateDelegate(delegateType);
            }

            return dic;
        }

        public static BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance;

        /// <summary>
        /// Gets a property based on the type and name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// <param name="type"></param>
        /// <returns></returns>
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
        /// </summary>
        /// <param name="type">The generic type to create e.g. List&lt;&gt;</param>
        /// <param name="arguments">The list of subtypes for the generic type, e.g string in List&lt;string&gt;</param>
        /// <param name="parameters"> List of parameters to pass to the constructor.</param>
        /// <returns></returns>
        public static object CreateGenericType(Type type, Type[] arguments, params  object[] parameters)
        {
            Type genericType = type.MakeGenericType(arguments);
            object obj;
            if (parameters != null && parameters.Count() > 0)
                obj = Activator.CreateInstance(genericType, parameters);
            else
                obj = Activator.CreateInstance(genericType);
            return obj;
        }

        public static Type GetGenericArgument(Type type)
        {
            Type[] types = type.GetGenericArguments();
            if (types.Count() > 1) throw new MapperException("Type {0} has more than one generic argument".Formatted(type.FullName));
            if (types.Count() == 0) throw new MapperException("The type {0} does not contain any generic arguments".Formatted(type.FullName));
            return types[0];
        }
    }
}
