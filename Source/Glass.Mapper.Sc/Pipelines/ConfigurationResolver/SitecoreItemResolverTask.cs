using System;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Pipelines.ConfigurationResolver
{
    public class SitecoreItemResolverTask : IConfigurationResolverTask
    {

        static SitecoreItemResolverTask()
        {
            ItemType = typeof (Item);
            Config = new SitecoreTypeConfiguration()
            {
                Type = ItemType
            };
        }
        public static Type ItemType { get; private set; }
        public  static SitecoreTypeConfiguration Config { get; private set; }

        public void Execute(ConfigurationResolverArgs args)
        {
            if (args.Result == null && args.RequestedType == ItemType)
            {
                args.Result = Config;
            }
        }
    }
}
