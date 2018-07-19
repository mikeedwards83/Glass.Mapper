using System;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Pipelines.CreateResolver
{
	public class CreateResolverPipeline
	{
		private const string PipelineName = "glassMapper.createResolver";

		public static void Run(CreateResolverPipelineArgs args)
		{
			if (args == null) throw new ArgumentNullException(nameof(args));

			var pipeline = CorePipelineFactory.GetPipeline(PipelineName, string.Empty);
			pipeline.Run(args);
		}
	}
}
