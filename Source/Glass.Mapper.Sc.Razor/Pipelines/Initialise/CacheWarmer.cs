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

