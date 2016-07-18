namespace Glass.Mapper.Configuration
{
    public class SelfConfiguration : AbstractPropertyConfiguration
    {
        public SelfConfiguration()
        {
            IsLazy = true;
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is lazy.
        /// </summary>
        /// <value><c>true</c> if this instance is lazy; otherwise, <c>false</c>.</value>
        public bool IsLazy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [infer type].
        /// </summary>
        /// <value><c>true</c> if [infer type]; otherwise, <c>false</c>.</value>
        public bool InferType { get; set; }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SelfConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SelfConfiguration;

            config.IsLazy = IsLazy;
            config.InferType = InferType;

            base.Copy(copy);
        }
    }
}