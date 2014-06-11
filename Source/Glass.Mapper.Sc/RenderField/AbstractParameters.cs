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

using System.Collections.Specialized;
using Sitecore.Text;

namespace Glass.Mapper.Sc.RenderField
{
    /// <summary>
    /// Base class for rendering parameters that will be passed to the renderField pipeline.
    /// </summary>
    public class AbstractParameters
    {
        private readonly NameValueCollection _parameters = new NameValueCollection();

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public NameValueCollection Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var url = new UrlString();
            url.Parameters.Add(Parameters);
            return url.Query;
        }

        public static implicit operator NameValueCollection(AbstractParameters parameters)
        {
            return parameters.Parameters;
        }

    }
}




