using System;
using System.Collections.Generic;
using System.Linq;
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
                //i am not sure about this at the moment
                CreateConcreteTask task = new CreateConcreteTask();

                _args.IsLazy = false;

                task.Execute(_args);
                _actual = _args.Object;
            }

            invocation.ReturnValue = invocation.Method.Invoke(_actual, invocation.Arguments);

        }

        #endregion


    }
}


