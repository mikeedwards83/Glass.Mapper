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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Sitecore.Text;

namespace Glass.Mapper.Sc.RenderField
{
    /// <summary>
    /// Base class for rendering parameters that will be passed to the renderField pipeline.
    /// </summary>
    public class AbstractParameters
    {
        private readonly NameValueCollection _parameters = new NameValueCollection();

        protected NameValueCollection Parameters
        {
            get
            {
                return _parameters;
            }
        }

        public override string ToString()
        {
            var url = new UrlString();
            url.Parameters.Add(Parameters);
            return url.Query;
        }
    }
}



