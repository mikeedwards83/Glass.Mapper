namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Class QueryConfiguration
    /// </summary>
    public class QueryConfiguration : AbstractNodePropertyConfiguration
    {
        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is relative.
        /// </summary>
        /// <value><c>true</c> if this instance is relative; otherwise, <c>false</c>.</value>
        public bool IsRelative { get; set; }

        public QueryConfiguration() : base(true)
        {

        }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new QueryConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as QueryConfiguration;

            config.Query = Query;
            config.IsRelative = IsRelative;

            base.Copy(copy);
        }
    }
}




