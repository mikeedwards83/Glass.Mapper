using System.Linq;
using System.Net.Mail;
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface
{
    public class CreateMultiInferaceTask : IObjectConstructionTask
    {
        private static volatile ProxyGenerator _generator;

              /// <summary>
        /// Initializes static members of the <see cref="CreateInterfaceTask"/> class.
        /// </summary>
        static CreateMultiInferaceTask()
        {
            _generator = new ProxyGenerator();
        }

        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null)
                return;

            if (args.Configurations.All(x=>x.Type.IsInterface) && args.Configurations.Count() > 1)
            {
                args.Result = _generator.CreateInterfaceProxyWithoutTarget(
                    args.Configurations.First().Type,
                    args.Configurations.Skip(1).Select(x=>x.Type).ToArray(),
                    new MultiInterfacePropertyInterceptor(args));
            }
        }
    }
}
