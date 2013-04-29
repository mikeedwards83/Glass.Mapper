using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Glass.Mapper.Umb
{
    /// <summary>
    /// Utilities
    /// </summary>
    public static class Utilities
    {
		private static readonly ConcurrentDictionary<Type, ActivationManager.CompiledActivator<object>> Activators =
			new ConcurrentDictionary<Type, ActivationManager.CompiledActivator<object>>();


        /// <summary>
        /// Creates the type of the generic.
        /// </summary>
        /// <param name="type">The generic type to create e.g. List&lt;&gt;</param>
        /// <param name="arguments">The list of subtypes for the generic type, e.g string in List&lt;string&gt;</param>
        /// <param name="parameters">List of parameters to pass to the constructor.</param>
        /// <returns></returns>
        public static object CreateGenericType(Type type, Type[] arguments, params  object[] parameters)
        {
            Type genericType = type.MakeGenericType(arguments);
            object obj;
            if (parameters != null && parameters.Any())
				obj = GetActivator(genericType, parameters.Select(p => p.GetType()))(parameters);
            else
				obj = GetActivator(genericType)();
            return obj;
        }

        /// <summary>
        /// Gets the generic argument.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="MapperException">
        /// Type {0} has more than one generic argument.Formatted(type.FullName)
        /// or
        /// The type {0} does not contain any generic arguments.Formatted(type.FullName)
        /// </exception>
        public static Type GetGenericArgument(Type type)
        {
            Type[] types = type.GetGenericArguments();
            if (types.Count() > 1) throw new MapperException("Type {0} has more than one generic argument".Formatted(type.FullName));
            if (!types.Any()) throw new MapperException("The type {0} does not contain any generic arguments".Formatted(type.FullName));
            return types[0];
        }

		private static ActivationManager.CompiledActivator<object> GetActivator(Type forType, IEnumerable<Type> parameterTypes = null)
		{
			var paramTypes = parameterTypes == null ? null : parameterTypes.ToArray();
			return Activators.GetOrAdd(forType, type => ActivationManager.GetActivator<object>(type, paramTypes));
		}
    }
}
