using System;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc.Builders
{
    public class CreateItemByModelBuilder : AbstractCreateItemByModelBuilder<CreateItemByModelBuilder>
    {
        public CreateItemByModelBuilder()
            : base(new CreateByModelOptions())
        {
        }

        public static implicit operator CreateOptions(CreateItemByModelBuilder builder)
        {
            return builder.Options;
        }
    }

    public class CreateItemByNameBuilder : AbstractCreateItemByNameBuilder<CreateItemByNameBuilder>
    {
        public CreateItemByNameBuilder() 
            : base(new CreateByNameOptions())
        {
        }

        public static implicit operator CreateOptions(CreateItemByNameBuilder builder)
        {
            return builder.Options;
        }
    }

    public abstract class AbstractCreateItemByNameBuilder<T> : AbstractCreateItemBuilder<T> where T : class, ICreateItemBuilder
    {
        private CreateByNameOptions _options;

        public AbstractCreateItemByNameBuilder(CreateByNameOptions options)
            : base(options)
        {
            _options = options;
        }

        public  T Name( string name) 
        {
            _options.Name = name;
            return this as T;
        }

        public  T Language( Language language) 
        {
            _options.Language = language;
            return this as T;
        }
    }

    public abstract class AbstractCreateItemByModelBuilder<T> : AbstractCreateItemBuilder<T> where T : class, ICreateItemBuilder
    {
        private CreateByModelOptions _options;

        public AbstractCreateItemByModelBuilder(CreateByModelOptions options)
            : base(options)
        {
            _options = options;
        }

        public T Model<K>(K model)
        {
            _options.Model = model;
            _options.Type = typeof(K);
            return this as T;
        }
    }

    public abstract class AbstractCreateItemBuilder<T>: ICreateItemBuilder where T : class, ICreateItemBuilder
    {
        private CreateOptions _options;

        public CreateOptions Options => _options;

        public AbstractCreateItemBuilder(CreateOptions options)
        {
            _options = options;
        }

        public  T Parent<K>( K parent) 
        {
            _options.Parent = parent;
            return this as T;
        }

        public  T Type( Type type) 
        {
            _options.Type = type;
            return this as T;
        }

        public T UpdateStatistics(bool value)
        {
            _options.UpdateStatistics = value;
            return this as T;
        }


        public T Silent(bool value)
        {
            _options.Silent = value;
            return this as T;
        }
    }
}
