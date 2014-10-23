using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc.Integration
{
    public class GlassMediaProvider : MediaProvider
    {
        public override string GetMediaUrl(MediaItem item)
        {

            return item.ID.ToString() + "media";

            return base.GetMediaUrl(item);
        }
    }
}
