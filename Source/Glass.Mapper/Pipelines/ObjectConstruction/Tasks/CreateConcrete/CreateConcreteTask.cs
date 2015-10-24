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
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{
    /// <summary>
    /// Class CreateConcreteTask
    /// </summary>
    public class CreateConcreteTask : IObjectConstructionTask
    {
        private const string ConstructorErrorMessage = "No constructor for class {0} with parameters {1}";

        private static volatile  ProxyGenerator _generator;
        private static volatile  ProxyGenerationOptions _options;

        /// <summary>
        /// Initializes static members of the <see cref="CreateConcreteTask"/> class.
        /// </summary>
        static CreateConcreteTask()
        {
            _generator = new ProxyGenerator();
            var hook = new LazyObjectProxyHook();
            _options = new ProxyGenerationOptions(hook);
        }

        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null 
                || args.Configuration == null 
                || args.Configuration.Type.IsInterface
		        || args.Configuration.Type.IsSealed)
                return;

            if(args.AbstractTypeCreationContext.IsLazy)
            {
                //here we create a lazy loaded version of the class
                args.Result = CreateLazyObject(args);
               // args.AbortPipeline();

            }
            else
            {
                //here we create a concrete version of the class
                args.Result = CreateObject(args);
                //args.AbortPipeline();
            }
        }

        /// <summary>
        /// Creates the lazy object.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>System.Object.</returns>
        protected virtual object CreateLazyObject(ObjectConstructionArgs args)
        {
            return  _generator.CreateClassProxy(args.Configuration.Type, new LazyObjectInterceptor(args));
        }

        /// <summary>
        /// Creates the object.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>System.Object.</returns>
        protected virtual object CreateObject(ObjectConstructionArgs args)
        {

            var constructorParameters = args.AbstractTypeCreationContext.ConstructorParameters;

            object obj;

            try
            {
                if (constructorParameters == null || constructorParameters.Length == 0)
                {
                    //conMethod = args.Configuration.DefaultConstructor;
                    obj = Activator.CreateInstance(args.Configuration.Type);
                }
                else
                {
                    var parameters = constructorParameters.Select(x => x.GetType()).ToArray();
                    var constructorInfo = args.Configuration.Type.GetConstructor(parameters);
                    Delegate conMethod = null;
                    conMethod = args.Configuration.ConstructorMethods[constructorInfo];
                    obj = conMethod.DynamicInvoke(constructorParameters);
                }

                args.Configuration.MapPropertiesToObject(obj, args.Service, args.AbstractTypeCreationContext);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to create type {0}".Formatted(args.Configuration.Type), ex);
            }
            return obj;
        }
    }
}




