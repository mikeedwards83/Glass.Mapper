using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core;

namespace Glass.Mapper.Umb.Integration
{
    /// <summary>
    /// Extends the UmbracoApplicationBase, which is needed to start the application with our own BootManager.
    /// </summary>
    public class TestApplicationBase : UmbracoApplicationBase
    {
        protected override IBootManager GetBootManager()
        {
            return new TestBootManager(this);
        }

        public void Start(object sender, EventArgs e)
        {
            Application_Start(sender, e);
        }

        public void Stop(object sender, EventArgs e)
        {
            Application_End(sender, e);
        }
    }
}
