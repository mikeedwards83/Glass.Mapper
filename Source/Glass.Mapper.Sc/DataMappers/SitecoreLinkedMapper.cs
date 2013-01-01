using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreLinkedMapper : AbstractDataMapper
    {
        public SitecoreLinkedMapper()
        {
            ReadOnly = true;
        }


        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreLinkedConfiguration;
            var scContext = mappingContext as SitecoreDataMappingContext;

            Type genericType = Utilities.GetGenericArgument(scConfig.PropertyInfo.PropertyType);

            var item = scContext.Item;

            var getItems = new Func<IEnumerable<Item>>(() =>
            {

                switch (scConfig.Option)
                {
                    case SitecoreLinkedOptions.All:
                        var itemLinks1 = global::Sitecore.Configuration.Factory.GetLinkDatabase().GetReferences(item);
                        var itemLinks2 = global::Sitecore.Configuration.Factory.GetLinkDatabase().GetReferrers(item);
                        return itemLinks1.Select(x => x.GetTargetItem()).Union(itemLinks2.Select(x => x.GetSourceItem()));
                        break;
                    case SitecoreLinkedOptions.References:
                        var itemLinks3 = global::Sitecore.Configuration.Factory.GetLinkDatabase().GetReferences(item);
                        return itemLinks3.Select(x => x.GetTargetItem());
                        break;
                    case SitecoreLinkedOptions.Referrers:
                        var itemLinks4 = global::Sitecore.Configuration.Factory.GetLinkDatabase().GetReferrers(item);
                        return itemLinks4.Select(x => x.GetSourceItem());
                        break;
                    default:
                        return new List<Item>();
                }

            });

            return scContext.Service.CreateClasses(scConfig.IsLazy, scConfig.InferType, genericType, getItems);
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            if (!(configuration is SitecoreLinkedConfiguration))
                return false;

            if (!configuration.PropertyInfo.PropertyType.IsGenericType) return false;

            Type outerType = Utilities.GetGenericOuter(configuration.PropertyInfo.PropertyType);
            Type innerType = Utilities.GetGenericArgument(configuration.PropertyInfo.PropertyType);

            return typeof (IEnumerable<>) == outerType && context.TypeConfigurations.ContainsKey(innerType);
        }
    }
}
