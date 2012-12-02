using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.DataMapperResolver.Tasks
{
    public class DataMapperStandardResolverTask : IDataMapperResolverTask
    {
        public void Execute(DataMapperResolverArgs args)
        {
            if (args.Result == null)
            {
                var mapper = args.DataMappers.FirstOrDefault(x => x.CanHandle(args.PropertyConfiguration));
                
                if(mapper == null) 
                    throw new MapperException("Could not find data mapper to handler property {0}".Formatted(args.PropertyConfiguration));

                args.Result = mapper;
            }
        }
    }
}
