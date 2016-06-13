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

namespace Glass.Mapper.Sc.Razor
{
    /// <summary>
    /// Class RawString
    /// </summary>
    public class RawString : RazorEngine.Text.RawString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawString"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        public RawString(string content)
            : base(content)
        {
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="RawString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="raw">The raw.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string (RawString raw){
            return raw.ToString();
        }
    }
}

