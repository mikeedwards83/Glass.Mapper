using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ConfigurationResolver
{
    public class ConfigurationResolverArgs : AbstractPipelineArgs
    {
        public AbstractTypeCreationContext AbstractTypeCreationContext { get; private set; }
        public Type Type { get; private set; }
        public AbstractTypeConfiguration Result { get; set; }

        public ConfigurationResolverArgs(Context context, AbstractTypeCreationContext abstractTypeCreationContext, Type type) :base(context)
        {
            AbstractTypeCreationContext = abstractTypeCreationContext;
            Type = type;
        }
    }
}
