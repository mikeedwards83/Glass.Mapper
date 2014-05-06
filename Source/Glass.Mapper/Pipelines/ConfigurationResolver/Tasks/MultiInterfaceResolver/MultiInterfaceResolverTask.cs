using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface;

namespace Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.MultiInterfaceResolver
{
    /// <summary>
    /// Used to resolve all configurations for multiple interface proxy type
    /// </summary>
    public class MultiInterfaceResolverTask : IConfigurationResolverTask
    {
        /// <summary>
        /// Key used to add multi interface types to the parameters dictionary
        /// </summary>
        public const string MultiInterfaceTypesKey = "A2453E13-1EAA-47A9-9F42-6BCF8D645188";

        public void Execute(ConfigurationResolverArgs args)
        {
            if (args.Parameters != null && args.Parameters.ContainsKey(MultiInterfaceTypesKey) && args.RequestedType.IsInterface)
            {
                var types = args.Parameters[MultiInterfaceTypesKey] as IEnumerable<Type>;
               
                if (types!= null && types.All(x => x.IsInterface))
                {
                    var typeContext = args.AbstractTypeCreationContext;
                    var originalType = typeContext.RequestedType;
                    args.Parameters.Remove(MultiInterfaceTypesKey);
                    var configuations = new List<AbstractTypeConfiguration>();
                    foreach (var type in types)
                    {
                        typeContext.RequestedType = type;
                        var result = args.Service.RunConfigurationPipeline(typeContext);
                        configuations.Add(result.Result);
                    }
                    args.Parameters[CreateMultiInferaceTask.MultiInterfaceConfigsKey] = configuations;
                    args.Parameters[MultiInterfaceTypesKey] = types;
                    typeContext.RequestedType = originalType;
                }
            }
        }
    }
}
