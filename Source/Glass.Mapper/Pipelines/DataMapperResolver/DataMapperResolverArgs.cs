using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.DataMapperResolver
{
    public class DataMapperResolverArgs : AbstractPipelineArgs
    {
        /// <summary>
        /// The configuration of the property to load
        /// </summary>
        public Configuration.AbstractPropertyConfiguration PropertyConfiguration { get; set; }

        /// <summary>
        /// The data mapper to use when loading the property
        /// </summary>
        public AbstractDataMapper Result { get; set; }

        public DataMapperResolverArgs(Context context, Configuration.AbstractPropertyConfiguration configuration)
            : base(context)
        {
            PropertyConfiguration = configuration;
        }
        /// <summary>
        /// A list of all the data mappers loaded by the current context
        /// </summary>
        public IEnumerable<AbstractDataMapper> DataMappers { get; set; }

        
    }
}
