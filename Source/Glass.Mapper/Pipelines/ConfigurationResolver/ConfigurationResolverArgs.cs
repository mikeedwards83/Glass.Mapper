using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ConfigurationResolver
{
    public class ConfigurationResolverArgs : AbstractPipelineArgs
    {
        public ITypeContext TypeContext { get; private set; }
        public Type Type { get; private set; }
        public AbstractTypeConfiguration Result { get; set; }

        public ConfigurationResolverArgs(Context context, ITypeContext typeContext, Type type) :base(context)
        {
            TypeContext = typeContext;
            Type = type;
        }
    }
}
