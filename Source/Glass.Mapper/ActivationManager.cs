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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Glass.Mapper
{
	/// <summary>
	/// Provides performant construction of types known only at runtime.
	/// Similar to the Activator class, but uses compiled expressions.
	/// </summary>
	/// <remarks>
	/// This type is thread safe.
	/// For more info, see: http://rogeralsing.com/2008/02/28/linq-expressions-creating-objects/
	/// </remarks>
	public static class ActivationManager
	{
		/// <summary>
		/// Activator delegate that can be called with an arbitrary number of constructor arguments
		/// </summary>
		/// <typeparam name="T">The type to be constructed</typeparam>
		/// <param name="args">Array of constructor arguments</param>
		/// <returns>An instance of type T, constructed with the specified args</returns>
		public delegate T CompiledActivator<out T>(params object[] args);

		/// <summary>
		/// Gets a constructor delegate for the given type and constructor arguments
		/// </summary>
		/// <typeparam name="T">Covariant type (in regards to param 'type') to be returned from the delegate</typeparam>
		/// <param name="type">Actual Type to construct at runtime</param>
		/// <param name="args">Constructor argument types</param>
		/// <returns>A compiled constructor delegate with a signature matching the passed in Type[] args, or null if no matching constructor exists</returns>
		public static CompiledActivator<T> GetActivator<T>(Type type, params Type[] args)
		{
			ConstructorInfo[] constructors = type.GetConstructors();
			ConstructorInfo constructor = args == null
				? constructors.FirstOrDefault(ci => !ci.GetParameters().Any())
				: constructors.FirstOrDefault(ci => ci.GetParameters().Select(p => p.ParameterType).SequenceEqual(args, (type1, type2) => type1.IsAssignableFrom(type2)));

			return constructor == null ? null : CreateActivator<T>(constructor);
		}

		/// <summary>
		/// Creates a constructor delegate for the given constructor
		/// </summary>
		/// <typeparam name="T">Covariant type (in regards to the ConstructorInfo's declaring type) to be returned from the delegate</typeparam>
		/// <param name="constructor">The ConstructorInfo to use in creating a constructor delegate</param>
		/// <returns>A compiled constructor delegate with a signature matching the passed in ConstructorInfo's signature</returns>
		public static CompiledActivator<T> CreateActivator<T>(ConstructorInfo constructor)
		{
			ParameterInfo[] paramsInfo = constructor.GetParameters();

			//create a single param of type object[]
			ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

			var argsExp = new Expression[paramsInfo.Length];

			// Create a typed expression with each arg from the parameter array
			for (int i = 0; i < paramsInfo.Length; i++)
			{
				Expression index = Expression.Constant(i);
				Type paramType = paramsInfo[i].ParameterType;

				Expression paramAccessorExp = Expression.ArrayIndex(param, index);
				Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

				argsExp[i] = paramCastExp;
			}

			// creates a new expression that calls the constructor with the specified arguments
			NewExpression newExp = Expression.New(constructor, argsExp);

			//create a lambda with the New Expression as the body and our param object[] as arg
			LambdaExpression lambda = Expression.Lambda(typeof(CompiledActivator<T>), newExp, param);

			// return the compiled activator
			return (CompiledActivator<T>)lambda.Compile();
		}

		/// <summary>
		/// Checks if a given sequence is equal to another, using the specified comparer function
		/// </summary>
		/// <param name="source"></param>
		/// <param name="second"></param>
		/// <param name="comparer"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		private static bool SequenceEqual<T>(this IEnumerable<T> source, IEnumerable<T> second, Func<T, T, bool> comparer)
		{
			return source.SequenceEqual(second, new LambdaComparer<T>(comparer));
		}

		#region Comparer Implementation

		private class LambdaComparer<T> : IEqualityComparer<T>
		{
			private readonly Func<T, T, bool> _innerComparer;

			public LambdaComparer(Func<T, T, bool> comparer)
			{
				if (comparer == null)
					throw new ArgumentNullException("comparer");
				_innerComparer = comparer;
			}

			public bool Equals(T x, T y)
			{
				return _innerComparer(x, y);
			}

			public int GetHashCode(T obj)
			{
				return 0; // Don't care
			}
		}

		#endregion

	}
}

