using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreLinkedMapper
    /// </summary>
    public class SitecoreLinkedMapper : AbstractDataMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreLinkedMapper"/> class.
        /// </summary>
        public SitecoreLinkedMapper()
        {
            ReadOnly = true;
        }


        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>The value to write</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>System.Object.</returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreLinkedConfiguration;
            var scContext = mappingContext as SitecoreDataMappingContext;

            var item = scContext.Item;

            var linkDb = global::Sitecore.Globals.LinkDatabase;

            var options = new GetItemsByFuncOptions();


            options.Copy(mappingContext.Options);
            scConfig.GetPropertyOptions(options);
           

            options.ItemsFunc = new Func<Database, IEnumerable<Item>>(database =>
            {
                //ME - i am not sure this is correct but there is an odd behaviour of references
                // languges come back as invariant, going with default language in this scenario
                var references = new Func<IEnumerable<Item>>(() =>
                {
                    var itemLinks = linkDb.GetReferences(item);
                    return itemLinks.Select(x => x.GetTargetItem());
                });

                IEnumerable<Item> items;

                switch (scConfig.Option)
                {

                    case SitecoreLinkedOptions.All:
                        var itemLinks1 = references();
                        var itemLinks2 = linkDb.GetReferrers(item);
                        items= itemLinks1.Union(itemLinks2.Select(x => x.GetSourceItem()));
                        break;
                    case SitecoreLinkedOptions.References:
                        items= references();
                        break;
                    case SitecoreLinkedOptions.Referrers:
                        var itemLinks4 = linkDb.GetReferrers(item);
                        items= itemLinks4.Select(x => x.GetSourceItem());
                        break;
                    default:
                        items= new List<Item>();
                        break;
                }

                return items;
            });


            var result = scContext.Service.GetItems(options);
            return result;
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            if (!(configuration is SitecoreLinkedConfiguration))
                return false;

            if (!configuration.PropertyInfo.PropertyType.IsGenericType) return false;

            Type outerType = Utilities.GetGenericOuter(configuration.PropertyInfo.PropertyType);

            return typeof (IEnumerable<>) == outerType;// && context.TypeConfigurations.ContainsKey(innerType);
        }
    }
}




