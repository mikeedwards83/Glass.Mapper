using Sitecore.LayoutService.Serialization;

namespace Glass.Mapper.Sc.LayoutService.Serialization.ItemSerializers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGlassItemSerializer
    {
        string Serialize<T>(T item) where T : class;

        string Serialize<T>(T item, SerializationOptions options) where T : class;
    }
}