
using System;
using Glass.Mapper.Sc.IoC;
using Sitecore.Data;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreContentContext
    /// </summary>
    [Obsolete("This class is obsolete in V5")]
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




