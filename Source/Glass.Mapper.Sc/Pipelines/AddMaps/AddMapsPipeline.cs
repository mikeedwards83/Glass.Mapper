using System;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Pipelines.AddMaps
{
	public class AddMapsPipeline
	{
		private const string PipelineName = "glassMapper.addMaps";

		public static void Run(AddMapsPipelineArgs args)
		{
			if (args == null) throw new ArgumentNullException(nameof(args));

			var pipeline = CorePipelineFactory.GetPipeline(PipelineName, string.Empty);
			pipeline.Run(args);
		}
	}
}
