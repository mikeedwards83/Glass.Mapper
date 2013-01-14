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
                var types = parameters.Select(x => x.ParameterType).ToArray();

                

                var dynMethod = new DynamicMethod("DM$OBJ_FACTORY_" + type.Name, type, types, type);

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

                if (!types.Any())
                    types = Type.EmptyTypes;

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
    }
}
