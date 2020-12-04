using Glass.Mapper.Sc.Fields;
using Newtonsoft.Json;

namespace Glass.Mapper.Sc.LayoutService.Serialization.FieldSerializers
{
    public interface IGlassFieldSerializer
    {
        /// <summary>Serializes the field value to JSON</summary>
        void Serialize(GlassField field, JsonTextWriter writer);

        /// <summary>
        /// Whether to return the FieldRendered field value ('editable') for experience editor support
        /// </summary>
        bool EnableRenderedValues { get; set; }
    }
}