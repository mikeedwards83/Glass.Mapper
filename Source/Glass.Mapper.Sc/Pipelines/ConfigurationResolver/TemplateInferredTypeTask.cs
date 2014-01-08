/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

using System;
using System.Linq;
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
                    var templateId = item != null ? item.TemplateID : scContext.TemplateId;

                    var configs = args.Context.TypeConfigurations.Select(x => x.Value as SitecoreTypeConfiguration);

                    var types = configs.Where(x => x.TemplateId == templateId);
                    if (types.Any())
                    {
                        var type = types.FirstOrDefault(x => requestedType.First().IsAssignableFrom(x.Type));
                        if (type != null) args.Result = new[] { type };
                    }
                }
            }
        }

        #endregion
    }
}

