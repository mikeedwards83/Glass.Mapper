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

namespace Glass.Mapper.Sc.Razor
{
    /// <summary>
    /// Class RazorException
    /// </summary>
    public class RazorException: ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.ApplicationException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public RazorException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.ApplicationException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public RazorException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}

