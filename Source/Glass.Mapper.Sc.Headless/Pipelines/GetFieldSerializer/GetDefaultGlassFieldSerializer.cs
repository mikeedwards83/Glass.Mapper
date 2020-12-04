using Glass.Mapper.Sc.LayoutService.Serialization;
using Glass.Mapper.Sc.LayoutService.Serialization.FieldSerializers;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Serialization.FieldSerializers;

namespace Glass.Mapper.Sc.Pipelines.GetFieldSerializer
{
    public class GetDefaultGlassFieldSerializer : BaseGetGlassFieldSerializer
    {
        public GetDefaultGlassFieldSerializer(IGlassFieldRenderer fieldRenderer, ILog log)
            : base(fieldRenderer, log)
        {
        }

        public override void Process(GetGlassFieldSerializerPipelineArgs args)
        {
            Assert.ArgumentNotNull((object)args, nameof(args));
            this.SetResult(args);
        }

        protected override void SetResult(GetGlassFieldSerializerPipelineArgs args)
        {
            args.Result = (IGlassFieldSerializer)new GlassTextFieldSerializer(this.FieldRenderer, this.Log);
            args.AbortPipeline();
        }
    }
}