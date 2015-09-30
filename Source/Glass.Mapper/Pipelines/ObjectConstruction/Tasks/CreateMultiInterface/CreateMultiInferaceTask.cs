using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface
{
    /// <summary>
    /// Creates a Castle Proxy to wrap all requested interfaces
    /// </summary>
    public class CreateMultiInferaceTask : IObjectConstructionTask
    {

        /// <summary>
        /// Key used to add multi interface configs to the parameters dictionary
        /// </summary>
        public const string MultiInterfaceConfigsKey = "9F1A4CFC-3DC5-4CD5-939B-A928D8D6A3B6";
        private static volatile ProxyGenerator _generator;

              /// <summary>
        /// Initializes static members of the <see cref="CreateMultiInferaceTask"/> class.
        /// </summary>
        static CreateMultiInferaceTask()
        {
            _generator = new ProxyGenerator();
        }

        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null 
                && args.Configuration.Type.IsInterface 
                && args.Parameters.ContainsKey(MultiInterfaceConfigsKey))
            {
                var configs = args.Parameters[MultiInterfaceConfigsKey] as IEnumerable<AbstractTypeConfiguration>;

                if (configs != null)
                {
                    args.Result = _generator.CreateInterfaceProxyWithoutTarget(
                        args.Configuration.Type,
                        configs.Select(x => x.Type).ToArray(),
                        new MultiInterfacePropertyInterceptor(args));
                }
            }
        }
    }
}
