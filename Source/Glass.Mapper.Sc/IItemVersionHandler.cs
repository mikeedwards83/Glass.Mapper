using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public interface IItemVersionHandler
    {
        bool VersionCountEnabled();

        bool HasVersions(Item item);

        bool VersionCountEnabledAndHasVersions(Item item);
    }
}
