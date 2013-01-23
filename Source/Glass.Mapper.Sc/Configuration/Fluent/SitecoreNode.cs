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

/*
   Copyright 2011 Michael Edwards
 
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Glass.Mapper.Sc.Configuration.Fluent
{

   
 ///    <summary>
    /// Used to pull in a Sitecore item 
     ///</summary>
     
     
    public class SitecoreNode<T> :AbstractPropertyBuilder <T, SitecoreNodeConfiguration>
    {


        public SitecoreNode(Expression<Func<T, object>> ex)
            : base(ex)
        {
        }

        /// <summary>
        /// The path to the item. If both a path and ID are specified the ID will be used.
        /// </summary>
        public SitecoreNode<T> Path(string path)
        {
            Configuration.Path = path;
            return this;

        }
        /// <summary>
        /// The Id of the item. 
        /// </summary>
        public SitecoreNode<T> Id(Guid id)
        {
            Configuration.Id = id.ToString();
            return this;
        }
        /// <summary>
        /// Indicates that the item should not be loaded lazily. If set the item will be loaded when the containing object is created.
        /// </summary>
        public SitecoreNode<T> IsNotLazy()
        {
            Configuration.IsLazy = false;
            return this;
        }

    }
}



