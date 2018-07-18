namespace Glass.Mapper.Sc.Builders
{
    public class SaveItemByModelBuilder : AbstractSaveItemByModelBuilder<SaveItemByModelBuilder>
    {
        public SaveItemByModelBuilder() 
            : base(new SaveOptions())
        {
        }

        public static implicit operator SaveOptions(SaveItemByModelBuilder builder)
        {
            return builder.Options;
        }
    }

    public abstract class AbstractSaveItemByModelBuilder<T> : AbstractWriteBuilder<T>, ISaveItemBuilder where T : class, ISaveItemBuilder
    {
        private SaveOptions _options;

        public SaveOptions Options => _options;

        protected AbstractSaveItemByModelBuilder(SaveOptions options):base(options)
        {
            _options = options;
        }

        public  T Model< K>( K model)
        {
            _options.Model = model;
            return this as T;
        }
    }

    public abstract class AbstractWriteBuilder<T> where T : class, ISaveItemBuilder
    {

        private WriteOptions _options;

        protected AbstractWriteBuilder(WriteOptions options)
        {
            _options = options;
        }

        public  T Silent(bool silent = true) 
        {
            _options.Silent = silent;
            return this as T;
        }

        public  T UpdateStatistics(bool updateStatistics = true) 
        {
            _options.UpdateStatistics = updateStatistics;
            return this as T;
        }



    }
}
