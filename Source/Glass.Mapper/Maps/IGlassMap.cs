using Glass.Mapper.Configuration;

namespace Glass.Mapper.Maps
{
    /// <summary>
    /// The sitecore version of the glass mapp
    /// </summary>
    /// <typeparam name="T">The type to be mapped</typeparam>
    public interface IGlassMap<out T> : IGlassMap
    {
        T GlassType { get; }
    }

    public interface IGlassMap
    {
        void PerformMap<TLoader>(TLoader mappingContainer) where TLoader : class, IConfigurationLoader;
    }
}