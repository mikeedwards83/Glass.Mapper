namespace Glass.Mapper.Umb.CastleWindsor
{
    /// <summary>
    /// Config
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        public Config()
        {
            UseWindsorContructor = false;
        }

        /// <summary>
        /// Indicates that classes should be build using the Windsor dependency resolver. Default is False
        /// </summary>
        public bool UseWindsorContructor { get; set; }
    }
}
