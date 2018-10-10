using Glass.Mapper.Sc.IoC;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Pipelines.PostLoad
{
	public class PostLoadPipelineArgs : PipelineArgs
	{
		public IDependencyResolver DependencyResolver { get; set; }
	}
}
