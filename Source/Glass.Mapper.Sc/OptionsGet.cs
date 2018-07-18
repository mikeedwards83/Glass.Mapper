using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc
{
    public class GetOptionsSc : GetOptions
    {
        public SitecoreEnforceTemplate EnforceTemplate { get; set; }
        public bool VersionCount { get; set; }
        public ID TemplateId { get; set; }

        public GetOptionsSc()
        {
            VersionCount = true;
        }

        public virtual Item GetItem(Database database)
        {
            throw new NotImplementedException();
        }

        public override void Copy(GetOptions other)
        {
            var local = other as GetOptionsSc;
            if (local != null)
            {
                this.TemplateId = local.TemplateId;
                this.EnforceTemplate = local.EnforceTemplate;
                this.VersionCount = local.VersionCount;
            }
            base.Copy(other);
        }


        /// <summary>
        /// Gets the language item.
        /// </summary>
        /// <param name="foundItem">The found item.</param>
        /// <param name="language">The language.</param>
        /// <returns>Item.</returns>
        public static Item GetLanguageItem(Item foundItem, Language language)
        {

            Item item = null;

            if (foundItem != null)
            {
                if (language == null || foundItem.Language == language)
                {
                    item = foundItem;
                }
                else
                {
                    item = foundItem.Database.GetItem(foundItem.ID, language);
                }
            }

            return item;
        }

        /// <summary>
        /// Gets the language items.
        /// </summary>
        /// <param name="foundItems">The found items.</param>
        /// <param name="language">The language.</param>
        /// <param name="config"></param>
        /// <returns>IEnumerable{Item}.</returns>
        public static IEnumerable<Item> GetLanguageItems(IEnumerable<Item> foundItems, Language language)
        {
            if (foundItems == null) return Enumerable.Empty<Item>();

            return foundItems.Select(x => GetLanguageItem(x, language)).Where(x => x != null).ToArray();
        }
    }
}
