namespace Glass.Mapper.Umb.CastleWindsor
{
    public class Config
    {
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
