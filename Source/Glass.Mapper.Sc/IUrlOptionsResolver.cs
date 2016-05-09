using Glass.Mapper.Sc.Configuration;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc
{
    public interface IUrlOptionsResolver
    {
        /// <summary>
        /// Creates the URL options.
        /// </summary>
        /// <param name="urlOptions">The URL options.</param>
        /// <returns>UrlOptions.</returns>
        UrlOptions CreateUrlOptions(SitecoreInfoUrlOptions urlOptions);

        UrlOptions CreateUrlOptions(SitecoreInfoUrlOptions urlOptions, UrlOptions defaultOptions);

    }
}
