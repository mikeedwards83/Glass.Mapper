using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    
    public class SitecoreFieldIEnumerableMapper : AbstractSitecoreFieldMapper
    {
        AbstractSitecoreFieldMapper _mapper;

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Type type = config.PropertyInfo.PropertyType;
            //Get generic type
            Type pType = Utilities.GetGenericArgument(type);

            //The enumerator only works with piped lists
            IEnumerable<string> parts = fieldValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            //replace any pipe encoding with an actual pipe
            parts = parts.Select(x => x.Replace(Global.PipeEncoding, "|")).ToArray();


            //fake field
            
            //IEnumerable<object> items = parts.Select(x => _mapper.GetFieldValue(x, item, service)).ToArray();
            //var list = Utility.CreateGenericType(typeof(List<>), new Type[] { pType });
            //Utility.CallAddMethod(items.Where(x => x != null), list);

    //        return list;
            return null;
        }

        public override void SetFieldValue(Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            var scConfig = configuration as SitecoreFieldConfiguration;


            Type type = scConfig.PropertyInfo.PropertyType;

            if (!type.IsGenericType) return false;

            if (type.GetGenericTypeDefinition() != typeof(IEnumerable<>) && type.GetGenericTypeDefinition() != typeof(IList<>))
                return false;

            return true;
        }

        public override void Setup(DataMapperResolverArgs args)
        {
            base.Setup(args);

            var scConfig = Configuration as SitecoreFieldConfiguration;

            var property = args.PropertyConfiguration.PropertyInfo;
            var type = Utilities.GetGenericArgument(property.PropertyType);

            var configCopy = scConfig.Copy();
            configCopy.PropertyInfo = new FakePropertyInfo(type, property.Name);

            _mapper =
                args.DataMappers.FirstOrDefault(
                    x => x.CanHandle(configCopy, args.Context) && x is AbstractSitecoreFieldMapper) 
                    as AbstractSitecoreFieldMapper;

            if (_mapper == null)
                throw new MapperException(
                    "No mapper to handle type {0} on property {1} class {2}".Formatted(type.FullName, property.Name,
                                                                                       property.ReflectedType.FullName));


        }
    }
}
