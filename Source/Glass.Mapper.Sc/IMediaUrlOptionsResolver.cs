using Glass.Mapper.Sc.Configuration;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc
{
    public interface IMediaUrlOptionsResolver
    {
        MediaUrlOptions GetMediaUrlOptions(SitecoreInfoMediaUrlOptions mediaUrlOptions);
        MediaUrlOptions GetMediaUrlOptions(SitecoreMediaUrlOptions mediaUrlOptions);
    }
}
