using System;
using System.Collections.Concurrent;
using System.Linq;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Shell.Framework.Commands;

namespace Glass.Mapper.Sc.Pipelines.ConfigurationResolver
{
    /// <summary>
    /// TemplateInferredTypeTask
    /// </summary>
    public class TemplateInferredTypeTask : AbstractConfigurationResolverTask
    {
        static ConcurrentDictionary<Tuple<Context, Type, ID>, SitecoreTypeConfiguration>
            _inferredCache = new ConcurrentDictionary<Tuple<Context, Type, ID>, SitecoreTypeConfiguration>();


        public TemplateInferredTypeTask()
        {
            Name = "TemplateInferredTypeTask";
        }

        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Execute(ConfigurationResolverArgs args)
        {
            if (args.Result == null && args.AbstractTypeCreationContext.Options.InferType)
            {
                var scContext = args.AbstractTypeCreationContext as SitecoreTypeCreationContext;

                var requestedType = scContext.Options.Type;
                var item = scContext.Item;
                var templateId = item != null ? item.TemplateID : scContext.TemplateId;

                var key = new Tuple<Context, Type, ID>(args.Context, requestedType, templateId);
                if (_inferredCache.ContainsKey(key))
                {
                    args.Result = _inferredCache[key];
                }
                else
                {
                    var configs = args.Context.TypeConfigurations.Select(x => x.Value as SitecoreTypeConfiguration);

                    var types = configs.Where(x => x.TemplateId == templateId);
                    if (types.Any())
                    {
                        args.Result = types.FirstOrDefault(x => requestedType.IsAssignableFrom(x.Type));
                        if (!_inferredCache.TryAdd(key, args.Result as SitecoreTypeConfiguration))
                        {
                            //TODO: some logging
                        }
                    }
                }
            }

            base.Execute(args);
        }


    }
}

