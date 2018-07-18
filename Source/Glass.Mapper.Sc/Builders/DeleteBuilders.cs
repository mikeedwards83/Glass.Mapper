namespace Glass.Mapper.Sc.Builders
{
    public class DeleteItemByModelBuilder : AbstractDeleteItemByModelBuilder<DeleteItemByModelBuilder>
    {
        public DeleteItemByModelBuilder() 
            : base(new DeleteByModelOptions())
        {
        }

        public static implicit operator DeleteByModelOptions(DeleteItemByModelBuilder builder)
        {
            return builder.Options;
        }
    }

    public abstract class AbstractDeleteItemByModelBuilder<T> : IDeleteItemBuilder where T : class, IDeleteItemBuilder
    {
        private readonly DeleteByModelOptions _options;

        public DeleteByModelOptions Options => _options;

        protected AbstractDeleteItemByModelBuilder(DeleteByModelOptions options)
        {
            _options = options;
        }

        public T Model< K>( K target) 
        {
            _options.Model = target;
            return this as T;
        }
    }

}
