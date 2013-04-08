using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Umb.CastleWindsor;

namespace Glass.Mapper.Umb.Integration
{
    class WindsorContainer
    {
        public static Context GetContext()
        {
            var config = new Config();
            var resolver = DependencyResolver.CreateStandardResolver(config);
            var context = Context.Create(resolver);
            ((DependencyResolver)resolver).Container.Install(new UmbracoInstaller(config));
            return context;
        }
    }
}
