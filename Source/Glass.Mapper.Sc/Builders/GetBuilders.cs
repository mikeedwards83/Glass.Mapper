using System;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc.Builders
{

    public class GetKnownItemBuilder : AbstractGetKnownItemBuilder<GetKnownItemBuilder>, IGetItemBuilder
    {
        public GetKnownItemBuilder()
            : base(new GetKnownOptions())
        {
        }
        public static implicit operator GetKnownOptions(GetKnownItemBuilder builder)
        {
            return builder.Options as GetKnownOptions;
        }
    }


    public class GetItemByItemBuilder : AbstractGetItemByItemBuilder<GetItemByItemBuilder>, IGetItemBuilder
    {
        public GetItemByItemBuilder()
            : base(new GetItemByItemOptions())
        {
        }
        public static implicit operator GetItemOptions(GetItemByItemBuilder builder)
        {
            return builder.Options;
        }
    }

    public class GetItemByQueryBuilder : AbstractGetItemByQueryBuilder<GetItemByQueryBuilder>, IGetItemBuilder
    {
        public GetItemByQueryBuilder()
            : base(new GetItemByQueryOptions())
        {
        }
        public static implicit operator GetItemOptions(GetItemByQueryBuilder builder)
        {
            return builder.Options;
        }
    }

    public class GetItemByIdBuilder : AbstractGetItemByIdBuilder<GetItemByIdBuilder>
    {
        public GetItemByIdBuilder()
            : base(new GetItemByIdOptions())
        {
        }
        public static implicit operator GetItemOptions(GetItemByIdBuilder builder)
        {
            return builder.Options;
        }
    }

    public class GetItemByPathBuilder : AbstractGetItemByPathBuilder<GetItemByPathBuilder>
    {
        public GetItemByPathBuilder()
            : base(new GetItemByPathOptions())
        {
        }
        public static implicit operator GetItemOptions(GetItemByPathBuilder builder)
        {
            return builder.Options;
        }
    }


    public class GetItemBuilder : AbstractGetItemBuilder<GetItemBuilder>
    {
        public GetItemBuilder()
            : base(new GetItemOptions())
        {
        }
    }


    public abstract class AbstractGetItemByItemBuilder<T> : AbstractGetItemBuilder<T> where T : class, IGetItemBuilder
    {
        private GetItemByItemOptions _options;

        protected AbstractGetItemByItemBuilder(GetItemByItemOptions options)
            : base(options)
        {
            _options = options;
        }

        public T Item(Item item)
        {
            _options.Item = item;
            return this as T;
        }

      
    }

    public abstract class AbstractGetKnownItemBuilder<T> : AbstractGetItemBuilder<T> where T : class, IGetItemBuilder
    {
        private GetKnownOptions _options;

        protected AbstractGetKnownItemBuilder(GetKnownOptions options)
            : base(options)
        {
            _options = options;
        }

        public T Item(Item item)
        {
            _options.Item = item;
            return this as T;
        }


    }



    public abstract class AbstractGetItemByQueryBuilder<T> : AbstractGetParamsBuilder<T> where T : class, IGetItemBuilder
    {
        private GetItemByQueryOptions _options;

        protected AbstractGetItemByQueryBuilder(GetItemByQueryOptions options)
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
    }

    public abstract class AbstractGetItemByIdBuilder<T> : AbstractGetParamsBuilder<T> where T : class, IGetItemBuilder
    {
        private GetItemByIdOptions _options;

        protected AbstractGetItemByIdBuilder(GetItemByIdOptions options)
            : base(options)
        {
            _options = options;
        }

        public T Id(Guid id)
        {
            _options.Id = id;
            return this as T;
        }
    }

    public abstract class AbstractGetItemByPathBuilder<T> : AbstractGetParamsBuilder<T> where T : class, IGetItemBuilder
    {
        private GetItemByPathOptions _options;

        protected AbstractGetItemByPathBuilder(GetItemByPathOptions options)
            : base(options)
        {
            _options = options;
        }

        public T Path(string path)
        {
            _options.Path = path;
            return this as T;
        }

    }

    public abstract class AbstractGetParamsBuilder<T> : AbstractGetItemBuilder<T> where T : class, IGetItemBuilder
    {
        private GetItemOptionsParams _options;

        protected AbstractGetParamsBuilder(GetItemOptionsParams options)
            : base(options)
        {
            _options = options;
        }

        public T Language(Language language)
        {
            _options.Language = language;
            return this as T;
        }
        public T Version(Sitecore.Data.Version version)
        {
            _options.Version = version;
            return this as T;
        }
    }

    public abstract class AbstractGetItemBuilder<T> : AbstractGetOptionsBuilder<T>, IGetItemBuilder where T : class, IGetItemBuilder
    {
        private GetItemOptions _options;
        public GetItemOptions Options => _options;

        protected AbstractGetItemBuilder(GetItemOptions options)
            : base(options)
        {
            _options = options;
        }

        public T VersionCountDisable()
        {
            _options.VersionCount = false;
            return this as T;
        }
        public T VersionCount(bool value = true)
        {
            _options.VersionCount = value;
            return this as T;
        }

        public T EnforceTemplate(SitecoreEnforceTemplate enforceTemplate)
        {
            _options.EnforceTemplate = enforceTemplate;
            return this as T;
        }

      
    }


    public abstract class AbstractGetOptionsBuilder<T> where T : class
    {
        private GetOptions _options;

        protected AbstractGetOptionsBuilder(GetOptions options)
        {
            _options = options;
        }

        public T AddParam<TK>(TK parameter)
        {
            _options.ConstructorParameters.Add(new ConstructorParameter<TK>(parameter));
            return this as T;
        }

        public T AddParam(object parameter)
        {
            _options.ConstructorParameters.Add(new ConstructorParameter(parameter.GetType(), parameter));
            return this as T;
        }

        public T AddParams(params object []  parameters)
        {
            foreach (var parameter in parameters)
            {
                AddParam(parameter);
            }
            return this as T;
        }

        public T Lazy(LazyLoading lazyOption)
        {
            _options.Lazy = lazyOption;
            return this as T;
        }
       
        public T LazyDisabled()
        {
            return Lazy(LazyLoading.Disabled);
        }
        public T LazyEnabled()
        {
            return Lazy(LazyLoading.Enabled);
        }

        public T InferType(bool value = true)
        {
            _options.InferType = value;
            return this as T;

        }

        public T CacheEnabled()
        {
            _options.Cache = Mapper.Configuration.Cache.Enabled;
            return this as T;
        }
        public T CacheDisabled()
        {
            _options.Cache = Mapper.Configuration.Cache.Disabled;
            return this as T;
        }
        public T Cache(Cache cache)
        {
            _options.Cache = cache;
            return this as T;
        }
        public T Type(Type type)
        {
            _options.Type = type;
            return this as T;
        }

        public T As<T>() where T: class
        {
            return null;
        }
    }
}
