using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Builders;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc
{
    public class GetItemsByQueryOptions : GetItemsOptions
    {
        public Query Query { get; set; }
        public Item RelativeItem { get; set; }
        public bool IsRelative { get { return RelativeItem != null; } }
        public Language Language { get; set; }

        public GetItemsByQueryOptions() { }
        public GetItemsByQueryOptions(Query query)
        {
            Query = query;
        }

        public override IEnumerable<Item> GetItems(Database database)
        {
            IEnumerable<Item> foundItems = null;

            if (IsRelative)
            {
                foundItems = RelativeItem.Axes.SelectItems(Query.Value);
            }
            else
            {
                foundItems = database.SelectItems(Query.Value);
            }

            if (Language != null)
            {
                foundItems = GetLanguageItems(foundItems,Language);
            }

            return GetLanguageItems(foundItems, Language);
        }

        public override void Copy(GetOptions other)
        {
            var local = other as GetItemsByQueryOptions;
            if (local != null)
            {
                this.Query = local.Query;
            }
            base.Copy(other);
        }
        public override void Validate()
        {
            if (Query.IsNullOrEmpty())
            {
                throw new NullReferenceException("Query parameter does not have a value");
            }
            base.Validate();
        }
    }

    public class GetItemsByFuncOptions : GetItemsOptions
    {
        public Func<Database, IEnumerable<Item>> ItemsFunc { get; set; }

        public GetItemsByFuncOptions() { }
        public GetItemsByFuncOptions(Func<Database, IEnumerable<Item>> itemsFunc)
        {
            ItemsFunc = itemsFunc;
        }

        public override IEnumerable<Item> GetItems(Database database)
        {
            return ItemsFunc(database);
        }

        public override void Copy(GetOptions other)
        {
            var local = other as GetItemsByFuncOptions;
            if (local != null)
            {
                this.ItemsFunc = local.ItemsFunc;
            }
            base.Copy(other);
        }
    }

    public class GetItemsOptions : GetOptionsSc
    {
        public virtual IEnumerable<Item> GetItems(Database database)
        {
            throw new NotImplementedException();
        }

        public override void Copy(GetOptions other)
        {
            var local = other as GetItemsOptions;
            if (local != null)
            {
            }

            base.Copy(other);
        }


    }
  
}
