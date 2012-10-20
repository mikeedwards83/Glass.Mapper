using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.DataMapperResolver
{
    public class DataMapperResolverArgs : AbstractPipelineArgs
    {
        public Configuration.AbstractPropertyConfiguration PropertyConfiguration { get; set; }

        public AbstractDataMapper Result { get; set; }

        public DataMapperResolverArgs(Context context, Configuration.AbstractPropertyConfiguration configuration)
            : base(context)
        {
            PropertyConfiguration = configuration;
        }
    }
}
