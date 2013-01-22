using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    public class SitecoreFluentConfigurationLoader : IConfigurationLoader
    {
        List<ISitecoreClass> _types = new List<ISitecoreClass>();

        public SitecoreType<T> Add<T>()
        {
            var config = new SitecoreType<T>();
            _types.Add(config);
            return config;
        }

        public IEnumerable<AbstractTypeConfiguration> Load()
        {
            return _types.Select(x => x.Config);
        }
    }
}
