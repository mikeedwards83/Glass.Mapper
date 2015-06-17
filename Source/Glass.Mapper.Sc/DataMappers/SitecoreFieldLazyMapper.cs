using System;
using System.Linq;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    public abstract class SitecoreFieldLazyMapper : AbstractSitecoreFieldMapper
    {
        protected SitecoreFieldLazyMapper(Type[] typesHandled)
            : base(typesHandled)
        {
            
        }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        public AbstractSitecoreFieldMapper Mapper { get; internal set; }

        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return Mapper.SetFieldValue(value, config, context);
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            Mapper.MapToCms(mappingContext);
        }

        protected virtual Lazy<T> GetLazy<T>(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            // todo: make this not generic dependant
            return new Lazy<T>(() => (T)Mapper.GetFieldValue(fieldValue, config, context));
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            Type type = typeof (Lazy<>);
            return IsSubClassOfGeneric(type, configuration.PropertyInfo.PropertyType) && TypesHandled.Contains(configuration.PropertyInfo.PropertyType);
        }

        public static object DoWork(AbstractSitecoreFieldMapper mapper, string fieldValue, SitecoreFieldConfiguration config,
            SitecoreDataMappingContext context)
        {
            return mapper.GetFieldValue(fieldValue, config, context);
        }

        protected virtual bool IsSubClassOfGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        public override void Setup(DataMapperResolverArgs args)
        {
            base.Setup(args);

            var scConfig = Configuration as SitecoreFieldConfiguration;

            var property = args.PropertyConfiguration.PropertyInfo;
            var type = property.PropertyType;

            if (scConfig == null)
            {
                return;
            }

            var configCopy = scConfig.Copy();
            configCopy.PropertyInfo = new FakePropertyInfo(type, property.Name, property.DeclaringType);

            Mapper =
                args.DataMappers.FirstOrDefault(
                    x => x.CanHandle(configCopy, args.Context) && !(x is SitecoreFieldLazyMapper)) as AbstractSitecoreFieldMapper;


            if (Mapper == null)
                throw new MapperException(
                    "No mapper to handle type {0} on property {1} class {2}".Formatted(type.FullName, property.Name,
                                                                                       property.ReflectedType.FullName));

            Mapper.Setup(new DataMapperResolverArgs(args.Context, configCopy));
        }
    }
}