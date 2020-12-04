using Glass.Mapper.Sc.LayoutService.Serialization.FieldSerializers;
using Sitecore.Abstractions;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Configuration;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Pipelines.GetFieldSerializer
{
    public class GetGlassFieldSerializerPipeline : IGetGlassFieldSerializerPipeline
    {
        public const string Name = "glassMapper.getFieldSerializer";
        protected readonly BaseCorePipelineManager PipelineManager;
        protected readonly IConfiguration Configuration;

        public GetGlassFieldSerializerPipeline(
            BaseCorePipelineManager pipelineManager,
            IConfiguration configuration)
        {
            Assert.ArgumentNotNull((object)pipelineManager, nameof(pipelineManager));
            Assert.ArgumentNotNull((object)configuration, nameof(configuration));
            this.PipelineManager = pipelineManager;
            this.Configuration = configuration;
        }

        public IGlassFieldSerializer GetResult(GetGlassFieldSerializerPipelineArgs args)
        {
            Assert.ArgumentNotNull((object)args, nameof(args));
            this.PipelineManager.Run("glassMapper.getFieldSerializer", (PipelineArgs)args, this.Configuration.PipelineGroupName);
            return args.Result;
        }
    }
   
}