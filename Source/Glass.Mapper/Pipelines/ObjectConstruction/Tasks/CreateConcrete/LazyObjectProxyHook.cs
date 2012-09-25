using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{
    public class LazyObjectProxyHook : IProxyGenerationHook
    {

        public void MethodsInspected()
        {
            //no idea what this method is used for
        }

        public void NonProxyableMemberNotification(Type type, System.Reflection.MemberInfo memberInfo)
        {
            throw new ObjectConstructionException("Can not proxy method {0} on class {1}.".Formatted(memberInfo.Name, type.FullName));
        }

        public bool ShouldInterceptMethod(Type type, System.Reflection.MethodInfo methodInfo)
        {
            //for interface we only support properties
            return !type.IsInterface;
        }
    }
}
