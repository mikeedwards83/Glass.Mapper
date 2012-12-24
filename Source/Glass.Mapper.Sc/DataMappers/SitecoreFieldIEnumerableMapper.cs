using System;
using System.Collections;
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
        public AbstractSitecoreFieldMapper Mapper { get; private set; }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Type type = config.PropertyInfo.PropertyType;
            //Get generic type
            Type pType = Utilities.GetGenericArgument(type);

            //The enumerator only works with piped lists
            IEnumerable<string> parts = fieldValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            //replace any pipe encoding with an actual pipe
            parts = parts.Select(x => x.Replace(Global.PipeEncoding, "|")).ToArray();
            
            IEnumerable<object> items = parts.Select(x => Mapper.GetFieldValue(x, config, context)).ToArray();
            var list = Utilities.CreateGenericType(typeof (List<>), new Type[] {pType}) as IList;
            
            foreach (var item in items)
            {
                if(item != null)
                    list.Add(item);
            }

            return list;
        }

        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {

            IEnumerable list = value as IEnumerable;

            if (list == null)
            {
                return string.Empty;
            }

            List<string> sList = new List<string>();


            foreach (object obj in list)
            {
                string result = Mapper.SetFieldValue(obj, config, context);
                if (!result.IsNullOrEmpty())
                    sList.Add(result);
            }
            if (sList.Any())
                return sList.Aggregate((x, y) => x + "|" + y);
            else
                return string.Empty;
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            var scConfig = configuration as SitecoreFieldConfiguration;

            if (scConfig == null)
                return false;

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

            Mapper =
                args.DataMappers.FirstOrDefault(
                    x => x.CanHandle(configCopy, args.Context) && x is AbstractSitecoreFieldMapper) 
                    as AbstractSitecoreFieldMapper;

            if (Mapper == null)
                throw new MapperException(
                    "No mapper to handle type {0} on property {1} class {2}".Formatted(type.FullName, property.Name,
                                                                                       property.ReflectedType.FullName));


        }
    }
}
