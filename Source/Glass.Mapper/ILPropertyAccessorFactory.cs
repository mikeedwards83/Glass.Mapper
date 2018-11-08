using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
   public class ILPropertyAccessorFactory : IPropertyAccessorFactory
    {
        private IPropertyAccessorFactory _lambdaFactory = new LambdaPropertyAccessorFactory();


        public  Func<object, object> GetPropertyFunc(PropertyInfo property)
        {
            if (property.CanRead)
            {
                var type = property.DeclaringType;
                var getMethod = property.GetGetMethod();

                if (getMethod == null)
                {
                    return _lambdaFactory.GetPropertyFunc(property);
                }

                DynamicMethod dmGet = new DynamicMethod("Get", typeof(object),
                    new Type[] { typeof(object), });


                ILGenerator ilGet = dmGet.GetILGenerator();

                // Load first argument to the stack

                ilGet.Emit(OpCodes.Ldarg_0);
                // Cast the object on the stack to the apropriate type

                ilGet.Emit(OpCodes.Castclass, type);

                // Call the getter method passing the object on the stack as the this reference

                ilGet.Emit(OpCodes.Callvirt, getMethod);

                // If the property type is a value type (int/DateTime/..)

                // box the value so we can return it


                if (property.PropertyType.IsValueType)
                {

                    ilGet.Emit(OpCodes.Box, property.PropertyType);
                }
                // Return from the method

                ilGet.Emit(OpCodes.Ret);
                // Getter dynamic method the signature would be :
                // { return ((TestClass)thisReference).Prop = (PropType)propValue; 

                return (Func<object, object>)dmGet.CreateDelegate(typeof(Func<object, object>));
            }
            else
            {
                return (object instance) => { return null; };
            }
        }


        public Action<object, object> SetPropertyAction(PropertyInfo property)
        {
            if (property.CanWrite)
            {
                var setMethod = property.GetSetMethod();

                if (setMethod == null)
                {
                    return _lambdaFactory.SetPropertyAction(property);
                }

                var type = property.DeclaringType;

                DynamicMethod dmSet = new DynamicMethod("Set", typeof(void),
                    new Type[] { typeof(object), typeof(object) });

                ILGenerator ilSet = dmSet.GetILGenerator();

                // Load first argument to the stack and cast it

                ilSet.Emit(OpCodes.Ldarg_0);

                ilSet.Emit(OpCodes.Castclass, type);



                // Load secons argument to the stack and cast it or unbox it

                ilSet.Emit(OpCodes.Ldarg_1);

                if (property.PropertyType.IsValueType)

                {
                    ilSet.Emit(OpCodes.Unbox_Any, property.PropertyType);
                }

                else
                {
                    ilSet.Emit(OpCodes.Castclass, property.PropertyType);
                }

                // Call Setter method and return

                ilSet.Emit(OpCodes.Callvirt, property.SetMethod);

                ilSet.Emit(OpCodes.Ret);
                return (Action<object, object>)dmSet.CreateDelegate(typeof(Action<object, object>));
            }
            else
            {
                return (object instance, object value) =>
                {
                    //does nothing
                };
            }
        }

    }
}
