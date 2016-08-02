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
using System.Linq;
using Castle.Core;
using Castle.MicroKernel.Registration;
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
                    var config = args.Configuration;
                    var type = config.Type;
                    var container = resolver.Container;

                    if (type.IsClass)
                    {
                        if(!container.Kernel.HasComponent(type))
                            container.Kernel.Register(Component.For(type).Named(type.FullName).LifeStyle.Is(LifestyleType.Transient));

                        args.Result = container.Resolve(type);
                        
                        if(args.Result != null)
                            config.MapPropertiesToObject(args.Result, args.Service, args.AbstractTypeCreationContext);
                    }
                }
            }
        }
    }
}

