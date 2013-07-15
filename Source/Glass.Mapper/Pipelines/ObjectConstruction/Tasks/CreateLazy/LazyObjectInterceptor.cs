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
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateLazy
{


    /// <summary>
    /// Class LazyObjectInterceptor
    /// </summary>
    public class LazyObjectInterceptor : IInterceptor
    {
        private readonly AbstractObjectFactory _factory;
        private readonly AbstractTypeCreationContext _typeCreationContext;
        private object _actual;

        internal Func<AbstractObjectFactory, AbstractTypeCreationContext, object> CreateConcrete =
            (factory, typeCreationContext )=> factory.InstantiateObject(typeCreationContext);
 

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyObjectInterceptor"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public LazyObjectInterceptor(ObjectConstructionArgs args):
            this(args.Service.ObjectFactory, args.AbstractTypeCreationContext)
        {
        }
        
        /// <summary>
        /// Used for unit tests
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="typeCreationContext"></param>
        internal LazyObjectInterceptor(AbstractObjectFactory factory, AbstractTypeCreationContext typeCreationContext)
        {
            _factory = factory;
            _typeCreationContext = typeCreationContext;
        }
      
        #region IInterceptor Members

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            //create class
            if (_actual == null)
            {
                _typeCreationContext.IsLazy = false;
                _actual = CreateConcrete(_factory, _typeCreationContext);
            }

            invocation.ReturnValue = invocation.Method.Invoke(_actual, invocation.Arguments);
        }

        #endregion


    }
}






