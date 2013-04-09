using System.Linq;
using Castle.Core;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Umb.CastleWindsor.Pipelines.ObjectConstruction
{
    /// <summary>
    /// WindsorConstruction
    /// </summary>
    public class WindsorConstruction : IObjectConstructionTask
    {
        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null)
                return;

            if (args.AbstractTypeCreationContext.ConstructorParameters == null || 
                        !args.AbstractTypeCreationContext.ConstructorParameters.Any())
            {
                var resolver = args.Context.DependencyResolver as DependencyResolver;
                if (resolver != null)
                {
                    var type = args.Configuration.Type;
                    var container = resolver.Container;

                    if (type.IsClass)
                    {
                        if(!container.Kernel.HasComponent(type))
                            container.Kernel.AddComponent(type.FullName, type, LifestyleType.Transient);

                        args.Result = container.Resolve(type);
                        
                        if(args.Result != null)
                            args.Configuration.MapPropertiesToObject(args.Result, args.Service, args.AbstractTypeCreationContext);
                    }
                }
            }
        }
    }
}
