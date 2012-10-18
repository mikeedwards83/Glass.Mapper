using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ConfigurationResolver
{
    public class ConfigurationResolverArgs : AbstractPipelineArgs
    {
        public IDataContext DataContext { get; private set; }
        public Type Type { get; private set; }
        public AbstractTypeConfiguration Result { get; set; }

        public ConfigurationResolverArgs(Context context, IDataContext dataContext, Type type) :base(context)
        {
            DataContext = dataContext;
            Type = type;
        }
    }
}
