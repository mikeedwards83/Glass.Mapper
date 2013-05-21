using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Razor.Pipelines.Initialise
{
    /// <summary>
    /// CacheWarmer
    /// </summary>
    public class CacheWarmer
    {
        /// <summary>
        /// Processes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public void Process(PipelineArgs args)
        {
            Task task = new Task(Warm);
            task.Start();
        }

        /// <summary>
        /// Warms this instance.
        /// </summary>
        protected void Warm()
        {
            var dbs = Sitecore.Configuration.Factory.GetDatabases();

            foreach (var db in dbs)
            {
                WarmDb(db);
            }
        }

        /// <summary>
        /// Warms the db.
        /// </summary>
        /// <param name="db">The db.</param>
        protected void WarmDb(Database db)
        {
            

            var behind = db.GetItem(SitecoreIds.GlassBehindRazorId);
            var typed = db.GetItem(SitecoreIds.GlassTypedRazorId);
            var dynamic = db.GetItem(SitecoreIds.GlassDynamicRazorId);

            WarmType(behind);
            WarmType(typed);
            WarmType(dynamic);
        }
        /// <summary>
        /// Warms the type.
        /// </summary>
        /// <param name="item">The item.</param>
        protected void WarmType(Item item)
        {
            if (item != null)
            {
                var renderings = global::Sitecore.Configuration.Factory.GetLinkDatabase().GetReferrers(item);
                foreach (var rendering in renderings)
                {
                    var path = rendering.GetSourceItem()["Name"];
                    ViewManager.GetRazorView(path);
                }
            }
        }
    }
}
