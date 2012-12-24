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
        public override object GetFieldValue(Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            //Type type = Property.PropertyType;
            ////Get generic type
            //Type pType = Utility.GetGenericArgument(type);

            //if (EnumSubHandler == null) EnumSubHandler = GetSubHandler(pType, service);

            ////The enumerator only works with piped lists
            //IEnumerable<string> parts = fieldValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            ////replace any pipe encoding with an actual pipe
            //parts = parts.Select(x => x.Replace(Settings.PipeEncoding, "|")).ToArray();



            //IEnumerable<object> items = parts.Select(x => EnumSubHandler.GetFieldValue(x, item, service)).ToArray();
            //var list = Utility.CreateGenericType(typeof(List<>), new Type[] { pType });
            //Utility.CallAddMethod(items.Where(x => x != null), list);

            //return list;
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
            var property = args.PropertyConfiguration.PropertyInfo;
            var type = property.PropertyType.GetGenericArguments()[0];
            
           // service.
           base.Setup(args);
        }
    }
}
