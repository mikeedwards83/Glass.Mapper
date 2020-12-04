using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.LayoutService.Serialization.FieldSerializers;
using Glass.Mapper.Sc.LayoutService.Serialization.ItemSerializers;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Pipelines.GetFieldSerializer
{
    public class GetGlassFieldSerializerPipelineArgs : PipelineArgs
    {
        public IGlassItemSerializer ItemSerializer { get; set; }

        public GlassField Field { get; set; }

        public IGlassFieldSerializer Result { get; set; }
    }
}