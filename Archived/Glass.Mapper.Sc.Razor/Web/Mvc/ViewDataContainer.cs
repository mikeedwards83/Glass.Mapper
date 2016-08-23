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
using System.Web.Mvc;

namespace Glass.Mapper.Sc.Razor.Web.Mvc
{
    /// <summary>
    /// Class ViewDataContainer
    /// </summary>
    public class ViewDataContainer : IViewDataContainer
    {
        /// <summary>
        /// Gets or sets the view data dictionary.
        /// </summary>
        /// <value>The view data.</value>
        /// <returns>The view data dictionary.</returns>
        public ViewDataDictionary ViewData
        {
            get;
            set;
        }
    }
}

