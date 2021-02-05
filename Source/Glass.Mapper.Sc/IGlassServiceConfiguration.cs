using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Glass.Mapper.Sc
{
    public interface IGlassServiceConfiguration
    {
         IServiceCollection ServiceCollection { get; }
    }
    public class GlassServiceConfiguration: IGlassServiceConfiguration{
        public IServiceCollection ServiceCollection { get; set; }
    }
}
