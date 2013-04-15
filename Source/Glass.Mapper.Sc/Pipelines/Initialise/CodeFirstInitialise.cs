using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.CodeFirst;
using Sitecore.Data;
using Sitecore.Data.DataProviders;
using Sitecore.Pipelines;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Pipelines.Initialise
{
    public class CodeFirstInitialise
    {
        public void Process(PipelineArgs args)
        {
            var dbs = Sitecore.Configuration.Factory.GetDatabases();
            foreach (var db in dbs)
            {
                var provider = db.GetDataProviders().FirstOrDefault(x => x is GlassDataProvider) as GlassDataProvider;
                if (provider != null)
                {
                    using (new SecurityDisabler())
                    {
                        provider.Initialise(db);
                    }
                }
            }
        }
    }
}
