using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.Pipelines.ConfigurationResolver
{
    /// <summary>
    /// TemplateInferredTypeTask
    /// </summary>
    public class TemplateInferredTypeTask : IConfigurationResolverTask
    {
        #region IPipelineTask<ConfigurationResolverArgs> Members

        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public void Execute(ConfigurationResolverArgs args)
        {
            if (args.Result == null)
            {
                if (args.AbstractTypeCreationContext.InferType)
                {
                    var scContext = args.AbstractTypeCreationContext as SitecoreTypeCreationContext;

                    var requestedType = scContext.RequestedType;
                    var item = scContext.Item;
                    var templateId = item.TemplateID;

                    var configs = args.Context.TypeConfigurations.Select(x => x.Value as SitecoreTypeConfiguration);

                    var types = configs.Where(x => x.TemplateId == templateId);
                    args.Result = types.FirstOrDefault(x => requestedType.IsAssignableFrom(x.Type));
                }
            }
        }

        #endregion
    }
}
