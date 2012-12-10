using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{


    public class LazyObjectInterceptor : IInterceptor
    {
        private readonly ObjectConstructionArgs _args;


        private object _actual = null;

        public LazyObjectInterceptor(ObjectConstructionArgs args)
        {
            _args = args;
        }

      
        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            //create class
            if (_actual == null)
            {
                
                //TODO: ME - this isn't correct. We have to send it through the pipeline again somehow
                var serviceType = _args.Service.GetType();


                MethodInfo method =  serviceType.GetMethod("InstantiateObject", BindingFlags.Public | BindingFlags.FlattenHierarchy);

                _args.AbstractTypeCreationContext.IsLazy = false;
              
                _actual =  method.Invoke(_args.Service, new object[] { _args.AbstractTypeCreationContext});
            }

            invocation.ReturnValue = invocation.Method.Invoke(_actual, invocation.Arguments);
        }

        #endregion


    }
}


