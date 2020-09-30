
using System.Collections.Generic;
using Glass.Mapper.Sc.RenderField;

namespace Glass.Mapper.Sc
{
    public class Config : Glass.Mapper.Config
    {
        public Config()
        {
            ForceItemInPageEditor = true;
            UseGlassHtmlLambdaCache = true;

            ImageAttributes = new HashSet<string>(new[]
           {
                ImageParameterKeys.BORDER,
                ImageParameterKeys.ALT,
                ImageParameterKeys.HSPACE,
                ImageParameterKeys.VSPACE,
                ImageParameterKeys.CLASS,
                ImageParameterKeys.WIDTHHTML,
                ImageParameterKeys.HEIGHTHTML
            });
            ImageQueryString = new HashSet<string>(new[]
            {
                ImageParameterKeys.OUTPUT_METHOD,
                ImageParameterKeys.ALLOW_STRETCH,
                ImageParameterKeys.IGNORE_ASPECT_RATIO,
                ImageParameterKeys.SCALE,
                ImageParameterKeys.MAX_WIDTH,
                ImageParameterKeys.MAX_HEIGHT,
                ImageParameterKeys.THUMBNAIL,
                ImageParameterKeys.BACKGROUND_COLOR,
                ImageParameterKeys.DATABASE,
                ImageParameterKeys.LANGUAGE,
                ImageParameterKeys.VERSION,
                ImageParameterKeys.DISABLE_MEDIA_CACHE,
                ImageParameterKeys.WIDTH,
                ImageParameterKeys.HEIGHT
            });
        }


        public HashSet<string> ImageAttributes { get; private set; }
        public HashSet<string> ImageQueryString { get; private set; }


        public bool ForceItemInPageEditor { get; set; }

        /// <summary>
        /// When using Lazy Enumerables this setting indicates that proxy objects
        /// should be used. Be default this is false.
        /// </summary>
        public bool UseProxiesForLazyEnumerables { get; set; }

        /// <summary>
        /// Indicates it the lambda expressions used by Editable expression in renderings are cached for performance.
        /// By default this is true, if you experience caching issue then set this to false.
        /// </summary>
        public bool UseGlassHtmlLambdaCache { get; set; }

        /// <summary>
        /// Fast version count checks the revision number of the item rather than the item.Version.Count.
        /// The revision number is blank when an item 
        /// </summary>
        public bool UseFastVesionCount { get; set; }


        /// <summary>
        /// Backward compatibility flag, default is false. If set to true, items used to create temporary rendering parameter items
        /// by the RenderingParametersModelFactory will call the item.Delete() method. This has been disabled because in Sitecore 9.2+
        /// it causes excessive logging by Sitecore, see issue https://github.com/mikeedwards83/Glass.Mapper/issues/402
        /// </summary>
        public bool DeleteRenderingParameterItems { get; set; }
    }
}
