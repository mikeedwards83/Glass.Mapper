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
using System;

namespace Glass.Mapper.Sc.Pipelines.ObjectConstruction
{
    /// <summary>
    /// CreateDynamicTask
    /// </summary>
    public class CreateDynamicTask : AbstractObjectConstructionTask
    {
        private static readonly Type _dynamicType;

           static CreateDynamicTask()
        {
            _dynamicType = typeof(IDynamicMetaObjectProvider);
        }

        public CreateDynamicTask()
        {
            Name = "CreateDynamicTask";
        }
        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null &&
                args.Configuration != null &&
                args.Configuration.Type.IsAssignableFrom(_dynamicType))
            {
                SitecoreTypeCreationContext typeContext =
                  args.AbstractTypeCreationContext as SitecoreTypeCreationContext;
                args.Result = new DynamicItem(typeContext.Item);
            }

            base.Execute(args);
        }
    }
}

