namespace Glass.Mapper.Sc.Builders
{


    public class MoveItemByModelBuilder : AbstractMoveItemByModelBuilder<MoveItemByModelBuilder>
    {
        public MoveItemByModelBuilder() 
            : base(new MoveByModelOptions())
        {
        }

        public static implicit operator MoveByModelOptions(MoveItemByModelBuilder builder)
        {
            return builder.Options;
        }
    }

    public abstract class AbstractMoveItemByModelBuilder<T> : IMoveItemBuilder where T : class, IMoveItemBuilder
    {
        private readonly MoveByModelOptions _options;

        public MoveByModelOptions Options => _options;

        public AbstractMoveItemByModelBuilder(MoveByModelOptions options)
        {
            _options = options;
        }

        public  T Model<K>( K model)
        {
            _options.Model = model;
            return this as T;
        }
        public  T NewParent<K>( K parent) 
        {
            _options.NewParent = parent;
            return this as T;
        }
    }




}
