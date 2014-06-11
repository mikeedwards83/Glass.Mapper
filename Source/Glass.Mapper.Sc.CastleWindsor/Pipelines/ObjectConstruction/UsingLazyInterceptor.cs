using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.CastleWindsor.Pipelines.ObjectConstruction
{
    public class UsingLazyInterceptor : IDisposable
    {
        private const string _key = "AF6D68E8-32F0-4951-80B4-930277D8CD6C";

        public static bool UseLazyInterceptor
        {
            get { return Sitecore.Context.Items.Contains(_key); }
        }

        public UsingLazyInterceptor()
        {
            Sitecore.Context.Items[_key] = true;
        }

        public void Dispose()
        {

           Sitecore.Context.Items.Remove(_key);
        }
    }
}
