using Sitecore.LayoutService.Serialization;

namespace Glass.Mapper.Sc.Serialization.ItemSerializers
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