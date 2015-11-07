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
using System.Web;
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
            : base(Sitecore.Context.Database, GetContextFromSite())
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
            base(database, GetContextFromSite())
        {
            
        }

        public static SitecoreContext GetFromHttpContext(string contextName = null)
        {
            if (string.IsNullOrEmpty(contextName))
            {
                contextName = GetContextFromSite();
            }

            var cachedContexts = CachedContexts;
            if (cachedContexts == null)
            {
                return new SitecoreContext(contextName);
            }
            else
            {
                SitecoreContext context = null;
                if (cachedContexts.ContainsKey(contextName))
                {
                    context = cachedContexts[contextName];
                }

                if (context == null)
                {
                    context = new SitecoreContext(contextName);
                    cachedContexts[contextName] = context;
                }
                return context;
            }

        }

        private const string CachedContextsKey = "CEC8A395-F2AE-48BD-A24F-4F40598094BD";

        protected static Dictionary<string, SitecoreContext> CachedContexts
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    throw new NotSupportedException("Cached Contexts are stored in the http context items collection, the http context is currently null");
                }

                if (Sitecore.Context.Items != null)
                {
                    var dictionary = HttpContext.Current.Items[CachedContextsKey] as Dictionary<string, SitecoreContext>;
                    if (dictionary == null)
                    {
                        dictionary = new Dictionary<string, SitecoreContext>();
                        HttpContext.Current.Items[CachedContextsKey] = dictionary;
                    }
                    return dictionary;
                }
                else
                {
                    return null;
                }


            }
        }
    }
}




