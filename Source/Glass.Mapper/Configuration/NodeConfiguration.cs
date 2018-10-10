namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Class NodeConfiguration
    /// </summary>
    public class NodeConfiguration : AbstractNodePropertyConfiguration
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        public NodeConfiguration() : base(false)
        {

        }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new NodeConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as NodeConfiguration;

            config.Id = Id;
            config.Path = Path;

            base.Copy(copy);
        }
    }
}




