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

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Ioc
{


    /// <summary>
    /// Class LazyObjectInterceptor
    /// </summary>
    public class LazyObjectInterceptor : IInterceptor
    {
        public Action<object> MappingAction { get; set; }
        private bool _isMapped = false;


     
        public LazyObjectInterceptor(Action<object> mappingAction)
        {
            MappingAction = mappingAction;
        }

        #region IInterceptor Members

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            //create class
            if (invocation.InvocationTarget != null && _isMapped == false)
            {
                lock (invocation.InvocationTarget)
                {
                    if (_isMapped == false)
                    {
                        _isMapped = true;

                        if (MappingAction == null)
                        {
                            throw new NullReferenceException("MappingAction is null");
                        }

                        MappingAction(invocation.InvocationTarget);

                        //release anything that was used for the mapping
                        MappingAction = null;
                    }
                }
            }
            invocation.Proceed();
        }

        #endregion


    }
}






