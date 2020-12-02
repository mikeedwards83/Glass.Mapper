using Glass.Mapper.Sc.Serialization.ItemSerializers;
using Sitecore.LayoutService.Configuration;

namespace Glass.Mapper.Sc.LayoutService.Extensions
{
    public static class RenderingConfigurationExtensions
    {
        public static IGlassItemSerializer GlassItemSerializer(this IRenderingConfiguration renderingConfiguration)
        {
            return new GlassItemSerializer();
        }
    }
}