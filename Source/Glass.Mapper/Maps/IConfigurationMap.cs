using Glass.Mapper.Configuration;

namespace Glass.Mapper.Maps
{
    public interface IConfigurationMap
    {
        T GetConfigurationLoader<T>() where T : class, IConfigurationLoader, new();
    }
}