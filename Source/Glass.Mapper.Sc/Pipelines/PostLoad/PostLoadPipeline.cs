using System;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Pipelines.PostLoad
{
	public class PostLoadPipeline
	{
		private const string PipelineName = "glassMapper.postLoad";

		public static void Run(PostLoadPipelineArgs args)
		{
			if (args == null) throw new ArgumentNullException(nameof(args));

			var pipeline = CorePipelineFactory.GetPipeline(PipelineName, string.Empty);
			pipeline.Run(args);
		}
	}
}
