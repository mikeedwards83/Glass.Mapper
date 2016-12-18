using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Pipelines.DataMapperResolver.Tasks
{
    public class DataMapperAttributeResolverTask : AbstractDataMapperResolverTask
    {

        public const string ErrorNoConstructor = "Specified data mapper {0} does not have a default constructor. {1}";

        public DataMapperAttributeResolverTask()
        {
            Name = "DataMapperAttributeResolverTask";
        }

        public override void Execute(DataMapperResolverArgs args)
        {

            if (args.Result == null)
            {
                AbstractDataMapper mapper;

                var propertyInfo = args.PropertyConfiguration.PropertyInfo;

                var attribute  = propertyInfo.GetCustomAttribute<DataMapperAttribute>();

                if (attribute == null || attribute.DataMapperType == null)
                {
                    Next(args);
                    return;
                }

                var mapperType = attribute.DataMapperType;
                var isAbstractDataMapper = typeof(AbstractDataMapper).IsAssignableFrom(attribute.DataMapperType);
                if (!isAbstractDataMapper)
                {
                    throw new MapperException(
                        "Specified data mapper {0} does not inherit from AbstractDataMapper. {1}".Formatted(mapperType.FullName, args.PropertyConfiguration));
                }

                // Look through registered mappers first
                mapper = args.DataMappers.FirstOrDefault(x => x.GetType() == mapperType);

                if (mapper == null)
                {
                    // Create new instance using the default constructor
                    var constructor = mapperType.GetConstructor(Type.EmptyTypes);
                    if (constructor == null)
                    {
                        throw new NotSupportedException(
                            ErrorNoConstructor.Formatted(mapperType.FullName, args.PropertyConfiguration));
                    }

                    mapper = (AbstractDataMapper)Activator.CreateInstance(mapperType);
                }

                if (!mapper.CanHandle(args.PropertyConfiguration, args.Context))
                {
                    throw new MapperException(
                        "Specified data mapper {0} cannot handle this property. {1}".Formatted(mapperType.FullName, args.PropertyConfiguration));
                }

                args.Result= mapper;
                return;
            }
        }

    }
}
