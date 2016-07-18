using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public interface IItemVersionHandler
    {
        bool VersionCountEnabled(Config config);

        bool HasVersions(Item item);

        bool VersionCountEnabledAndHasVersions(Item item, Config config);
    }
}
