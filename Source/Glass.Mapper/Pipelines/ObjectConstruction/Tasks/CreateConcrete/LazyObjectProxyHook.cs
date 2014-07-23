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

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{
    /// <summary>
    /// Class LazyObjectProxyHook
    /// </summary>
    public class LazyObjectProxyHook : IProxyGenerationHook
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
        /// <exception cref="Glass.Mapper.Pipelines.ObjectConstruction.ObjectConstructionException">Can not proxy method {0} on class {1}..Formatted(memberInfo.Name, type.FullName)</exception>
        /// <remarks>This method gives an opportunity to inspect any non-proxyable member of a type that has
        /// been requested to be proxied, and if appropriate - throw an exception to notify the caller.</remarks>
        public void NonProxyableMemberNotification(Type type, System.Reflection.MemberInfo memberInfo)
        {
            throw new ObjectConstructionException("Can not proxy method {0} on class {1}.".Formatted(memberInfo.Name, type.FullName));
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
            return !type.IsInterface;
        }
    }
}




