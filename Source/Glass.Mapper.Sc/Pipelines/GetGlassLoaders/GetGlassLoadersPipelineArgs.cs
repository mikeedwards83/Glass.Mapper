using System.Collections.Generic;
using Glass.Mapper.Configuration;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Pipelines.GetGlassLoaders
{
	public class GetGlassLoadersPipelineArgs : PipelineArgs
	{
		public List<IConfigurationLoader> GlassLoaders { get; set; }

		public GetGlassLoadersPipelineArgs()
		{
			GlassLoaders = new List<IConfigurationLoader>();
		}
	}
}
