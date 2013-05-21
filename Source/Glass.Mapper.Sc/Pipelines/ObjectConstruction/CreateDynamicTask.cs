using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.Dynamic;

namespace Glass.Mapper.Sc.Pipelines.ObjectConstruction
{
    /// <summary>
    /// CreateDynamicTask
    /// </summary>
    public class CreateDynamicTask : IObjectConstructionTask
    {
        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null)
            {
                if (args.Configuration.Type.IsAssignableFrom(typeof(IDynamicMetaObjectProvider))) 
                {
                    SitecoreTypeCreationContext typeContext =
                      args.AbstractTypeCreationContext as SitecoreTypeCreationContext;
                    args.Result = new DynamicItem(typeContext.Item);
                }
            }
        }
    }
}
