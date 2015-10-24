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

namespace Glass.Mapper
{
    /// <summary>
    /// Class ExtensionMethods
    /// </summary>
    public static class ExtensionMethods
    {
        #region String


        public static string OrDefault(this string target, Func<string> defaultValue)
        {
            return target.HasValue() ?  target : defaultValue();
        }

        /// <summary>
        /// Replaces the format item in a specified string with the string representation
        /// of a corresponding object in a specified array.
        /// </summary>
        /// <param name="target">A composite format string (see Remarks).</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.</returns>
        public static string Formatted(this string target, params object[] args)
        {
            return string.Format(target, args);
        }

        /// <summary>
        /// Indicates whether the specified string is null or an System.String.Empty
        /// string.
        /// </summary>
        /// <param name="target">The string to test.</param>
        /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }


        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of
        ///     white-space characters.
        /// </summary>
        /// <param name="target">The string to test.</param>
        /// <returns> true if the value parameter is null or System.String.Empty, or if value consists
        ///     exclusively of white-space characters.</returns>
        public static bool IsNullOrWhiteSpace(this string target)
        {
            return string.IsNullOrWhiteSpace(target);
        }

        /// <summary>
        /// Indicates whether the specified string is not null and isn't System.String.Empty
        /// string.
        /// </summary>
        /// <param name="target">The string to test.</param>
        /// <returns>flase if the value parameter is null or an empty string (""); otherwise, true.</returns>
        [Obsolete("Use HasValue")]
        public static bool IsNotNullOrEmpty(this string target)
        {
            return HasValue(target);
        }

        /// <summary>
        /// Indicates whether the specified string has a value, i.e. Not null or empty
        /// </summary>
        public static bool HasValue(this string target)
        {
            return !IsNullOrEmpty(target);
        }

        public static string[] Split(this string text, char character, StringSplitOptions options)
        {
            return text.Split(new[] {character}, options);
        }

        public static int ToInt(this string target)
        {
            int val = 0;
            if (target.HasValue())
            {
                 int.TryParse(target, out val);
            }

            return val;
        }

        public static float ToFlaot(this string target)
        {
            float val = 0;
            if (target.HasValue())
            {
                float.TryParse(target, out val);
            }

            return val;
        }

        #endregion

        #region IEnumerable<T>

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="action">The action.</param>
        /// <returns>IEnumerable{``0}.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
            return list;
        }
        /// <summary>
        /// Makes the enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns>IEnumerable{``0}.</returns>
        public static IEnumerable<T> MakeEnumerable<T>(this T obj)
        {
            return new T[] { obj };
        }

        #endregion

      


        #region Misc

        /// <summary>
        /// Casts to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <returns>``0.</returns>
        public static T CastTo<T>(this object target)
        {
            return (T)target;
        }



        #endregion
    }
}




