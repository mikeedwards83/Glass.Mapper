using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.TypeResolver;
using Castle.Windsor;

namespace Glass.Mapper
{
    public abstract class GlassCastleConfigBase : IGlassConfiguration
    {
        public abstract void Configure(WindsorContainer container, string contextName);
    }
}
