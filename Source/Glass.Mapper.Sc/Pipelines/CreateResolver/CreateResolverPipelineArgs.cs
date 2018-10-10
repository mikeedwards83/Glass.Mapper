using Glass.Mapper.Sc.IoC;
using Sitecore.Pipelines;

namespace Glass.Mapper.Sc.Pipelines.CreateResolver
{
	public class CreateResolverPipelineArgs : PipelineArgs
	{
		public IDependencyResolver DependencyResolver { get; set; }
	}
}
