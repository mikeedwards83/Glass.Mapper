using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Configuration;
using Glass.Mapper.IoC;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.IoC;
using IDependencyResolver = Glass.Mapper.Sc.IoC.IDependencyResolver;

namespace Glass.Mapper.Sc
{
    public class GlassMapperScOptions
    {
        public string ContextName { get; set; } = Context.DefaultContextName;
        public IList<IConfigurationLoader> Loaders { get; } = new List<IConfigurationLoader>();

        public Glass.Mapper.Sc.Config Config { get; set; }= new Config();

        public IList<Func<IGlassMap>> GlassMaps { get; set; } = new List<Func<IGlassMap>>();

        public Action PostLoad { get; set; } = () => { };

        public Action<IDependencyResolver> ConfigureResolver { get; set; } = resolver => { };
    }
}
