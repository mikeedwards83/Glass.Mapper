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
using Glass.Mapper.Sc.IoC;
using Sitecore.Data;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreContext
    /// </summary>
    public class SitecoreContext : AbstractSitecoreContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreContext"/> class.
        /// </summary>
        public SitecoreContext()
            : this(Sitecore.Context.Database, GlassContextProvider.Default)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractService" /> class.
        /// </summary>
        /// <param name="contextName">Name of the context.</param>
        public SitecoreContext(string contextName)
            : base(Sitecore.Context.Database, contextName)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreContext"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SitecoreContext(Context context)
            : base(Sitecore.Context.Database, context)
        {

        }

        /// <summary>
        /// Used for unit tests only
        /// </summary>
        /// <param name="database"></param>
        public SitecoreContext(Database database):
            this(database, GlassContextProvider.Default)
        {
            
        }

        protected SitecoreContext(Database database, IGlassContextProvider glassContextProvider) : 
            base(database, glassContextProvider.GetContext())
        {
            
        }

        [Obsolete("Use SitecoreContextFactory.Default.GetSitecoreContext(contextName) instead.")]
        public static ISitecoreContext GetFromHttpContext(string contextName = null)
        {
            return SitecoreContextFactory.Default.GetSitecoreContext(contextName);
        }
    }
}




