using System;
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface
{
    /// <summary>
    /// Class CreateInterfaceProxyHook
    /// </summary>
    public class CreateInterfaceProxyHook : IProxyGenerationHook
    {

        /// <summary>
        /// Invoked by the generation process to notify that the whole process has completed.
        /// </summary>
        public void MethodsInspected()
        {
            //no idea what this method is used for
        }

        /// <summary>
        /// Invoked by the generation process to notify that a member was not marked as virtual.
        /// </summary>
        /// <param name="type">The type which declares the non-virtual member.</param>
        /// <param name="memberInfo">The non-virtual member.</param>
        /// <exception cref="Glass.Mapper.Pipelines.ObjectConstruction.ObjectConstructionException">Can not proxy method {0} on interface {1}. Only properties of interface can be proxied.Formatted(memberInfo.Name, type.FullName)</exception>
        /// <remarks>This method gives an opportunity to inspect any non-proxyable member of a type that has
        /// been requested to be proxied, and if appropriate - throw an exception to notify the caller.</remarks>
        public void NonProxyableMemberNotification(Type type, System.Reflection.MemberInfo memberInfo)
        {
            throw new ObjectConstructionException("Can not proxy method {0} on interface {1}. Only properties of interface can be proxied".Formatted(memberInfo.Name, type.FullName));
        }

        /// <summary>
        /// Invoked by the generation process to determine if the specified method should be proxied.
        /// </summary>
        /// <param name="type">The type which declares the given method.</param>
        /// <param name="methodInfo">The method to inspect.</param>
        /// <returns>True if the given method should be proxied; false otherwise.</returns>
        public bool ShouldInterceptMethod(Type type, System.Reflection.MethodInfo methodInfo)
        {
            //for interface we only support properties
            return type.IsInterface && methodInfo.MemberType == System.Reflection.MemberTypes.Property;
        }
    }
}




