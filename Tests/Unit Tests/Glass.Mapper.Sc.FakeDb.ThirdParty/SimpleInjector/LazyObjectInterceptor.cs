using System;
using Castle.DynamicProxy;

namespace Glass.Mapper.Sc.FakeDb.ThirdParty.SimpleInjector
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
                        MappingAction(invocation.InvocationTarget);
                        
                    }
                }
            }
            invocation.Proceed();
        }

        #endregion


    }
}






