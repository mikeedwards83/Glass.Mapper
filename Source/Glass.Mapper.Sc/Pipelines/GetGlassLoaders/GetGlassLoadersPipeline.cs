using System;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Pipelines.GetGlassLoaders
{
	public class GetGlassLoadersPipeline
	{
		private const string PipelineName = "glassMapper.getGlassLoaders";

		public static void Run(GetGlassLoadersPipelineArgs args)
		{
			if (args == null) throw new ArgumentNullException(nameof(args));

			var pipeline = CorePipelineFactory.GetPipeline(PipelineName, string.Empty);
			pipeline.Run(args);
		}
	}
}
