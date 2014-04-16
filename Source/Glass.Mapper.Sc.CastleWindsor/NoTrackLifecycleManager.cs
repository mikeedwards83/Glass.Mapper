using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace Glass.Mapper.Sc.CastleWindsor
{

    /// <summary>
    /// This manager deliberately does not track
    /// </summary>
    public class NoTrackLifestyleManager : AbstractLifestyleManager
    {
        public override void Dispose()
        {
        }

        public override object Resolve(CreationContext context, IReleasePolicy releasePolicy)
        {
            var localBurden = base.CreateInstance(context, trackedExternally: true);
            return localBurden.Instance;
        }
    }

  
}
