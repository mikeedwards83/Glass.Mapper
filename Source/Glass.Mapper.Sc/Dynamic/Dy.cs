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

namespace Glass.Mapper.Sc.Dynamic
{
    /// <summary>
    /// Class Dy
    /// </summary>
    public class Dy
    {
        /// <summary>
        /// Fcs the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>Func{dynamicSystem.Boolean}.</returns>
        public static Func<dynamic, bool> Fc(Func<dynamic, bool> expression)
        {
            return expression;
        }

        /// <summary>
        /// Fcs the T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>Func{dynamic``0}.</returns>
        public static Func<dynamic, T> FcT<T>(Func<dynamic, T> expression)
        {
            return expression;
        }

    }
}




