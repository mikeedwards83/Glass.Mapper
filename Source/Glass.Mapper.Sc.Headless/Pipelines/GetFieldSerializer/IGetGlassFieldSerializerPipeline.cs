using Glass.Mapper.Sc.LayoutService.Serialization.FieldSerializers;

namespace Glass.Mapper.Sc.Pipelines.GetFieldSerializer
{
    public interface IGetGlassFieldSerializerPipeline
    {
        IGlassFieldSerializer GetResult(GetGlassFieldSerializerPipelineArgs args);
    }
}