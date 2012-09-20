using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface
{
    public class InterfacePropertyInterceptor : IInterceptor
    {
        public InterfacePropertyInterceptor(ObjectConstructionArgs args)
        {
        }
        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
