using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.FakeDb.Sites;
using Sitecore.Sites;

namespace Glass.Mapper.Sc.FakeDb.Infrastructure
{
    public class FakeSite :IDisposable
    {
        private FakeSiteContext _fakeSiteContext;
        private SiteContextSwitcher _switcher;

        public FakeSite()
        {
            _fakeSiteContext = new Sitecore.FakeDb.Sites.FakeSiteContext(
                new Sitecore.Collections.StringDictionary
                {
                    {"name", "website"},
                    {"database", "web"}
                });

            _switcher = new FakeSiteContextSwitcher(_fakeSiteContext);
        }

        public void Dispose()
        {
           _switcher.Dispose();
        }
    }
}
