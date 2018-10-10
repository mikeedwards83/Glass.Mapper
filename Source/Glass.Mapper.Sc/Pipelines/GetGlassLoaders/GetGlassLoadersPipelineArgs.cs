using System.Collections.Generic;
using Glass.Mapper.Configuration;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Pipelines.GetGlassLoaders
{
	public class GetGlassLoadersPipelineArgs : PipelineArgs
	{
		public List<IConfigurationLoader> Loaders { get; set; }
	    public Context Context { get; set; }

	    public GetGlassLoadersPipelineArgs()
		{
			Loaders = new List<IConfigurationLoader>();
		}
	}
}
