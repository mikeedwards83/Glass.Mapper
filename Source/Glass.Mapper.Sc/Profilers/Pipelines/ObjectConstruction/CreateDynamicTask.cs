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

using System.Dynamic;
using System.Linq;
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
                if (
                    args.Configurations != null && 
                    args.Configurations.Any() && 
                    args.Configurations.First().Type.IsAssignableFrom(typeof(IDynamicMetaObjectProvider))) 
                {
                    SitecoreTypeCreationContext typeContext =
                      args.AbstractTypeCreationContext as SitecoreTypeCreationContext;
                    args.Result = new DynamicItem(typeContext.Item);
                }
            }
        }
    }
}

