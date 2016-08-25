using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreLazyMapper : AbstractDataMapper
    {
        protected AbstractDataMapper Mapper { get; set; }
        protected Type InnerType { get;  set; }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            

        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            Func<object> arg1 = () =>  Mapper.MapToProperty(mappingContext);
            return  Glass.Mapper.Utilities.CreateGenericType(typeof (GlassLazy<>), new[] { InnerType}, arg1);
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            Type type = configuration.PropertyInfo.PropertyType;

            if (!type.IsGenericType) return false;

            if (type.GetGenericTypeDefinition() != typeof(GlassLazy<>))
                return false;

            return true;
        }

        public override void Setup(DataMapperResolverArgs args)
        {
            base.Setup(args);

            var property = args.PropertyConfiguration.PropertyInfo;
            InnerType = Glass.Mapper.Utilities.GetGenericArgument(property.PropertyType);

            var configCopy = Configuration.Copy();
            configCopy.PropertyInfo = new FakePropertyInfo(InnerType, property.Name, property.DeclaringType);

            Mapper =
                args.DataMappers.FirstOrDefault(x => x.CanHandle(configCopy, args.Context) );

            if (Mapper == null)
                throw new MapperException(
                    "No mapper to handle type {0} on property {1} class {2}".Formatted(InnerType.FullName, property.Name,
                                                                                       property.ReflectedType.FullName));

            Mapper.Setup(new DataMapperResolverArgs(args.Context, configCopy));
        }
    }
}
