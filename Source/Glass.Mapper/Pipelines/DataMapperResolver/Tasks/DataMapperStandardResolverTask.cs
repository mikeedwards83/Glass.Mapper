using System.Linq;

namespace Glass.Mapper.Pipelines.DataMapperResolver.Tasks
{
    /// <summary>
    /// Class DataMapperStandardResolverTask
    /// </summary>
    public class DataMapperStandardResolverTask : AbstractDataMapperResolverTask
    {

        public DataMapperStandardResolverTask()
        {
            Name = "DataMapperStandardResolverTask";
        }

        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <exception cref="Glass.Mapper.MapperException">Could not find data mapper to handle property {0}.Formatted(args.PropertyConfiguration)</exception>
        public override void Execute(DataMapperResolverArgs args)
        {
            if (args.Result == null)
            {
                var mapper = args.DataMappers.FirstOrDefault(x => x.CanHandle(args.PropertyConfiguration, args.Context));

                if(mapper == null)
                    throw new MapperException("Could not find a data mapper to handle property {0}".Formatted(args.PropertyConfiguration));

                mapper.Setup(args);
                args.Result = mapper;
            }
            base.Execute(args);
        }

      
    }
}




