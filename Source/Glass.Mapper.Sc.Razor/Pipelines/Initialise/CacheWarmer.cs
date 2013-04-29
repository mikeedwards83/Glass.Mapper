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
    public class CacheWarmer
    {
        public void Process(PipelineArgs args)
        {
            Task task = new Task(Warm);
            task.Start();
        }

        protected void Warm()
        {
            var dbs = Sitecore.Configuration.Factory.GetDatabases();

            foreach (var db in dbs)
            {
                WarmDb(db);
            }
        }

        protected void WarmDb(Database db)
        {
            var viewManager = new ViewManager();

            var behind = db.GetItem(SitecoreIds.GlassBehindRazorId);
            var typed = db.GetItem(SitecoreIds.GlassTypedRazorId);
            var dynamic = db.GetItem(SitecoreIds.GlassDynamicRazorId);

            WarmType(behind, viewManager);
            WarmType(typed, viewManager);
            WarmType(dynamic, viewManager);
        }
        protected void WarmType(Item item, ViewManager viewManager)
        {
            if (item != null)
            {
                var renderings = global::Sitecore.Configuration.Factory.GetLinkDatabase().GetReferrers(item);
                foreach (var rendering in renderings)
                {
                    var path = rendering.GetSourceItem()["Name"];
                    var contents = viewManager.GetRazorView(path);
                    RazorEngine.Razor.GetTemplate(contents, path);
                }
            }
        }
    }
}
