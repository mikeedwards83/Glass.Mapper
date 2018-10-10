using System;
using Glass.Mapper.Sc.Builders;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Version = Sitecore.Data.Version;

namespace Glass.Mapper.Sc
{
    public class GetItemOptions : GetOptionsSc
    {
    }

    public class GetItemOptionsParams : GetItemOptions
    {
        public virtual Language Language { get; set; }
        public virtual Sitecore.Data.Version Version { get; set; }

        public override void Copy(GetOptions other)
        {
            var local = other as GetItemOptionsParams;
            if (local != null)
            {
                this.Language = local.Language;
                this.Version = local.Version;
            }
            base.Copy(other);
        }
    }

    public class GetItemByIdOptions : GetItemOptionsParams
    {
        public Guid Id { get; set; }

        public GetItemByIdOptions()
        {

        }

        public GetItemByIdOptions(Guid id)
        {
            Id = id;
        }

        public override Item GetItem(Database database)
        {
            if (Language == null)
            {
                return database.GetItem(new ID(Id));
            }
            if (Version == null)
            {
                return database.GetItem(new ID(Id), Language);
            }

            return database.GetItem(new ID(Id), Language, Version);
        }
        public override void Copy(GetOptions other)
        {
            var local = other as GetItemByIdOptions;
            if (local != null)
            {
                this.Id = local.Id;
            }
            base.Copy(other);
        }
    }

    public class GetItemByPathOptions : GetItemOptionsParams
    {
        public string Path { get; set; }

        public GetItemByPathOptions()  {
        }
        public GetItemByPathOptions(string path)
        {
            Path = path;
        }

        public override Item GetItem(Database database)
        {
            if (Language == null)
            {
                return database.GetItem(Path);
            }
            if (Version == null)
            {
                return database.GetItem(Path, Language);
            }
            return database.GetItem(Path, Language, Version);
        }

        public override void Copy(GetOptions other)
        {
            var local = other as GetItemByPathOptions;
            if (local != null)
            {
                this.Path = local.Path;
            }
            base.Copy(other);
        }
    }

    public class GetItemByQueryOptions : GetItemOptionsParams
    {
        public Query Query { get; set; }
        public Item RelativeItem { get; set; }
        public bool IsRelative { get { return RelativeItem != null; } }

        public GetItemByQueryOptions()
        {

        }

        public GetItemByQueryOptions(Query query)
        {
            Query = query;
        }


        public override Item GetItem(Database database)
        {
            Item item = null;

            if (IsRelative)
            {
                item = RelativeItem.Axes.SelectSingleItem(Query.Value);
            }
            else
            {
                item= database.SelectSingleItem(Query.Value);
            }

            return GetLanguageItem(item, Language);
        }
        public override void Validate()
        {
            if (Query.IsNullOrEmpty())
            {
                throw new NullReferenceException("Query parameter does not have a value");
            }
            base.Validate();
        }

        public override void Copy(GetOptions other)
        {
            var local = other as GetItemByQueryOptions;
            if (local != null)
            {
                this.Query = local.Query;
                this.RelativeItem = local.RelativeItem;
            }
            base.Copy(other);
        }
    }

    public class GetItemByItemOptions : GetItemOptions
    {
        public Item Item { get; set; }

        public GetItemByItemOptions() { }

        public GetItemByItemOptions(Item item)
        {
            Item = item;
        }

        public override Item GetItem(Database database)
        {
            return Item;
        }
        public override void Copy(GetOptions other)
        {
            var local = other as GetItemByItemOptions;
            if (local != null)
            {
                this.Item = local.Item;
            }
            base.Copy(other);
        }

        public override void Validate()
        {
            base.Validate();
        }
    }

    public class GetKnownOptions : GetItemByItemOptions
    {
        public override void Copy(GetOptions other)
        {
            var local = other as GetKnownOptions;
            if (local != null)
            {
            }
            base.Copy(other);
        }

        public override void Validate()
        {
            base.Validate();
        }

    }
    




































    }
