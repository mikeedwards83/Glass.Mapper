using Castle.DynamicProxy;
using Glass.Mapper.Diagnostics;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface
{
    /// <summary>
    /// Creates classes based on interfaces
    /// </summary>
    public class CreateInterfaceTask : AbstractObjectConstructionTask
    {
        private readonly LazyLoadingHelper _lazyLoadingHelper;
        private static volatile  ProxyGenerator _generator;

        /// <summary>
        /// Initializes static members of the <see cref="CreateInterfaceTask"/> class.
        /// </summary>
        static CreateInterfaceTask()
        {
            _generator = new ProxyGenerator();
        }

        public CreateInterfaceTask(
            LazyLoadingHelper lazyLoadingHelper
            )
        {
            _lazyLoadingHelper = lazyLoadingHelper;
            Name = "CreateInterfaceTask";
        }

        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Execute(ObjectConstructionArgs args)
        {
            if (args.Result== null 
                && args.Configuration.Type.IsInterface)
            {
                var interceptor = new InterfacePropertyInterceptor(
                    args,
                    _lazyLoadingHelper
                    );

                args.Result = _generator.CreateInterfaceProxyWithoutTarget(
                    args.Configuration.Type, interceptor
                    );

                ModelCounter.Instance.ProxyModelsCreated++;
            }

            base.Execute(args);
        }
    }
}




