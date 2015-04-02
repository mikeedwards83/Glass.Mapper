namespace Glass.Mapper.Sc
{
    public class Config
    {
        public Config()
        {
            ForceItemInPageEditor = true;
            UseGlassHtmlLambdaCache = true;
        }
        public bool ForceItemInPageEditor { get; set; }

        /// <summary>
        /// Indicates it the lambda expressions used by Editable expression in renderings are cached for performance.
        /// By default this is true, if you experience caching issue then set this to false.
        /// </summary>
        public bool UseGlassHtmlLambdaCache { get; set; }
    }
}
