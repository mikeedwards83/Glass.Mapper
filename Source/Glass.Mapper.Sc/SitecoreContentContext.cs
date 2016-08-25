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


using Glass.Mapper.Sc.IoC;
using Sitecore.Data;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreContentContext
    /// </summary>
    public class SitecoreContentContext : AbstractSitecoreContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreContentContext"/> class.
        /// </summary>
        public SitecoreContentContext()
            : this(Sitecore.Context.ContentDatabase, GlassContextProvider.Default)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreContentContext" /> class.
        /// </summary>
        /// <param name="contextName">Name of the context.</param>
        public SitecoreContentContext(string contextName)
            : base(Sitecore.Context.ContentDatabase, contextName)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreContentContext"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SitecoreContentContext(Context context)
            : base(Sitecore.Context.ContentDatabase, context)
        {

        }

        /// <summary>
        /// Used for unit tests only
        /// </summary>
        /// <param name="database"></param>
        public SitecoreContentContext(Database database):
            this(database, GlassContextProvider.Default)
        {

        }

        protected SitecoreContentContext(Database database, IGlassContextProvider glassContextProvider) : base(database, glassContextProvider.GetContext())
        {
            
        }
    }
}




