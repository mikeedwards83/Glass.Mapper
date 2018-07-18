using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc.Builders
{
 

    public class GetItemsByFuncBuilder : AbstractGetItemsByFuncBuilder<GetItemsByFuncBuilder>
    {
        public GetItemsByFuncBuilder()
            : base(new GetItemsByFuncOptions())
        {
        }
        public static implicit operator GetItemsOptions(GetItemsByFuncBuilder builder)
        {
            return builder.Options;
        }
    }

    public class GetItemsByQueryBuilder : AbstractGetItemsByQueryBuilder<GetItemsByQueryBuilder> {

        public GetItemsByQueryBuilder() 
            : base(new GetItemsByQueryOptions())
        {
        }
        public static implicit operator GetItemsOptions(GetItemsByQueryBuilder builder)
        {
            return builder.Options;
        }

    }


    public abstract class AbstractGetItemsByQueryBuilder<T> : AbstractGetItemsBuilder<T> where T : class, IGetItemsBuilder
    {
        private GetItemsByQueryOptions _options;

        public AbstractGetItemsByQueryBuilder(GetItemsByQueryOptions options)
            : base(options)
        {
            _options = options;
        }

        public T Query(string query)
        {
            _options.Query = Sc.Query.New(query);
            return this as T;

        }

        public T RelativeTo(Item item)
        {
            _options.RelativeItem = item;
            return this as T;
        }

        public T Language(Language language)
        {
            _options.Language = language;
            return this as T;
        }
    }

    public abstract class AbstractGetItemsByFuncBuilder<T> : AbstractGetItemsBuilder<T> where T : class, IGetItemsBuilder
    {
        private GetItemsByFuncOptions _options;

        public AbstractGetItemsByFuncBuilder(GetItemsByFuncOptions options) 
            : base(options)
        {
            _options = options;
        }

        public  T Func(Func<Database, IEnumerable<Item>> items) 
        {
            _options.ItemsFunc = items;
            return this as T;
        }
    }

    public abstract class AbstractGetItemsBuilder<T> : AbstractGetOptionsBuilder<T>, IGetItemsBuilder where T : class, IGetItemsBuilder
    {
        private GetItemsOptions _options;

        public GetItemsOptions Options => _options;

        public AbstractGetItemsBuilder(GetItemsOptions options) 
            :base(options)
        {
            _options = options;
        }

        public T EnforceTemplate(SitecoreEnforceTemplate enforceTemplate =SitecoreEnforceTemplate.Template) 
        {
            _options.EnforceTemplate = enforceTemplate;
            return this as T;
        }

        public T EnforceTemplateAndBase()
        {
            return EnforceTemplate(SitecoreEnforceTemplate.TemplateAndBase) as T;
        }

        public T EnforceTemplateDisabled()
        {
            return EnforceTemplate(SitecoreEnforceTemplate.No) as T;
        }

        public T VersionCountDisabled()
        {
            _options.VersionCount = false;
            return this as T;
        }

        public T VersionCount(bool value = true)
        {
            _options.VersionCount = value;
            return this as T;
        }
       
    }
}
