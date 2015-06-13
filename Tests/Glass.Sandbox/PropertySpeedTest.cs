using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Glass.Sandbox
{
    [TestFixture]
    public class PropertySpeedTest
    {
        [Test]
        public void SetProperty()
        {
            //Arrange
            var type = typeof (Stub);
            var property = type.GetProperty("Value");
            var stopWatch1 = new Stopwatch();
            var stopWatch2 = new Stopwatch();
            var stopWatch3 = new Stopwatch();
            var stopWatch4 = new Stopwatch();
            var lambda = Lambda(property);
            var value = "test";
            var il = CreateSetMethod(property);

            //Act

            var obj1 = new Stub();
            stopWatch1.Start();
            for (int i = 0; i < 50000; i++)
            {
                obj1.Value = value;
            }
            stopWatch1.Stop();

            var obj2 = new Stub();
            stopWatch2.Start();
            for (int i = 0; i < 50000; i++)
            {
                lambda(obj2, value);
            }
            stopWatch2.Stop();

            var obj3 = new Stub();
            stopWatch3.Start();
            for (int i = 0; i < 50000; i++)
            {
                il(obj3, value);
            }
            stopWatch3.Stop();


            var obj4 = new Stub();
            stopWatch4.Start();
            for (int i = 0; i < 50000; i++)
            {
                property.SetMethod.Invoke(obj4, new []{value});
            }
            stopWatch4.Stop();

            //Assert
            
            Console.WriteLine("Standard {0} lambda {1} il {2} setmethod {3}", stopWatch1.ElapsedTicks, stopWatch2.ElapsedTicks, stopWatch3.ElapsedTicks, stopWatch4.ElapsedTicks);



        }

        public delegate void GenericSetter(object target, object value);

        private static GenericSetter CreateSetMethod(PropertyInfo propertyInfo)
        {
            /*
            * If there's no setter return null
            */
            MethodInfo setMethod = propertyInfo.GetSetMethod();
            if (setMethod == null)
                return null;

            /*
            * Create the dynamic method
            */
            Type[] arguments = new Type[2];
            arguments[0] = arguments[1] = typeof(object);

            DynamicMethod setter = new DynamicMethod(
              String.Concat("_Set", propertyInfo.Name, "_"),
              typeof(void), arguments, propertyInfo.DeclaringType);
            ILGenerator generator = setter.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            generator.Emit(OpCodes.Ldarg_1);

            if (propertyInfo.PropertyType.IsClass)
                generator.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
            else
                generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);

            generator.EmitCall(OpCodes.Callvirt, setMethod, null);
            generator.Emit(OpCodes.Ret);

            /*
            * Create the delegate and return it
            */
            return (GenericSetter)setter.CreateDelegate(typeof(GenericSetter));
        }


        private static Action<object, object> Lambda(PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;
            Type type = propertyInfo.DeclaringType;

            ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "instance");
            ParameterExpression valueParameter = Expression.Parameter(typeof(object), "value");

            Expression<Action<object, object>> lambda = Expression.Lambda<Action<object, object>>(
                Expression.Assign(
                    Expression.Property(Expression.Convert(instanceParameter, type), propertyInfo),
                    Expression.Convert(valueParameter, propertyType)),
                instanceParameter,
                valueParameter
                );

            return lambda.Compile();
        }


        public class Stub
        {
            public string Value { get; set; }

        }

    }
}
