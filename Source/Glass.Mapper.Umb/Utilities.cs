using System;
using System.Linq;
using umbraco.interfaces;

namespace Glass.Mapper.Umb
{
    public static class Utilities
    {
        public static IProperty GetProperty(INode node, string alias)
        {
            return node.GetProperty(alias);
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
