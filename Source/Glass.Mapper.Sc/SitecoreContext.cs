using System;
using Glass.Mapper.Sc.IoC;
using Sitecore.Data;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreContext
    /// </summary>
    [Obsolete("Use either IRequestContext, IMvcContext or IWebFormsContext")]
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
        /// Initializes a new instance of the <see cref="SitecoreContext"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SitecoreContext(Context context)
            : base(Sitecore.Context.Database, context)
        {
            CacheAlwaysOnCheck();
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
            CacheAlwaysOnCheck();
        }

        private void CacheAlwaysOnCheck()
        {
            if (Config.Cache.AlwaysOn)
            {
                throw new NotSupportedException("SitecoreContext and Cache.CacheAlwaysOn is not supported");
            }
        }
    }
}




