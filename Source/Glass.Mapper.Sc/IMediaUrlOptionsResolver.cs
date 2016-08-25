using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc
{
    public interface IMediaUrlOptionsResolver
    {
        MediaUrlOptions GetMediaUrlOptions(SitecoreInfoMediaUrlOptions mediaUrlOptions);

    }
}
