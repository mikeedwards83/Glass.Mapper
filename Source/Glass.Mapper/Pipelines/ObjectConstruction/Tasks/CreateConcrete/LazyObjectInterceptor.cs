using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{


    public class LazyObjectInterceptor : IInterceptor
    {
        private readonly ObjectConstructionArgs _args;

        private object _actual;

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
                _args.AbstractTypeCreationContext.IsLazy = false;
                _actual = _args.Service.InstantiateObject(_args.AbstractTypeCreationContext);
            }

            invocation.ReturnValue = invocation.Method.Invoke(_actual, invocation.Arguments);
        }

        #endregion


    }
}


