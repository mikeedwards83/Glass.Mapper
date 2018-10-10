using Glass.Mapper.IoC;
using Glass.Mapper.Maps;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Pipelines.AddMaps
{
	public class AddMapsPipelineArgs : PipelineArgs
	{
		public IConfigFactory<IGlassMap> MapsConfigFactory { get; set; }
	}
}
