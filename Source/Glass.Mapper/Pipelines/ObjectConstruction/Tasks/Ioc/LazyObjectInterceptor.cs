using System;
using Castle.DynamicProxy;
using Glass.Mapper.Diagnostics;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Ioc
{


    /// <summary>
    /// Class LazyObjectInterceptor
    /// </summary>
    public class LazyObjectInterceptor : IInterceptor
    {
        public Action<object> MappingAction { get; set; }
        private bool _isMapped = false;


     
        public LazyObjectInterceptor(Action<object> mappingAction, ObjectConstructionArgs args)
        {
            MappingAction = (obj) =>
            {
                ModelCounter.Instance.ProxyModelsCreated++;
                mappingAction(obj);
            };
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






